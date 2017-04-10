using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlanetControllerImpl : PlanetController
{
    
    private FleetController fleetController;
    public GameObject baseFleet;
    private Transform mapObject;
    private GameGameObject game;

    public PlanetControllerImpl(FleetController fleetCont, Transform mapObj, GameGameObject game, GameObject baseFleet)
    {
        fleetController = fleetCont;
        mapObject = mapObj;
        this.game = game;
        this.baseFleet = baseFleet;
    }

    /**
     * The types of queue items that are autobuilds
    */ 
    private static QueueItemType[] autoBuildTypes = EnumSet<QueueItemType>.of(QueueItemType.AutoAlchemy, QueueItemType.AutoMine, QueueItemType.AutoDefense,
                                                                        QueueItemType.AutoFactory, QueueItemType.AutoTerraform);


    /**
    * Create a new planet
    */
    public Planet makePlanet(string name, int x, int y)
    {
        return new Planet(name, x, y);

    }

    /**
    * Randomize a planet
    */
    public void randomise(Planet planet)
    {

    System.Random random = new System.Random();

    planet.setConcMinerals(new Mineral(random.Next(Consts.maxStartingConc) + Consts.minStartingConc, random.Next(Consts.maxStartingConc)
                                                                                                        + Consts.minStartingConc,
                                       random.Next(Consts.maxStartingConc) + Consts.minStartingConc));
        
    planet.setCargo(new Cargo(0, 0, 0, 0, 0));
    planet.setMineYears(new Mineral());
        
    int grav = random.Next(100);
    if (grav > 1)
    {
        grav = random.Next(89) + 10;
    }
    else
    {
        grav = (int)(11 - (float)(random.Next(100)) / 100.0 * 10.0);
    }

    int temp = random.Next(100);
    if (temp > 1)
    {
        temp = random.Next(89) + 10;
    }
    else
    {
        temp = (int)(11 - (float)(random.Next(100)) / 100.0 * 10.0);
    }

    int rad = random.Next(98) + 1;
        
    planet.setHab(new Hab(grav, temp, rad));
        
    planet.setMines(0);
    planet.setFactories(0);
    planet.setDefenses(0);
    planet.setPopulation(0);
    planet.setOwner(null);
    planet.getQueue().getItems().Clear();
    planet.setScanner(false);
}

/**
 * Grow a planet by whatever amount it grows in a year
 */
public void managePopulation(Planet planet)
{
    planet.setPopulation(planet.getPopulation() + planet.getGrowthAmount());
}

/**
 * Mine a planet, moving minerals from concentrations to to surface minerals
 */
public void mine(Planet planet)
{
    planet.setCargo(planet.getCargo().add(planet.getMineralOutput()));
    planet.setMineYears(planet.getMineYears().add(planet.getMines()));

    reduceMineralConcentrations(planet);
}

/**
 * Reduce the mineral concentrations of a planet after mining.
 */
private void reduceMineralConcentrations(Planet planet)
{

    for (int i = 0; i < 3; i++)
    {
        int conc = planet.getConcMinerals().getAtIndex(i);
        int minesPer = Consts.mineralDecayFactor / conc / conc;
        int mineYears = planet.getMineYears().getAtIndex(i);
        if (mineYears > minesPer)
        {
            conc -= mineYears / minesPer;
            if (planet.isHomeworld())
            {
                if (conc < Consts.minHWMineralConc)
                {
                    conc = Consts.minHWMineralConc;
                }
            }
            else
            {
                if (conc < Consts.minMineralConc)
                {
                    conc = Consts.minMineralConc;
                }
            }
            mineYears %= minesPer;

            planet.getMineYears().setAtIndex(i, mineYears);
            planet.getConcMinerals().setAtIndex(i, conc);
        }
    }

}

/**
 * Build anything in the production queue on the planet.
 */
public int buildAndProduce(Planet planet)
{
    Cost allocated = new Cost(planet.getCargo().getIronium(), planet.getCargo().getBoranium(), planet.getCargo().getGermanium(),
                              planet.getResourcesPerYearAvailable());

    allocated = allocated.add(planet.getQueue().getAllocated());
        
    int index = 0;
    while (index < planet.getQueue().getItems().Count)
    {
        ProductionQueueItem item = planet.getQueue().getItems()[index];
        Cost costPer = item.getCostOfOne(planet.getOwner().getRace());
        int numBuilt = allocated.divide(costPer);
            
        if (0 < numBuilt && numBuilt < item.getQuantity())
        {
            allocated = buildItem(planet, item, numBuilt, allocated);
                
            allocated = allocated.subtract(costPer.multiply(numBuilt));

            if (!autoBuildTypes.Contains(item.getType()))
            {
                planet.getQueue().getItems()[index].setQuantity(planet.getQueue().getItems()[index].getQuantity() - numBuilt);
            }
            
            allocated = allocateToQueue(planet.getQueue(), costPer, allocated);
        }
        else if (numBuilt >= item.getQuantity())
        {
            numBuilt = item.getQuantity();
            allocated = buildItem(planet, item, numBuilt, allocated);
                
            allocated = allocated.subtract(costPer.multiply(numBuilt));

            if (!autoBuildTypes.Contains(item.getType()))
            {
                planet.getQueue().getItems().RemoveAt(index);
                index--;
            }
            planet.getQueue().setAllocated(new Cost());
        }
        else
        {
            allocated = allocateToQueue(planet.getQueue(), costPer, allocated);
            break;
        }
        index++;
    }
    
    planet.setCargo(new Cargo(allocated.getIronium(), allocated.getBoranium(), allocated.getGermanium(), planet.getCargo().getColonists(),
                              planet.getCargo().getFuel()));
        
    return allocated.getResources();

}

/**
 * Build 1 or more items of this production queue item type Adding mines, factories, defenses,
 * etc to planets Building new fleets
 */
private Cost buildItem(Planet planet, ProductionQueueItem item, int num_built, Cost allocated)
{
    if (item.getType() == QueueItemType.Mine || item.getType() == QueueItemType.AutoMine)
    {
        planet.setMines(planet.getMines() + num_built);
        Message.mine(planet.getOwner(), planet, num_built);
    }
    else if (item.getType() == QueueItemType.Factory || item.getType() == QueueItemType.AutoFactory)
    {
        planet.setFactories(planet.getFactories() + num_built);
        Message.factory(planet.getOwner(), planet, num_built);
    }
    else if (item.getType() == QueueItemType.Defense || item.getType() == QueueItemType.AutoDefense)
    {
        planet.setDefenses(planet.getDefenses() + num_built);
        Message.defense(planet.getOwner(), planet, num_built);
    }
    else if (item.getType() == QueueItemType.Alchemy || item.getType() == QueueItemType.AutoAlchemy)
    {
        // add the minerals back to our allocated amount
        allocated = allocated.add(new Cost(num_built, num_built, num_built, 0));
    }
    else if(item.getType() == QueueItemType.Terraform || item.getType() == QueueItemType.AutoTerraform)
    {
            Race playerRace = planet.getOwner().getRace();

            int[] distanceFromIdeal = new int[3];
            distanceFromIdeal[0] = planet.getHab().getAtIndex(0) - playerRace.getHabCenter(0);
            distanceFromIdeal[1] = planet.getHab().getAtIndex(1) - playerRace.getHabCenter(1);
            distanceFromIdeal[2] = planet.getHab().getAtIndex(2) - playerRace.getHabCenter(2);

            for (int i = 0; i < 3; i++)
            {
                if (Mathf.Abs(distanceFromIdeal[i]) > item.getQuantity())
                {
                    if (planet.getHab().getAtIndex(i) > playerRace.getHabCenter(i))
                        planet.getHab().setAtIndex(i, planet.getHab().getAtIndex(i) + (item.getQuantity() * -1));
                    else if (planet.getHab().getAtIndex(i) < playerRace.getHabCenter(i))
                        planet.getHab().setAtIndex(i, planet.getHab().getAtIndex(i) + (item.getQuantity()));
                    Message.terraform(planet.getOwner(), planet, item.getQuantity(), i);
                }
                else
                    Message.noNeedToTerraform(planet.getOwner(), planet, i);
            }
        }
    else if (item.getType() == QueueItemType.Fleet)
    {
        buildFleet(planet, item, num_built);
    }
    else if (item.getType() == QueueItemType.Starbase)
    {
        buildStarbase(planet, item);
    }

    return allocated;
}

/**
 * Build a fleet and add it to the planet
 */
private void buildFleet(Planet planet, ProductionQueueItem item, int numBuilt)
{
    planet.getOwner().setNumFleetsBuilt(planet.getOwner().getNumFleetsBuilt() + 1);
    string name = (item.getFleetName() != null ? item.getFleetName() : string.Format("Fleet #" + planet.getOwner().getNumFleetsBuilt()));
    bool foundFleet = false;
    if (item.getFleetName() != null && planet.getOrbitingFleets().Count > 0)
    {
        foreach (Fleet fleet in planet.getOrbitingFleets())
        {
            if (fleet.getName().Equals(item.getFleetName()))
            {
                fleetController.addFleetToStack(fleet, new ShipStack(item.getShipDesign(), numBuilt));
                foundFleet = true;
                break;
            }
        }
    }
        if (!foundFleet)
        {
            Fleet fleet = fleetController.makeFleet(
                name,
                planet.getX(),
                planet.getY(),
                planet.getOwner());
            fleet.getShipStacks().Add(new ShipStack(item.getShipDesign(), item.getQuantity()));
            fleet.computeAggregate();

            GameObject go = GameObject.Instantiate(baseFleet, planet.PlanetGameObject.transform, false);
            go.transform.position = Vector3.zero;


            go.GetComponent<FleetGameObject>().setFleet(fleet);
            go.name = fleet.getName();
            go.SetActive(true);

            go.GetComponent<FleetGameObject>().getFleet().setFuel(fleet.getAggregate().getFuelCapacity());
            go.GetComponent<FleetGameObject>().getFleet().setOrbiting(planet);
            go.GetComponent<FleetGameObject>().getFleet().addWaypoint(fleet.getX(), fleet.getY(), 5, WaypointTask.None, planet);
            game.getGame().addFleet(go.GetComponent<FleetGameObject>().getFleet());
        }
}

/**
 * Build or upgrade the starbase on the planet
 */
private void buildStarbase(Planet planet, ProductionQueueItem item)
{
        if (planet.getStarbase() != null)
        {
            planet.getStarbase().getShipStacks().Clear();
            planet.getStarbase().getShipStacks().Add(new ShipStack(item.getShipDesign(), 1));
            planet.getStarbase().setDamage(0);
            planet.getStarbase().computeAggregate();
        }
        else
        {
            Fleet fleet = fleetController.makeFleet(planet.getName() + "-starbase", planet.getX(), planet.getY(), planet.getOwner());
            fleet.getShipStacks().Add(new ShipStack(item.getShipDesign(), 1));
            fleet.computeAggregate();
            fleet.addWaypoint(planet.getX(), planet.getY(), 5, WaypointTask.None, planet);
            planet.setStarbase(fleet);
            game.getGame().addFleet(fleet);
        }
}

/**
 *  Allocate resources to the top item on this production queue
 *  and return the leftover resources
 */
private Cost allocateToQueue(ProductionQueue queue, Cost costPer, Cost allocated)
{
    double ironiumPerc = (costPer.getIronium() > 0 ? (double)(allocated.getIronium()) / costPer.getIronium() : 100.0);
    double boraniumPerc = (costPer.getBoranium() > 0 ? (double)(allocated.getBoranium()) / costPer.getBoranium() : 100.0);
    double germaniumPerc = (costPer.getGermanium() > 0 ? (double)(allocated.getGermanium()) / costPer.getGermanium() : 100.0);
    double resourcesPerc = (costPer.getResources() > 0 ? (double)(allocated.getResources()) / costPer.getResources() : 100.0);
        
    double minPerc = Mathf.Min((float)ironiumPerc, Mathf.Min((float)boraniumPerc, Mathf.Min((float)germaniumPerc, (float)resourcesPerc)));
        
    queue.setAllocated(new Cost((int)(costPer.getIronium() * minPerc), (int)(costPer.getBoranium() * minPerc), (int)(costPer.getGermanium() * minPerc),
                                (int)(costPer.getResources() * minPerc)));
    return allocated.subtract(queue.getAllocated());
}

}
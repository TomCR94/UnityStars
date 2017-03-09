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
                                                                        QueueItemType.AutoFactory);


    /**
* Create a new planet
* 
* @param name The name of the planet
* @param x The x coord of the planet
* @param y The y coord of the planet
* @return The newly created planet
*/
    public Planet create(string name, int x, int y)
{
    return new Planet(name, x, y);

}

/**
 * Randomize a planet
 * 
 * @param planet the planet to randomize
 */
public void randomize(Planet planet)
{

    System.Random random = new System.Random();
    // mineral concentrations
    planet.setConcMinerals(new Mineral(random.Next(Consts.maxStartingConc) + Consts.minStartingConc, random.Next(Consts.maxStartingConc)
                                                                                                        + Consts.minStartingConc,
                                       random.Next(Consts.maxStartingConc) + Consts.minStartingConc));

    // surface minerals
    planet.setCargo(new Cargo(0, 0, 0, 0, 0));
    planet.setMineYears(new Mineral());

    // generate hab range of this planet
    int grav = random.Next(100);
    if (grav > 1)
    {
        // this is a "normal" planet, so put it in the 10 to 89 range
        grav = random.Next(89) + 10;
    }
    else
    {
        grav = (int)(11 - (float)(random.Next(100)) / 100.0 * 10.0);
    }

    int temp = random.Next(100);
    if (temp > 1)
    {
        // this is a "normal" planet, so put it in the 10 to 89 range
        temp = random.Next(89) + 10;
    }
    else
    {
        temp = (int)(11 - (float)(random.Next(100)) / 100.0 * 10.0);
    }

    int rad = random.Next(98) + 1;

    // set the hab range
    planet.setHab(new Hab(grav, temp, rad));

    // wipe out the planet
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
 * 
 * @param planet The planet to grow
 */
public void grow(Planet planet)
{
    planet.setPopulation(planet.getPopulation() + planet.getGrowthAmount());
}

/**
 * Mine a planet, moving minerals from concentrations to to surface minerals
 * 
 * @param planet the planet to mine
 */
public void mine(Planet planet)
{
    planet.setCargo(planet.getCargo().add(planet.getMineralOutput()));

    // add the number of mines we mined with this year
    // to our total mineyears for mineral concentration reduction
    planet.setMineYears(planet.getMineYears().add(planet.getMines()));

    reduceMineralConcentrations(planet);
}

/**
 * Reduce the mineral concentrations of a planet after mining.
 * 
 * @param planet The planet to reduce
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
 * 
 * @param planet The planet to build.
 */
public int build(Planet planet)
{

    // allocate surface minerals + resources not going to research
    Cost allocated = new Cost(planet.getCargo().getIronium(), planet.getCargo().getBoranium(), planet.getCargo().getGermanium(),
                              planet.getResourcesPerYearAvailable());

    // add the production queue's last turn resources
    allocated = allocated.add(planet.getQueue().getAllocated());

    // try and build each item in the queue, in order
    int index = 0;
    while (index < planet.getQueue().getItems().Count)
    {
        ProductionQueueItem item = planet.getQueue().getItems()[index];
        Cost costPer = item.getCostOfOne(planet.getOwner().getRace());
        int numBuilt = allocated.divide(costPer);

        // log.debug('Building item: %s cost_per: %s allocated: %s num_build: %s', item,
        // cost_per, allocated, num_built)
        // If we can build some, but not all
        if (0 < numBuilt && numBuilt < item.getQuantity())
        {
            // build however many we can
            allocated = buildItem(planet, item, numBuilt, allocated);

            // remove this cost from our allocated amount
            allocated = allocated.subtract(costPer.multiply(numBuilt));

            if (!autoBuildTypes.Contains(item.getType()))
            {
                // reduce the quantity
                planet.getQueue().getItems()[index].setQuantity(planet.getQueue().getItems()[index].getQuantity() - numBuilt);
            }

            // allocate the leftover resources to the remaining items
            allocated = allocateToQueue(planet.getQueue(), costPer, allocated);
        }
        else if (numBuilt >= item.getQuantity())
        {
            // only build the amount required
            numBuilt = item.getQuantity();
            allocated = buildItem(planet, item, numBuilt, allocated);

            // remove this cost from our allocated amount
            allocated = allocated.subtract(costPer.multiply(numBuilt));

            if (!autoBuildTypes.Contains(item.getType()))
            {
                // remove this item from the queue
                planet.getQueue().getItems().RemoveAt(index);
                index--;
            }
            // we built this completely, wipe out the allocated amount
            planet.getQueue().setAllocated(new Cost());
        }
        else
        {
            // allocate as many minerals/resources as we can to the queue
            // and break out of the loop, no more building will take place
            allocated = allocateToQueue(planet.getQueue(), costPer, allocated);
            break;
        }
        index++;
    }

    // reset surface minerals to whatever we have leftover
    planet.setCargo(new Cargo(allocated.getIronium(), allocated.getBoranium(), allocated.getGermanium(), planet.getCargo().getColonists(),
                              planet.getCargo().getFuel()));

    // return the resources we have left for research
    return allocated.getResources();

}

/**
 * Build 1 or more items of this production queue item type Adding mines, factories, defenses,
 * etc to planets Building new fleets
 * 
 * @param allocated
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
    // if (we have a fleetName defined for this queue item, try and append it
    // to that similarly named fleet if (it is orbiting this planet
    if (item.getFleetName() != null && planet.getOrbitingFleets().Count > 0)
    {
        foreach (Fleet fleet in planet.getOrbitingFleets())
        {
            if (fleet.getName().Equals(item.getFleetName()))
            {
                fleetController.merge(fleet, new ShipStack(item.getShipDesign(), numBuilt));
                foundFleet = true;
                break;
            }
        }
    }

        // if (we didn't have a fleet of that name, or it wasn't defined
        // just add this fleet as it's own entity
        if (!foundFleet)
        {
            Fleet fleet = fleetController.create(
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
            //planet.getOrbitingFleets().Add(go.GetComponent<FleetGameObject>().getFleet());
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
            // upgrade the existing starbase
            planet.getStarbase().getShipStacks().Clear();
            planet.getStarbase().getShipStacks().Add(new ShipStack(item.getShipDesign(), 1));
            planet.getStarbase().setDamage(0);
            planet.getStarbase().computeAggregate();
        }
        else
        {
            // create a new starbase
            Fleet fleet = fleetController.create(planet.getName() + "-starbase", planet.getX(), planet.getY(), planet.getOwner());
            fleet.getShipStacks().Add(new ShipStack(item.getShipDesign(), 1));
            fleet.computeAggregate();
            fleet.addWaypoint(planet.getX(), planet.getY(), 5, WaypointTask.None, planet);
            planet.setStarbase(fleet);
            game.getGame().addFleet(fleet);
        }
}

/**
 * <pre>
 *  Allocate resources to the top item on this production queue
 *  and return the leftover resources
 *   
 *  Costs are allocated by lowest percentage, i.e. if (we require
 *  Cost(10, 10, 10, 100) and we only have Cost(1, 10, 10, 100)
 *  we allocate Cost(1, 1, 1, 10)
 *   
 *  The min amount we have is 10 percent of the ironium, so we
 *  apply 10 percent to each cost amount
 * </pre>
 */
private Cost allocateToQueue(ProductionQueue queue, Cost costPer, Cost allocated)
{
    double ironiumPerc = (costPer.getIronium() > 0 ? (double)(allocated.getIronium()) / costPer.getIronium() : 100.0);
    double boraniumPerc = (costPer.getBoranium() > 0 ? (double)(allocated.getBoranium()) / costPer.getBoranium() : 100.0);
    double germaniumPerc = (costPer.getGermanium() > 0 ? (double)(allocated.getGermanium()) / costPer.getGermanium() : 100.0);
    double resourcesPerc = (costPer.getResources() > 0 ? (double)(allocated.getResources()) / costPer.getResources() : 100.0);

    // figure out the lowest percentage
    double minPerc = Mathf.Min((float)ironiumPerc, Mathf.Min((float)boraniumPerc, Mathf.Min((float)germaniumPerc, (float)resourcesPerc)));

    // allocate the lowest percentage of each cost
    queue.setAllocated(new Cost((int)(costPer.getIronium() * minPerc), (int)(costPer.getBoranium() * minPerc), (int)(costPer.getGermanium() * minPerc),
                                (int)(costPer.getResources() * minPerc)));
    // return the leftovers
    return allocated.subtract(queue.getAllocated());
}

}
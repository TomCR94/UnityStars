using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Turn processor for sending colony ships to planets to colonise and for building new colony ships
 */
public class ColoniserTurnProcessor : AbstractTurnProcessor
{

    public static ColoniserTurnProcessor instance;

    private void Awake()
    {
        instance = this;
    }
    private static double POP_DENSITY_REQUIRED = .33;

    /**
     * The required dock size
     */
    private static int DOCK_SIZE_REQUIRED = 0;

    protected bool isColoniserFleet(Fleet fleet)
    {
        if (fleet.getOwner().getID() == player.getID())
        {
            if (fleet.getAggregate().isColoniser())
            {
                return true;
            }
        }
        return false;
    }

    /**
     * Check if this is a colonisable planet
     */
    protected bool isColonisablePlanet(Planet planet)
    {
        PlanetKnowledge k = player.getPlanetKnowledge(planet);
        if (k != null)
        {
            if (planet.getOwner() == null && player.getRace().getPlanetHabitability(k.getHab()) > 0)
            {
                return true;
            }
        }
        return false;
    }

    /**
     * Process the turn by firing off scout ships
     */
    public new void process()
    {
        
        List<Planet> colonisablePlanets = new List<Planet>();
        List<Planet> buildablePlanets = new List<Planet>();
        foreach (Planet planet in game.getPlanets())
        {
            if (isColonisablePlanet(planet))
            {
                colonisablePlanets.Add(planet);
            }
            if (isBuildablePlanet(planet, DOCK_SIZE_REQUIRED) && planet.getPopulationDensity() > POP_DENSITY_REQUIRED)
            {
                buildablePlanets.Add(planet);
            }
        }
        List<Fleet> coloniserFleets = new List<Fleet>();
        foreach (Fleet fleet in game.getFleets())
        {
            if (isColoniserFleet(fleet))
            {
                if (fleet.getWaypoints().Count == 1)
                {
                    if (fleet.getOrbiting() != null && fleet.getOrbiting().getOwner() != null && fleet.getOrbiting().getOwner().getID() == fleet.getOwner().getID() && fleet.getOrbiting().getPopulationDensity() > POP_DENSITY_REQUIRED)
                    {
                        coloniserFleets.Add(fleet);
                    }
                }
                else
                {
                    MapObject target = fleet.getWaypoints()[1].getTarget();
                    if (target != null && target is Planet) {
            colonisablePlanets.Remove((Planet)target);
        }
        if (target == null)
        {
            Debug.Log(string.Format("Found a coloniser {0} going to a destination with no target!", fleet.getName()));
        }
    }

}
        }
        foreach (Fleet fleet in coloniserFleets) {
            Planet planetToColonise = closestPlanet(fleet, colonisablePlanets);
            if (planetToColonise != null) {
                Debug.Log(string.Format("{0} is targeting {1} for colonizing", fleet.getName(), planetToColonise.getName()));
                
                fleet.getCargo().setColonists(fleet.getAggregate().getCargoCapacity());
                fleet.getOrbiting().setPopulation(fleet.getOrbiting().getPopulation() - fleet.getAggregate().getCargoCapacity() * 100);
                
                fleet.addWaypoint(planetToColonise.getX(), planetToColonise.getY(), 5, WaypointTask.Colonise, planetToColonise);
                
                colonisablePlanets.Remove(planetToColonise);
            }
        }
        
        buildFleets(buildablePlanets, colonisablePlanets.Count);
    }
    
    /**
     * Build any new coloniser fleets
     */
    private void buildFleets(List<Planet> buildablePlanets, int numShipsNeeded)
{
    ShipDesign colonyShip = null;
    foreach (ShipDesign design in player.getDesigns())
    {
        if (design.getAggregate().isColoniser())
        {
            colonyShip = design;
            break;
        }
    }

    if (colonyShip != null)
    {

        int queuedToBeBuilt = 0;

        List<Planet> planetsToBuildOn = new List<Planet>();
        foreach (Planet planet in buildablePlanets)
        {
            bool isBuilding = false;
            foreach (ProductionQueueItem item in planet.getQueue().getItems())
            {
                if (item.getShipDesign() != null && item.getShipDesign().getAggregate().isColoniser())
                {
                    isBuilding = true;
                    queuedToBeBuilt++;
                }
            }
            
            if (!isBuilding)
            {
                planetsToBuildOn.Add(planet);
            }
        }

        foreach (Planet planet in planetsToBuildOn)
        {
            if (queuedToBeBuilt < numShipsNeeded)
            {
                planet.addQueueItem(QueueItemType.Fleet, 1, colonyShip);
                queuedToBeBuilt++;
            }
        }

    }

}
}
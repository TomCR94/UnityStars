using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Turn processor for sending colony ships to planets to colonize and for building new colony ships
 */
public class ColonizerTurnProcessor : AbstractTurnProcessor
{

    public static ColonizerTurnProcessor instance;

    private void Awake()
    {
        instance = this;
    }

    /**
     * the required population density required of a planet in order to suck people off of it
     * setting this to .33 because we don't want to suck people off a planet until it's reached the
     * max of its growth rate (over 1/3rd crowded)
     */
    private static double POP_DENSITY_REQUIRED = .33;

    /**
     * The required dock size
     */
    private static int DOCK_SIZE_REQUIRED = 0;

    protected bool isColonizerFleet(Fleet fleet)
    {
        if (fleet.getOwner().getID() == player.getID())
        {
            if (fleet.getAggregate().isColonizer())
            {
                return true;
            }
        }
        return false;
    }

    /**
     * Check if this is a colonizable planet
     * @param planet
     * @return
     */
    protected bool isColonizablePlanet(Planet planet)
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
    public void process()
    {

        // find all the planets we don't know about yet
        List<Planet> colonizablePlanets = new List<Planet>();
        List<Planet> buildablePlanets = new List<Planet>();
        foreach (Planet planet in player.getGame().getPlanets())
        {
            if (isColonizablePlanet(planet))
            {
                colonizablePlanets.Add(planet);
                Debug.Log("Found colonizable planet: " + planet.getName());
            }
            if (isBuildablePlanet(planet, DOCK_SIZE_REQUIRED) && planet.getPopulationDensity() > POP_DENSITY_REQUIRED)
            {
                buildablePlanets.Add(planet);
                Debug.Log("Found buildable planet: " + planet.getName());
            }
        }

        // get all the fleets that can scan and don't have waypoints yet
        List<Fleet> colonizerFleets = new List<Fleet>();
        foreach (Fleet fleet in player.getGame().getFleets())
        {
            if (isColonizerFleet(fleet))
            {
                if (fleet.getWaypoints().Count == 1)
                {
                    if (fleet.getOrbiting() != null && fleet.getOrbiting().getOwner() != null && fleet.getOrbiting().getOwner().getID() == fleet.getOwner().getID() && fleet.getOrbiting().getPopulationDensity() > POP_DENSITY_REQUIRED)
                    {
                        colonizerFleets.Add(fleet);
                    }
                }
                else
                {
                    // this fleet is already doing something, if it's targeting a planet
                    // remove the planet from our list
                    MapObject target = fleet.getWaypoints()[1].getTarget();
                    if (target != null && target is Planet) {
            colonizablePlanets.Remove((Planet)target);
        }
        if (target == null)
        {
            Debug.Log("Found a colonizer {} going to a destination with no target!", fleet);
        }
    }

}
        }

        // go through each scanner fleet and find it a new planet to scout
        foreach (Fleet fleet in colonizerFleets) {
            Planet planetToColonize = closestPlanet(fleet, colonizablePlanets);
            if (planetToColonize != null) {
                Debug.Log(string.Format("{} is targeting {} for colonizing", fleet, planetToColonize));
                
                // fill the fleet with cargo
                fleet.getCargo().setColonists(fleet.getAggregate().getCargoCapacity());
                fleet.getOrbiting().setPopulation(fleet.getOrbiting().getPopulation() - fleet.getAggregate().getCargoCapacity() * 100);

                // add this planet as a waypoint
                fleet.addWaypoint(planetToColonize.getX(), planetToColonize.getY(), 5, WaypointTask.Colonize, planetToColonize);

                // remove this planet from our list
                colonizablePlanets.Remove(planetToColonize);
            }
        }

        // build additional colony ship fleets
        buildFleets(buildablePlanets, colonizablePlanets.Count);
    }
    
    /**
     * Build any new colonizer fleets
     */
    private void buildFleets(List<Planet> buildablePlanets, int numShipsNeeded)
{

    // find the first colony ship design
    // TODO: pick the best one
    ShipDesign colonyShip = null;
    foreach (ShipDesign design in player.getDesigns())
    {
        if (design.getAggregate().isColonizer())
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
                if (item.getShipDesign() != null && item.getShipDesign().getAggregate().isColonizer())
                {
                    isBuilding = true;
                    queuedToBeBuilt++;
                    Debug.Log(string.Format("planet {} is already buildinga colony ship", planet.getName()));
                }
            }

            // if this planet isn't already building a colony ship, build one
            if (!isBuilding)
            {
                planetsToBuildOn.Add(planet);
            }
        }

        foreach (Planet planet in planetsToBuildOn)
        {
            // if this planet isn't building a colony ship already, add
            // one to the queue
            if (queuedToBeBuilt < numShipsNeeded)
            {
                planet.addQueueItem(QueueItemType.Fleet, 1, colonyShip);
                Debug.Log("Added colony ship to planet queue: " + planet.getName());
                queuedToBeBuilt++;
            }
        }

    }

}
}
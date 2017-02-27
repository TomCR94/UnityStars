using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutTurnProcessor : AbstractTurnProcessor {

    public static ScoutTurnProcessor instance;

    private void Awake()
    {
        instance = this;
    }

    /**
     * Process the turn by firing off scout ships
     */
    public void process()
    {

        // find the first colony ship design
        // TODO: pick the best one
        ShipDesign scoutShip = null;
        foreach (ShipDesign design in player.getDesigns())
        {
            if (design.getHullName().Equals("Scout"))
            {
                scoutShip = design;
                break;
            }
        }

        // find all the planets we don't know about yet
        List<Planet> unknownPlanets = new List<Planet>();
        List<Planet> buildablePlanets = new List<Planet>();
        foreach (Planet planet in player.getGame().getPlanets())
        {
            if (!player.hasKnowledge(planet))
            {
                unknownPlanets.Add(planet);
            }
            else if (scoutShip != null && isBuildablePlanet(planet, scoutShip.getAggregate().getMass()))
            {
                buildablePlanets.Add(planet);
            }
        }

        // get all the fleets that can scan and don't have waypoints yet
        List<Fleet> scannerFleets = new List<Fleet>();
        foreach (Fleet fleet in player.getGame().getFleets())
        {
            if (fleet.getOwner().getID() == player.getID() && fleet.canScan())
            {
                // if we got here this fleet can scan
                if (fleet.getWaypoints().Count <= 1)
                {
                    // if this fleet isn't already scanning something, add it to the list
                    scannerFleets.Add(fleet);
                }
                else
                {
                    // this fleet is already doing something, if it's targetting a planet
                    // remove the planet from our list
                    MapObject target = fleet.getWaypoints()[1]. getTarget();
                    if (target != null && target is Planet) {
            unknownPlanets.Remove((Planet)target);
        }
    }
}
        }
        
        // go through each scanner fleet and find it a new planet to scout
        foreach (Fleet fleet in scannerFleets) {
            Planet planetToScout = closestPlanet(fleet, unknownPlanets);
            if (planetToScout != null) {
                // add this planet as a waypoint
                fleet.addWaypoint(planetToScout.getX(), planetToScout.getY(), 5, WaypointTask.None, planetToScout);
                
                // remove this planet from our list
                unknownPlanets.Remove(planetToScout);
            }
        }


        buildFleets(buildablePlanets, unknownPlanets.Count, scoutShip);
        
    }
    
    /**
     * Build any new colonizer fleets
     */
    private void buildFleets(List<Planet> buildablePlanets, int numShipsNeeded, ShipDesign scoutShip)
{
    if (scoutShip != null)
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
                    Debug.Log(string.Format("planet {} is already building a scout ship", planet.getName()));
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
                planet.addQueueItem(QueueItemType.Fleet, 1, scoutShip);
                Debug.Log("Added scout ship to planet queue: " + planet.getName());
                queuedToBeBuilt++;
            }
        }

    }

}
}

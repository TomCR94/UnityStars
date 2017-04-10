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
    public new void process()
    {
        ShipDesign scoutShip = null;
        foreach (ShipDesign design in player.getDesigns())
        {
            if (design.getHullName().Equals("Scout"))
            {
                scoutShip = design;
                break;
            }
        }
        
        List<Planet> unknownPlanets = new List<Planet>();
        List<Planet> buildablePlanets = new List<Planet>();
        foreach (Planet planet in game.getPlanets())
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
        
        List<Fleet> scannerFleets = new List<Fleet>();
        foreach (Fleet fleet in game.getFleets())
        {
            if (fleet.getOwner().getID() == player.getID() && fleet.canScan())
            {
                if (fleet.getWaypoints().Count <= 1)
                {
                    scannerFleets.Add(fleet);
                }
                else
                {
                    MapObject target = fleet.getWaypoints()[1]. getTarget();
                    if (target != null && target is Planet) {
            unknownPlanets.Remove((Planet)target);
        }
    }
}
        }
        
        foreach (Fleet fleet in scannerFleets) {
            Planet planetToScout = closestPlanet(fleet, unknownPlanets);
            if (planetToScout != null) {
                fleet.addWaypoint(planetToScout.getX(), planetToScout.getY(), 5, WaypointTask.None, planetToScout);
                
                unknownPlanets.Remove(planetToScout);
            }
        }


        buildFleets(buildablePlanets, unknownPlanets.Count, scoutShip);
        
    }
    
    /**
     * Build any new scout fleets
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
                planet.addQueueItem(QueueItemType.Fleet, 1, scoutShip);
                queuedToBeBuilt++;
            }
        }

    }

}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface FleetController
{

    /**
     * Create a new fleet
     */
    Fleet makeFleet(string name, int x, int y, Player owner);

    /**
     * Move this fleet towards it's goal <br/>
     */
    void moveFleetTowards(Fleet fleet);

    /**
     * Process the task for a given waypoint
     */
    void doTask(Fleet fleet, Waypoint wp);

    /**
     * Scrap this fleet, adding resources to the waypoint
     */
    void destroyAndDistributeResources(Fleet fleet, Waypoint wp);

    /**
     * Take this fleet and have it colonise a planet
     */
    void colonise(Fleet fleet, Waypoint wp);

    /**
     * Merge a ship_stack with this fleet, combining ship_stacks if the same design is found
     */
    void addFleetToStack(Fleet fleet, ShipStack stack);

    /**
     * Fuel usage calculation
     */
    int getFuelCost(int speed, int mass, double dist, double ifeFactor, TechEngine engine);

    /**
     * Get the fuel cost for a fleet
     */
    int getFuelCost(Fleet fleet, int speed, double dist);
}
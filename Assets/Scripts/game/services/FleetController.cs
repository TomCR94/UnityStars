using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface FleetController
{

    /**
     * Create a new fleet
     * @param name The name of the fleet
     * @param x the x coord
     * @param y the y coord
     * @param owner the owner
     * @return The newly created fleet
     */
    Fleet create(string name, int x, int y, Player owner);

    /**
     * Move this fleet towards it's goal <br/>
     * TODO: Still need to have this account for fleets chasing fleets and other strange
     * circumstances
     * 
     * @param fleet The fleet to move
     */
    void move(Fleet fleet);

    /**
     * Process the task for a given waypoint
     * @param fleet The fleet to process
     * @param wp The waypoint with the task
     */
    void processTask(Fleet fleet, Waypoint wp);

    /**
     * Scrap this fleet, adding resources to the waypoint
     * @param fleet The fleet to scrap
     */
    void scrap(Fleet fleet, Waypoint wp);

    /**
     * Take this fleet and have it colonize a planet
     * @param fleet The fleet to colonize
     * @param wp The waypoint pointing to the target planet to colonize
     */
    void colonize(Fleet fleet, Waypoint wp);

    /**
     * Merge a ship_stack with this fleet, combining ship_stacks if the same design is found
     * 
     * @param fleet The fleet to merge into
     * @param stack The stack to merge
     */
    void merge(Fleet fleet, ShipStack stack);

    /**
     * Fuel usage calculation courtesy of m.a@stars
     * 
     * @param speed The warp speed 1 to 10
     * @param mass The mass of the fleet
     * @param dist The distance travelled
     * @param ifeFactor The factor for improved fuel efficiency (.85 if you have the LRT)
     * @param engine The engine being used
     * @return The amount of mg of fuel used
     */
    int getFuelCost(int speed, int mass, double dist, double ifeFactor, TechEngine engine);

    /**
     * Get the fuel cost for a fleet
     * @param fleet The fleet to check
     * @param speed The speed to check for
     * @param dist The distance traveled
     * @return The amount of mg of fuel used
     */
    int getFuelCost(Fleet fleet, int speed, double dist);
}
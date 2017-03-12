using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetControllerImpl : FleetController {
    

    /**
     * Create a new fleet
     * @param name The name of the fleet
     * @param x the x coord
     * @param y the y coord
     * @param owner the owner
     * @return The newly created fleet
     */
    public Fleet create(string name, int x, int y, Player owner)
    {
        return new Fleet(name, x, y, owner);
    }

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
    public int getFuelCost(int speed, int mass, double dist, double ifeFactor, TechEngine engine)
    {
        // 1 mg of fuel will move 200 kt of weight 1 LY at a Fuel Usage Number of 100.
        // Number of engines doesn't matter. Neither number of ships with the same engine.

        double distanceCeiling = Mathf.Ceil((float)dist); // rounding to next integer gives best graph fit
        // window.status = 'Actual distance used is ' + Distan + 'ly';

        // IFE is applied to drive specifications, just as the helpfile hints.
        // Stars! probably does it outside here once per turn per engine to save time.
        double engineEfficiency = Mathf.Ceil((float)ifeFactor * engine.getFuelUsage()[speed - 1]);

        // 20000 = 200*100
        // Safe bet is Stars! does all this with integer math tricks.
        // Subtracting 2000 in a loop would be a way to also get the rounding.
        // Or even bitshift for the 2 and adjust "decimal point" for the 1000
        double teorFuel = (Mathf.Floor((float)(mass * engineEfficiency * distanceCeiling / 2000) / 10));
        // using only one decimal introduces another artifact: .0999 gets rounded down to .0

        // The heavier ships will benefit the most from the accuracy
        int intFuel = (int)Mathf.Ceil((float)teorFuel);

        // That's all. Nothing really fancy, much less random. Subtle differences in
        // math lib workings might explain the rarer and smaller discrepancies observed
        return intFuel;
        // Unrelated to this fuel math are some quirks inside the
        // "negative fuel" watchdog when the remainder of the
        // trip is < 1 ly. Aahh, the joys of rounding! ;o)
    }

    public int getFuelCost(Fleet fleet, int speed, double dist)
    {
        // figure out how much fuel we're going to use
        double ifeFactor = fleet.getOwner().getRace().hasLRT(LRT.IFE) ? .85 : 1.0;

        int fuelCost = 0;

        // compute each ship stack separately
        foreach (ShipStack stack in fleet.getShipStacks())
        {
            // figure out this ship stack's mass as well as it's proportion of the cargo
            int mass = stack.getDesign().getAggregate().getMass() * stack.getQuantity();
            int fleetCargo = fleet.getCargo().getTotal();
            int stackCapacity = stack.getDesign().getAggregate().getCargoCapacity() * stack.getQuantity();
            int fleetCapacity = fleet.getAggregate().getCargoCapacity();

            if (fleetCapacity > 0)
            {
                mass += (int)((float)fleetCargo * ((float)stackCapacity / (float)fleetCapacity));
            }
            fuelCost += getFuelCost(speed, mass, dist, ifeFactor, stack.getDesign().getAggregate().getEngine());
        }

        return fuelCost;
    }

    public void move(Fleet fleet)
    {
        if (fleet.getWaypoints().Count > 1)
        {
            Waypoint wp0 = fleet.getWaypoints()[0];
            Waypoint wp1 = fleet.getWaypoints()[1];
            int totaldist = (int)(MapObject.realDist(fleet.getX(), fleet.getY(), wp1.getX(), wp1.getY()));
            int dist = wp1.getSpeed() * wp1.getSpeed();

            // go with the lower
            if (totaldist < dist)
            {
                dist = totaldist;
            }

            // get the cost for the fleet
            int fuelCost = getFuelCost(fleet, wp1.getSpeed(), dist);
            fleet.setFuel(fleet.getFuel() - fuelCost);

            if (totaldist == dist)
            {
                fleet.setX(wp1.getX());
                fleet.setY(wp1.getY());
                if (wp1.getTarget() != null && wp1.getTarget() is Planet) {
                    Planet planet = (Planet)wp1.getTarget();
                    fleet.setOrbiting(planet);

                    if (wp1.getTask() == WaypointTask.Colonize)
                        colonize(fleet, wp1);
                }

                // we've arrive, process this waypoint
                /*
                processTask(fleet, wp1);
                fleet.getWaypoints().remove(0);
                if (fleet.getWaypoints().size() == 1) {
                    Message.fleetCompletedAssignedOrders(fleet.getOwner(), fleet);
                }
                */
            }
            else
            {
                // move this fleet closer to the next waypoint
                fleet.setX(fleet.getX() + (int)((wp1.getX() - fleet.getX()) * ((float)(dist) / (float)(totaldist))));
                fleet.setY(fleet.getY() + (int)((wp1.getY() - fleet.getY()) * ((float)(dist) / (float)(totaldist))));

                
                wp0.setX(fleet.getX());
                wp0.setY(fleet.getY());
                fleet.setOrbiting(null);
                wp0.setTarget(null);
            }
        }
    }

    /**
     * Process the task for a given waypoint
     * 
     * @param fleet The fleet to process
     * @param wp The waypoint with the task
     */
    public void processTask(Fleet fleet, Waypoint wp)
    {

        switch (wp.getTask())
        {
            case WaypointTask.Colonize:
                colonize(fleet, wp);
                break;
            case WaypointTask.LayMineField:
                break;
            case WaypointTask.MergeWithFleet:
                break;
            case WaypointTask.Patrol:
                break;
            case WaypointTask.RemoteMining:
                break;
            case WaypointTask.Route:
                break;
            case WaypointTask.ScrapFleet:
                scrap(fleet, wp);
                break;
            case WaypointTask.TransferFleet:
                break;
            case WaypointTask.Transport:
                break;

        }
    }

    /**
     * Scrap this fleet, adding resources to the waypoint
     * 
     * @param fleet The fleet to scrap
     */
    public void scrap(Fleet fleet, Waypoint wp)
    {
        Cost cost = new Cost(fleet.getAggregate().getCost());
        cost = cost.add(new Cost(fleet.getCargo().getIronium(), fleet.getCargo().getBoranium(), fleet.getCargo().getGermanium(), 0));
        if (PlanetDictionary.instance.getPlanetForID(wp.getTarget().getName()) != null)
        {
            Planet planet = PlanetDictionary.instance.getPlanetForID(wp.getTarget().getName());
            planet.setCargo(planet.getCargo().add(new Mineral(cost.getIronium(), cost.getBoranium(), cost.getGermanium())));
            fleet.setOrbiting(null);
            fleet.setScrapped(true);
            FleetDictionary.instance.fleetDict.Remove(fleet.getID());
            GameObject.Destroy(fleet.FleetGameObject);
        }
    }

    /**
     * Take this fleet and have it colonize a planet
     * 
     * @param fleet The fleet to colonize
     * @param wp The waypoint pointing to the target planet to colonize
     */
    public void colonize(Fleet fleet, Waypoint wp)
    {
        Planet planet = PlanetDictionary.instance.getPlanetForID(wp.getTarget().getName());
        if (planet == null)
        {
            Message.colonizeNonPlanet(fleet.getOwner(), fleet);
        }
        else if (planet.getOwner() != null)
        {
            Message.colonizeOwnedPlanet(fleet.getOwner(), fleet);
        }
        else if (!fleet.getAggregate().isColonizer())
        {
            Message.colonizeWithNoModule(fleet.getOwner(), fleet);
        }
        else if (fleet.getCargo().getColonists() <= 0)
        {
            Message.colonizeWithNoColonists(fleet.getOwner(), fleet);
        }
        else
        {
            planet.setOwner(fleet.getOwner());
            planet.setPopulation(fleet.getCargo().getColonists() * 100);
            fleet.getCargo().setColonists(0);
            scrap(fleet, wp);
        }
    }

    /**
     * Merge a ship_stack with this fleet, combining ship_stacks if the same design is found
     * 
     * @param fleet The fleet to merge into
     * @param stack The stack to merge
     */
    public void merge(Fleet fleet, ShipStack stack)
    {
        bool found_stack = false;
        foreach (ShipStack fleetStack in fleet.getShipStacks())
        {
            if (stack.getDesign().getName().Equals(fleetStack.getDesign().getName()))
            {
                fleetStack.setQuantity(fleetStack.getQuantity() + stack.getQuantity());
                found_stack = true;
                break;
            }
        }

        if (!found_stack)
        {
            fleet.getShipStacks().Add(stack);
        }
    }

}
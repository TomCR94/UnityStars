using System;
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
                if (wp1.getTarget() != null && wp1.getTarget() is Planet)
                {
                    Planet planet = (Planet)wp1.getTarget();
                    fleet.setOrbiting(planet);

                    if (wp1.getTask() == WaypointTask.Colonize)
                        colonize(fleet, wp1);
                    else if (wp1.getTask() == WaypointTask.Bomb)
                        Bomb(fleet);
                    else if (wp1.getTask() == WaypointTask.Invade)
                        Invade(fleet, wp1);
                    else if (wp1.getTask() == WaypointTask.UnloadCargo)
                        UnloadCargo(fleet, wp1);
                    else if (wp1.getTask() == WaypointTask.ScrapFleet)
                        scrap(fleet, wp1);
                    else if (wp1.getTask() == WaypointTask.Terraform)
                        Terraform(fleet, wp1);
                }
                else if (wp1.getTarget() != null && wp1.getTarget() is Wormhole)
                {
                    if (wp1.getTask() == WaypointTask.Stabilize)
                        Stabilize(fleet, wp1);
                    else
                    {

                        Wormhole wormhole = WormholeDictionary.instance.getWormholeForID(wp1.getTarget().getID());

                        Wormhole twin = wormhole.getTwin();
                        fleet.setX(twin.getX());
                        fleet.setY(twin.getY());

                        Message.Warped(fleet, fleet.getOwner(), wormhole, twin);
                    }
                }

                    fleet.getWaypoints().RemoveAt(1);
                if (fleet.getWaypoints().Count == 1) {
                    Message.fleetCompletedAssignedOrders(fleet.getOwner(), fleet);
                }
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

    private void Stabilize(Fleet fleet, Waypoint wp1)
    {
        if (WormholeDictionary.instance.getWormholeForID(wp1.getTarget().getID()) == null)
        {
            Message.NotAWormHole(fleet, fleet.getOwner(), wp1.getTarget());
            return;
        }

        if (WormholeDictionary.instance.getWormholeForID(wp1.getTarget().getID()).getStabilized())
        {
            Message.AlreadyStabilized(fleet, fleet.getOwner(), wp1.getTarget());
            return;
        }

        if (fleet.getCargo().getIronium() < 10000)
        {
            Message.CannotAffordStabilize(fleet, fleet.getOwner(), wp1.getTarget());
            return;
        }

        fleet.getCargo().addIronium(-10000);
        WormholeDictionary.instance.getWormholeForID(wp1.getTarget().getID()).setStabilized(true);
        Message.Stabilized(fleet, fleet.getOwner(), wp1.getTarget());
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
            case WaypointTask.Invade:
                Invade(fleet, wp);
                break;
            case WaypointTask.ScrapFleet:
                scrap(fleet, wp);
                break;
            case WaypointTask.UnloadCargo:
                UnloadCargo(fleet, wp);
                break;
            case WaypointTask.Terraform:
                Terraform(fleet, wp);
                break;
            case WaypointTask.Bomb:
                Bomb(fleet);
                break;
            case WaypointTask.Stabilize:
                Stabilize(fleet, wp);
                break;
        }
    }

    private void Terraform(Fleet fleet, Waypoint wp)
    { 
    }

    private void Bomb(Fleet fleet)
    {
        if (fleet.getOrbiting() == null)
        {
            Message.BombInvalidTarget(fleet.getOwner(), fleet);
            return;
        }
        Planet planet = fleet.getOrbiting();

        if (planet.getCargo().getColonists() == 0 || planet.getStarbase() != null)
        {
            Message.BombNoOne(fleet.getOwner(), fleet, planet);
            return;
        }

        // If we don't have bombers then there is nothing more to do here

        if (!fleet.getAggregate().isBomber())
        {
            Message.BombNotBomber(fleet.getOwner(), fleet);
            return;
        }
        // Bomb colonists
        double killFactor = fleet.getAggregate().getKillPop();
        double defenseFactor = 1.0 - planet.getDefenses()/100f;
        double populationKill = killFactor * defenseFactor;
        double killed = (double)planet.getCargo().getColonists() * populationKill;

        double minKilled = fleet.getAggregate().getMinKill()
                              * (1 - planet.getDefenses() / 100f);

        int dead = (int)Math.Max(killed, minKilled);
        planet.getCargo().addColonists(-dead);


        // Get installation details
        double totalBuildings = planet.getMines() + planet.getFactories() + planet.getDefenses();

        double buildingKills = fleet.getAggregate().getKillPop() * (1 - planet.getDefenses() / 100f);
        double damagePercent = buildingKills / totalBuildings;

        if (damagePercent > 1)
        {
            damagePercent = 1;
        }

        // We now have the percentage of each building type to destroy (which
        // has been clamped at a maximum of 100% (normalised so that 100% =
        // 1). Let's apply that percentage to each building type in
        // turn. First Defenses:

        // Defenses
        int defensesDestroyed = (int)((double)planet.getDefenses() * damagePercent);
        planet.setDefenses(planet.getDefenses() - defensesDestroyed);

        // Now Factories
        int factoriesDestroyed = (int)(planet.getFactories() * damagePercent);
        planet.setFactories(planet.getFactories() - factoriesDestroyed);

        // Now Mines
        int minesDestroyed = (int)(planet.getMines() * damagePercent);
        planet.setMines(planet.getMines() - minesDestroyed);

        if (planet.getCargo().getColonists() > 0)
        {
            Message.BombKillSome(fleet.getOwner(), fleet, planet, dead, defensesDestroyed, factoriesDestroyed, minesDestroyed);
        }
        else
        {
            Message.BombKillAll(fleet.getOwner(), fleet, planet);

            // clear out the colony
            planet.getQueue().getItems().Clear();
            planet.setCargo(new Cargo(0, 0, 0, 0, 0));
            planet.setOwner(null);
        }
    }

    private void UnloadCargo(Fleet fleet, Waypoint wp)
    {
        if (fleet.getOrbiting() == null)
        {
            Message.unloadNotInOrbit(fleet.getOwner(), fleet);
            return;
        }

        Planet targetPlanet = fleet.getOrbiting();
        
        Message.unloadInOrbit(fleet.getOwner(), fleet, targetPlanet);

        fleet.getWaypoints().RemoveAt(0);

        targetPlanet.getCargo().addIronium(fleet.getCargo().getIronium());
        targetPlanet.getCargo().addBoranium(fleet.getCargo().getBoranium());
        targetPlanet.getCargo().addGermanium(fleet.getCargo().getGermanium());

        fleet.getCargo().setIronium(0);
        fleet.getCargo().setBoranium(0);
        fleet.getCargo().setGermanium(0);

        // check if this is normal transportation or an invasion
        if (fleet.getOwner().getID() != targetPlanet.getOwner().getID() && fleet.getCargo().getColonists() != 0)
        {
            Invade(fleet, wp);
        }
        else
        {
            targetPlanet.getCargo().addColonists(fleet.getCargo().getColonists());
            fleet.getCargo().setColonists(0);
        }

    }

    private void Invade(Fleet fleet, Waypoint wp)
    {
        // First check that we are actuallly in orbit around a planet.

        if (fleet.getOrbiting() == null)
        {
            Message.InvadeNotOrbiting(fleet.getOwner(), fleet);
            return;
        }

        // and that we have troops.

        int troops = fleet.getCargo().getColonists();
        Planet planet = fleet.getOrbiting(); ;

        if (planet.getOwner() == null)
        {
            colonize(fleet, wp);
        }

        if (troops == 0)
        {
            Message.InvadeNoTroops(fleet.getOwner(), fleet, planet);
            return;
        }

        // Consider the diplomatic situation
        if (fleet.getOwner().getID() == planet.getOwner().getID())
        {
            // already own this planet, so colonists can beam down safely
            planet.getCargo().addColonists(troops);
            fleet.getCargo().setColonists(0);
            Message.InvadeAlreadyOwned(fleet.getOwner(), fleet, planet);
            return;
        }

        // check for starbase
        if (planet.getStarbase() != null)
        {
            Message.InvadeStarBase(fleet.getOwner(), fleet, planet);
            return;
        }

        // The troops are now committed to take the star or die trying
        fleet.getCargo().setColonists(0);

        // Take into account the Defenses
        int troopsOnGround = (int)(troops * (1 - (planet.getDefenses()/100)));

        // Apply defender and attacker bonuses
        double attackerBonus = 1.1;
        if (fleet.getOwner().getRace().getPRT() == PRT.WM)//WM
            attackerBonus *= 1.5;

        double defenderBonus = 1.0;
        if (planet.getOwner().getRace().getPRT() == PRT.IS)//IS
            defenderBonus *= 2.0;

        int defenderStrength = (int)(planet.getCargo().getColonists() * defenderBonus);
        int attackerStrength = (int)(troopsOnGround * attackerBonus);
        int survivorStrength = defenderStrength - attackerStrength; // will be negative if attacker wins
        
        if (survivorStrength > 0)
        {
            // defenders win
            int remainingDefenders = (int)(survivorStrength / defenderBonus);
            int defendersKilled = planet.getCargo().getColonists()- remainingDefenders;
            planet.getCargo().setColonists(remainingDefenders);

            Message.InvadeAttackersSlain(fleet.getOwner(), planet, defendersKilled);
        }
        else if (survivorStrength < 0)
        {
            // attacker wins
            planet.getQueue().getItems().Clear();
            int remainingAttackers = (int)(-survivorStrength / attackerBonus);
            int attackersKilled = troops - remainingAttackers;
            planet.getCargo().setColonists(remainingAttackers);
            planet.setOwner(fleet.getOwner());

            Message.InvadeDefendersSlain(fleet.getOwner(), planet, attackersKilled);
        }
        else
        {
            // no survivors!
            Message.InvadeDraw(fleet.getOwner(), planet);

            // clear out the colony
            planet.getQueue().getItems().Clear();
            planet.setCargo(new Cargo(0, 0, 0, 0, 0));
            planet.setOwner(null);
        }
    }

    /**
     * Scrap this fleet, adding resources to the waypoint
     * 
     * @param fleet The fleet to scrap
     */
    public void scrap(Fleet fleet, Waypoint wp)
    {
        Debug.Log("Scrap");
        Cost cost = new Cost(fleet.getAggregate().getCost());
        cost = cost.add(new Cost(fleet.getCargo().getIronium(), fleet.getCargo().getBoranium(), fleet.getCargo().getGermanium(), 0));
        Debug.Log("PlanetDict" + wp.getTarget().getName());
        if (PlanetDictionary.instance.getPlanetForID(wp.getTarget().getName()) != null)
        {
            Planet planet = PlanetDictionary.instance.getPlanetForID(wp.getTarget().getName());
            Debug.Log("Planet exists " + planet.getName());
            planet.setCargo(planet.getCargo().add(new Mineral(cost.getIronium(), cost.getBoranium(), cost.getGermanium())));
            fleet.setOrbiting(null);
            fleet.setScrapped(true);
            FleetDictionary.instance.fleetDict.Remove(fleet.getID());
            GameObject.Destroy(fleet.FleetGameObject.gameObject);
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
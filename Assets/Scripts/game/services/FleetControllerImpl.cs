using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetControllerImpl : FleetController {
    

    /**
     * Create a new fleet
     */
    public Fleet makeFleet(string name, int x, int y, Player owner)
    {
        return new Fleet(name, x, y, owner);
    }

    /**
     * Fuel usage calculation
     */
    public int getFuelCost(int speed, int mass, double dist, double ifeFactor, TechEngine engine)
    {

        double distanceCeiling = Mathf.Ceil((float)dist); 
        double engineEfficiency = Mathf.Ceil((float)ifeFactor * engine.getFuelUsage()[speed - 1]);
        double teorFuel = (Mathf.Floor((float)(mass * engineEfficiency * distanceCeiling / 2000) / 10));
        
        int intFuel = (int)Mathf.Ceil((float)teorFuel);
        return intFuel;
    }

    public int getFuelCost(Fleet fleet, int speed, double dist)
    {
        double ifeFactor = fleet.getOwner().getRace().hasLRT(LRT.IFE) ? .85 : 1.0;

        int fuelCost = 0;
        
        foreach (ShipStack stack in fleet.getShipStacks())
        {
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

    public void moveFleetTowards(Fleet fleet)
    {
        if (fleet.getWaypoints().Count > 1)
        {
            Waypoint wp0 = fleet.getWaypoints()[0];
            Waypoint wp1 = fleet.getWaypoints()[1];
            int totaldist = (int)(MapObject.realDist(fleet.getX(), fleet.getY(), wp1.getX(), wp1.getY()));
            int dist = wp1.getSpeed() * wp1.getSpeed();
            
            if (totaldist < dist)
            {
                dist = totaldist;
            }
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

                    if (wp1.getTask() == WaypointTask.Colonise)
                        colonise(fleet, wp1);
                    else if (wp1.getTask() == WaypointTask.Bomb)
                        Bomb(fleet);
                    else if (wp1.getTask() == WaypointTask.Invade)
                        Invade(fleet, wp1);
                    else if (wp1.getTask() == WaypointTask.UnloadCargo)
                        UnloadCargo(fleet, wp1);
                    else if (wp1.getTask() == WaypointTask.ScrapFleet)
                        destroyAndDistributeResources(fleet, wp1);
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
     */
    public void doTask(Fleet fleet, Waypoint wp)
    {

        switch (wp.getTask())
        {
            case WaypointTask.Colonise:
                colonise(fleet, wp);
                break;
            case WaypointTask.Invade:
                Invade(fleet, wp);
                break;
            case WaypointTask.ScrapFleet:
                destroyAndDistributeResources(fleet, wp);
                break;
            case WaypointTask.UnloadCargo:
                UnloadCargo(fleet, wp);
                break;
            case WaypointTask.Bomb:
                Bomb(fleet);
                break;
            case WaypointTask.Stabilize:
                Stabilize(fleet, wp);
                break;
        }
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

        if (!fleet.getAggregate().isBomber())
        {
            Message.BombNotBomber(fleet.getOwner(), fleet);
            return;
        }
        double killFactor = fleet.getAggregate().getKillPop();
        double defenseFactor = 1.0 - planet.getDefenses()/100f;
        double populationKill = killFactor * defenseFactor;
        double killed = (double)planet.getCargo().getColonists() * populationKill;

        double minKilled = fleet.getAggregate().getMinKill()
                              * (1 - planet.getDefenses() / 100f);

        int dead = (int)Math.Max(killed, minKilled);
        planet.getCargo().addColonists(-dead);
        double totalBuildings = planet.getMines() + planet.getFactories() + planet.getDefenses();

        double buildingKills = fleet.getAggregate().getKillPop() * (1 - planet.getDefenses() / 100f);
        double damagePercent = buildingKills / totalBuildings;

        if (damagePercent > 1)
        {
            damagePercent = 1;
        }
        int defensesDestroyed = (int)((double)planet.getDefenses() * damagePercent);
        planet.setDefenses(planet.getDefenses() - defensesDestroyed);
        
        int factoriesDestroyed = (int)(planet.getFactories() * damagePercent);
        planet.setFactories(planet.getFactories() - factoriesDestroyed);
        
        int minesDestroyed = (int)(planet.getMines() * damagePercent);
        planet.setMines(planet.getMines() - minesDestroyed);

        if (planet.getCargo().getColonists() > 0)
        {
            Message.BombKillSome(fleet.getOwner(), fleet, planet, dead, defensesDestroyed, factoriesDestroyed, minesDestroyed);
        }
        else
        {
            Message.BombKillAll(fleet.getOwner(), fleet, planet);
            
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

        if (fleet.getOrbiting() == null)
        {
            Message.InvadeNotOrbiting(fleet.getOwner(), fleet);
            return;
        }

        int troops = fleet.getCargo().getColonists();
        Planet planet = fleet.getOrbiting(); ;

        if (planet.getOwner() == null)
        {
            colonise(fleet, wp);
        }

        if (troops == 0)
        {
            Message.InvadeNoTroops(fleet.getOwner(), fleet, planet);
            return;
        }
        
        if (fleet.getOwner().getID() == planet.getOwner().getID())
        {
            planet.getCargo().addColonists(troops);
            fleet.getCargo().setColonists(0);
            Message.InvadeAlreadyOwned(fleet.getOwner(), fleet, planet);
            return;
        }
        
        if (planet.getStarbase() != null)
        {
            Message.InvadeStarBase(fleet.getOwner(), fleet, planet);
            return;
        }
        
        fleet.getCargo().setColonists(0);
        
        int troopsOnGround = (int)(troops * (1 - (planet.getDefenses()/100)));
        
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
            int remainingDefenders = (int)(survivorStrength / defenderBonus);
            int defendersKilled = planet.getCargo().getColonists()- remainingDefenders;
            planet.getCargo().setColonists(remainingDefenders);

            Message.InvadeAttackersSlain(fleet.getOwner(), planet, defendersKilled);
        }
        else if (survivorStrength < 0)
        {
            planet.getQueue().getItems().Clear();
            int remainingAttackers = (int)(-survivorStrength / attackerBonus);
            int attackersKilled = troops - remainingAttackers;
            planet.getCargo().setColonists(remainingAttackers);
            planet.setOwner(fleet.getOwner());

            Message.InvadeDefendersSlain(fleet.getOwner(), planet, attackersKilled);
        }
        else
        {
            Message.InvadeDraw(fleet.getOwner(), planet);
            
            planet.getQueue().getItems().Clear();
            planet.setCargo(new Cargo(0, 0, 0, 0, 0));
            planet.setOwner(null);
        }
    }

    /**
     * Scrap this fleet, adding resources to the waypoint
     */
    public void destroyAndDistributeResources(Fleet fleet, Waypoint wp)
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
            GameObject.Destroy(fleet.FleetGameObject.gameObject);
        }
    }

    /**
     * Take this fleet and have it colonise a planet
     */
    public void colonise(Fleet fleet, Waypoint wp)
    {
        Planet planet = PlanetDictionary.instance.getPlanetForID(wp.getTarget().getName());
        if (planet == null)
        {
            Message.coloniseNonPlanet(fleet.getOwner(), fleet);
        }
        else if (planet.getOwner() != null)
        {
            Message.coloniseOwnedPlanet(fleet.getOwner(), fleet);
        }
        else if (!fleet.getAggregate().isColoniser())
        {
            Message.coloniseWithNoModule(fleet.getOwner(), fleet);
        }
        else if (fleet.getCargo().getColonists() <= 0)
        {
            Message.coloniseWithNoColonists(fleet.getOwner(), fleet);
        }
        else
        {
            planet.setOwner(fleet.getOwner());
            planet.setPopulation(fleet.getCargo().getColonists() * 100);
            fleet.getCargo().setColonists(0);
            destroyAndDistributeResources(fleet, wp);
        }
    }

    /**
     * Merge a ship_stack with this fleet, combining ship_stacks if the same design is found
     */
    public void addFleetToStack(Fleet fleet, ShipStack stack)
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
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Fleet : MapObject, CargoHolder {
    [SerializeField]
    private string playerID;

    private FleetGameObject fleetGameObject;

    [SerializeField]
    private List<ShipStack> shipStacks = new List<ShipStack>();


    [SerializeField]
    private List<Waypoint> waypoints = new List<Waypoint>();
    
    [SerializeField]
    private string orbitingID;

    [SerializeField]
    private int damage;
    [SerializeField]
    private bool starbase;
    [SerializeField]
    private bool scrapped;

    [SerializeField]
    private Cargo cargo = new Cargo();

    [SerializeField]
    private FleetAggregate aggregate = new FleetAggregate();

    public FleetGameObject FleetGameObject
    {
        get
        {
            return fleetGameObject;
        }

        set
        {
            fleetGameObject = value;
        }
    }

    public Fleet() : base()
    {
    }

    public Fleet(string name, int x, int y, Player owner) : base(name, x, y)
    {
        this.playerID = owner.getID();
    }

    public void CloneFrom(Fleet fleet)
    {
        this._name = fleet._name;
        this.setID(fleet.getID());
        this.x = fleet.x;
        this.y = fleet.y;
        this.playerID = fleet.playerID;
        this.shipStacks = fleet.shipStacks;
        this.waypoints = fleet.waypoints;
        this.orbitingID = fleet.orbitingID;
        this.setOrbiting(PlanetDictionary.instance.planetDict[orbitingID]);
        this.damage = fleet.damage;
        this.starbase = fleet.starbase;
        this.scrapped = fleet.scrapped;
        this.cargo = fleet.cargo;
        this.aggregate = fleet.aggregate;
    }

    override public string ToString()
    {
        return "Fleet [name=" + getName() + ", x=" + getX() + ", y=" + getY() + ", ships=" + shipStacks.ToString() + "]";
    }

    /**
     * Add a waypoint to this fleet
     */
    public Waypoint addWaypoint(int x, int y, int speed, WaypointTask task)
    {
        Waypoint wp = new Waypoint(x, y, speed, task);
        waypoints.Add(wp);
        return wp;
    }

    /**
     * Add a waypoint to this fleet
     */
    public Waypoint addWaypoint(int x, int y, int speed, WaypointTask task, MapObject target)
    {
        Waypoint wp = new Waypoint(x, y, speed, task, target);
        waypoints.Add(wp);
        return wp;
    }

    /**
     * Add a ShipStack to the fleet
     */
    public ShipStack addShipStack(ShipDesign design, int quantity)
    {
        ShipStack stack = new ShipStack(design, quantity);
        shipStacks.Add(stack);
        return stack;
    }

    override public void prePersist()
    {
        computeAggregate();
    }

    /**
     * Compute the aggregate of this fleet, combining all ships in the fleet
     */
    public void computeAggregate()
    {
        aggregate = new FleetAggregate();
        aggregate.setKillPop(0);
        aggregate.setMinKill(0);
        aggregate.setMass(0);
        aggregate.setShield(0);
        aggregate.setCargoCapacity(0);
        aggregate.setFuelCapacity(0);
        aggregate.setColoniser(false);
        aggregate.setCost(new Cost());
        aggregate.setSpaceDock(-1);

        foreach (ShipStack stack in shipStacks)
        {
            Cost cost = stack.getDesign().getAggregate().getCost().multiply(stack.getQuantity());
            aggregate.setCost(cost.add(aggregate.getCost()));
            
            aggregate.setKillPop(aggregate.getKillPop() + stack.getDesign().getAggregate().getKillPop() * stack.getQuantity());
            aggregate.setMinKill(aggregate.getMinKill() + stack.getDesign().getAggregate().getMinKill() * stack.getQuantity());
            
            aggregate.setMass(aggregate.getMass() + stack.getDesign().getAggregate().getMass() * stack.getQuantity());
            
            aggregate.setArmor(aggregate.getArmor() + stack.getDesign().getAggregate().getArmor() * stack.getQuantity());
            
            aggregate.setShield(aggregate.getShield() + stack.getDesign().getAggregate().getShield() * stack.getQuantity());
            
            aggregate.setCargoCapacity(aggregate.getCargoCapacity() + stack.getDesign().getAggregate().getCargoCapacity() * stack.getQuantity());
            
            aggregate.setFuelCapacity(aggregate.getFuelCapacity() + stack.getDesign().getAggregate().getFuelCapacity() * stack.getQuantity());
            
            if (stack.getDesign().getAggregate().isColoniser())
            {
                aggregate.setColoniser(true);
            }
            
            if (stack.getDesign().getAggregate().getSpaceDock() != 0)
            {
                aggregate.setSpaceDock(stack.getDesign().getAggregate().getSpaceDock());
            }

            if (stack.getDesign().getAggregate().getScanRange() != 0)
            {
                if (aggregate.getScanRange() != 0)
                {
                    aggregate.setScanRange(Mathf.Max(aggregate.getScanRange(), stack.getDesign().getAggregate().getScanRange()));
                }
                else
                {
                    aggregate.setScanRange(stack.getDesign().getAggregate().getScanRange());
                }
            }

            if (stack.getDesign().getAggregate().getScanRangePen() != 0)
            {
                if (aggregate.getScanRangePen() != 0)
                {
                    aggregate.setScanRangePen(Mathf.Max(aggregate.getScanRangePen(), stack.getDesign().getAggregate().getScanRangePen()));
                }
                else
                {
                    aggregate.setScanRangePen(stack.getDesign().getAggregate().getScanRangePen());
                }
            }
        }
    }

    /**
     * @return true if this fleet can scan
     */
    public bool canScan()
    {
        if (aggregate.getScanRange() != 0)
        {
            return true;
        }
        return false;
    }

    public int getCargoCapacity()
    {
        return aggregate.getCargoCapacity();
    }

    /**
     * Have a user 'discover' this fleet
     */
    public void discover(Player player, bool pen)
    {
        FleetKnowledge knowledge;
        if (!player.hasKnowledge(this))
        {
            player.getFleetKnowledges().Add(new FleetKnowledge(this));
        }
        knowledge = player.getFleetKnowledge(this);

        knowledge.discover(pen);

        /*
        if (!knowledges.containsKey(player.getId())) {
            knowledges.put(player.getId(), new FleetKnowledge(this, player));
        }
        knowledge = knowledges.get(player.getId());
        // discover this fleet
        knowledge.discover(pen);
        */
    }

    public Player getOwner()
    {
        foreach (Player player in fleetGameObject.game.getGame().getPlayers())
            if (player.getID() == playerID)
                return player;
        return null;
    }

    public void setOwner(Player owner)
    {
        this.playerID = owner.getID();
    }

    public List<ShipStack> getShipStacks()
    {
        return shipStacks;
    }

    public void setShipStacks(List<ShipStack> shipStacks)
    {
        this.shipStacks = shipStacks;
    }

    public List<Waypoint> getWaypoints()
    {
        return waypoints;
    }

    public void setWaypoints(List<Waypoint> waypoints)
    {
        this.waypoints = waypoints;
    }

    public Cargo getCargo()
    {
        return cargo;
    }

    public void setCargo(Cargo cargo)
    {
        this.cargo = cargo;
    }

    public int getDamage()
    {
        return damage;
    }

    public void setDamage(int damage)
    {
        this.damage = damage;
    }

    public bool isStarbase()
    {
        return starbase;
    }

    public void setStarbase(bool starbase)
    {
        this.starbase = starbase;
    }

    public int getFuel()
    {
        return cargo.getFuel();
    }

    public void setFuel(int fuel)
    {
        cargo.setFuel(fuel);
    }

    public bool isScrapped()
    {
        return scrapped;
    }

    public void setScrapped(bool scrapped)
    {
        this.scrapped = scrapped;
    }

    public FleetAggregate getAggregate()
    {
        return aggregate;
    }

    public void setAggregate(FleetAggregate aggregate)
    {
        this.aggregate = aggregate;
    }

    public void setOrbiting(Planet orbiting)
    {
        // remove ourselves from the orbiting list
        if (this.getOrbiting() != null)
        {
            this.getOrbiting().getOrbitingFleets().Remove(this);
            this.FleetGameObject.transform.SetParent(this.FleetGameObject.transform.parent.parent);
        }
        if (orbiting != null)
            this.orbitingID = orbiting.getID();
        else
            this.orbitingID = "";

        // add ourselves to the new orbiting planet
        if (this.getOrbiting() != null)
        {
            this.getOrbiting().getOrbitingFleets().Add(this);
            this.FleetGameObject.transform.SetParent(orbiting.PlanetGameObject.transform);
        }
    }

    public Planet getOrbiting()
    {
        if (orbitingID == null || orbitingID == "")
            return null;
        return PlanetDictionary.instance.planetDict[orbitingID];
    }


}

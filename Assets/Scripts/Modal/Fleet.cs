using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fleet : MapObject, CargoHolder {
    [SerializeField]
    private Player owner;


    [SerializeField]
    private List<ShipStack> shipStacks = new List<ShipStack>();


    [SerializeField]
    private List<Waypoint> waypoints = new List<Waypoint>();


    [SerializeField]
    private Planet orbiting;

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

    public Fleet() : base()
    {
    }

    public Fleet(string name, int x, int y, Player owner) : base(name, x, y)
    {
        this.owner = owner;
    }

    public void CloneFrom(Fleet fleet)
    {
        this._name = fleet._name;
        this.setID(fleet.getID());
        this.x = fleet.x;
        this.y = fleet.y;
        this.owner = fleet.owner;
        this.shipStacks = fleet.shipStacks;
        this.waypoints = fleet.waypoints;
        this.orbiting = fleet.orbiting;
        this.damage = fleet.damage;
        this.starbase = fleet.starbase;
        this.scrapped = fleet.scrapped;
        this.cargo = fleet.cargo;
        this.aggregate = fleet.aggregate;
    }

    private void Update()
    {
            GetComponent<Image>().enabled = getOrbiting() == null;
            GetComponent<Button>().enabled = getOrbiting() == null;

            transform.localPosition = new Vector3(GetComponent<Fleet>().getX() - Game.instance.getWidth() / 2, GetComponent<Fleet>().getY() - Game.instance.getHeight() / 2);
    }


    override public string ToString()
    {
        return "Fleet [name=" + getName() + ", x=" + getX() + ", y=" + getY() + ", ships=" + shipStacks.ToString() + "]";
    }

    /**
     * Add a waypoint to this fleet
     * @param x The x point
     * @param y The y point
     * @param speed The speed
     * @param task The task to perform
     * @return The newly created Waypoint
     */
    public Waypoint addWaypoint(int x, int y, int speed, WaypointTask task)
    {
        Waypoint wp = new Waypoint(x, y, speed, task);
        waypoints.Add(wp);
        return wp;
    }

    /**
     * Add a waypoint to this fleet
     * @param x The x point
     * @param y The y point
     * @param speed The speed
     * @param task The task to perform
     * @param target The target for the waypoints
     * @return The newly created Waypoint
     */
    public Waypoint addWaypoint(int x, int y, int speed, WaypointTask task, MapObject target)
    {
        Waypoint wp = new Waypoint(x, y, speed, task, target);
        waypoints.Add(wp);
        return wp;
    }

    /**
     * Add a ShipStack to the fleet
     * @param design The design to put in the stack
     * @param quantity The number of ships in the stack
     * @return The newly added ShipStack
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
        aggregate.setMass(0);
        aggregate.setShield(0);
        aggregate.setCargoCapacity(0);
        aggregate.setFuelCapacity(0);
        aggregate.setColonizer(false);
        aggregate.setCost(new Cost());
        aggregate.setSpaceDock(-1);

        foreach (ShipStack stack in shipStacks)
        {
            // cost
            Cost cost = stack.getDesign().getAggregate().getCost().multiply(stack.getQuantity());
            aggregate.setCost(cost.add(aggregate.getCost()));

            // mass
            aggregate.setMass(aggregate.getMass() + stack.getDesign().getAggregate().getMass() * stack.getQuantity());

            // armor
            aggregate.setArmor(aggregate.getArmor() + stack.getDesign().getAggregate().getArmor() * stack.getQuantity());

            // shield
            aggregate.setShield(aggregate.getShield() + stack.getDesign().getAggregate().getShield() * stack.getQuantity());

            // cargo
            aggregate.setCargoCapacity(aggregate.getCargoCapacity() + stack.getDesign().getAggregate().getCargoCapacity() * stack.getQuantity());

            // fuel
            aggregate.setFuelCapacity(aggregate.getFuelCapacity() + stack.getDesign().getAggregate().getFuelCapacity() * stack.getQuantity());

            // colonization
            if (stack.getDesign().getAggregate().isColonizer())
            {
                aggregate.setColonizer(true);
            }

            // We should only have one ship stack with spacedock capabilities, so no need to add
            if (stack.getDesign().getAggregate().getSpaceDock() != null)
            {
                aggregate.setSpaceDock(stack.getDesign().getAggregate().getSpaceDock());
            }

            if (stack.getDesign().getAggregate().getScanRange() != null)
            {
                if (aggregate.getScanRange() != null)
                {
                    aggregate.setScanRange(Mathf.Max(aggregate.getScanRange(), stack.getDesign().getAggregate().getScanRange()));
                }
                else
                {
                    aggregate.setScanRange(stack.getDesign().getAggregate().getScanRange());
                }
            }

            if (stack.getDesign().getAggregate().getScanRangePen() != null)
            {
                if (aggregate.getScanRangePen() != null)
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
        if (aggregate.getScanRange() != null)
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
        if (!player.getFleetKnowledges().ContainsKey(id))
        {
            player.getFleetKnowledges().Add(id, new FleetKnowledge(this));
        }
        player.getFleetKnowledges().TryGetValue(id, out knowledge);

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
        return owner;
    }

    public void setOwner(Player owner)
    {
        this.owner = owner;
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
        if (this.orbiting != null)
        {
            this.orbiting.getOrbitingFleets().Remove(this);
            this.transform.SetParent(transform.parent.parent);
        }
        this.orbiting = orbiting;

        // add ourselves to the new orbiting planet
        if (this.orbiting != null)
        {
            this.orbiting.getOrbitingFleets().Add(this);
            this.transform.SetParent(orbiting.transform);
        }
    }

    public Planet getOrbiting()
    {
        return orbiting;
    }


}

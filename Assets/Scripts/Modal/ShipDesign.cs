using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class ShipDesign : AbstractStarsObject_NonMono
{
    [SerializeField]
    private string name;

    /**
     * This hull is filled in when a player initializes there ShipDesigns after load.
     */
    [SerializeField]
    private TechHull hull;

    [SerializeField]
    private string hullName;

    [SerializeField]
    private int hullSetNumber = 1;

    /**
     * The order of these slots must match up with the slots for the Hull this ShipDesign is for.
     */
    [SerializeField]
    private List<ShipDesignSlot> slots = new List<ShipDesignSlot>();

    [SerializeField]
    private ShipDesignAggregate aggregate = new ShipDesignAggregate();

    public ShipDesign() : base()
    {
    }

    public ShipDesign(string name, TechHull hull) : base()
    {
        this.name = name;
        this.hull = hull;
        this.hullName = hull.getName();

        setupSlots();
    }

    override public string ToString()
    {
        string temp = "ShipDesign [name=" + name + ", hull=" + hull.getName() + "], slots:" + slots.Count;
        foreach (ShipDesignSlot s in slots)
            temp += ", slot [component" + s.getHullComponent() + "]";
        return temp;
    }

    /**
     * Setup the ShipDesignSlots to correspond to the TechHullSlots
     */
    private void setupSlots()
    {
        this.slots.Clear();
        foreach (TechHullSlot slot in hull.getSlots())
        {
            this.slots.Add(new ShipDesignSlot(slot, null, 0));
        }
    }

    /**
     * Assign a slot with a hull component and a quantity
     * 
     * @param slot The slot from this ShipDesign
     * @param hullComponent The HullComponent to assign to the slot
     * @param quantity The quantity to assign (will be reset to max)
     */
    public void assignSlot(ShipDesignSlot slot, TechHullComponent hullComponent, int quantity)
    {
        if (!slot.getHullSlot()
            .getTypes()
            .Contains(
            hullComponent
            .getHullSlotType()))
        {
            Debug.Log(string.Format("This TechHullComponent {0} does not go into the slot: {1}", hullComponent.getName(),
                                                                         slot.getHullSlot().getTypes()));
        }

        // if this slot already has this hull component
        // increment the quantity
        if (slot.getHullComponent() == hullComponent)
        {
            slot.setQuantity(slot.getQuantity() + quantity);
        }
        else
        {
            slot.setHullComponent(hullComponent);
            slot.setQuantity(quantity);
        }
        // if we have too many, reduce it to the proper size
        if (slot.getQuantity() > slot.getHullSlot().getCapacity())
        {
            slot.setQuantity(slot.getHullSlot().getCapacity());
        }
    }

    /**
     * Compute the aggregate of this ship design, all hull components and such
     */
    public void computeAggregate(Player owner)
    {
        aggregate = new ShipDesignAggregate();
        aggregate.setMass(hull.getMass());
        aggregate.setArmor(hull.getArmor());
        aggregate.setShield(0);
        aggregate.setCargoCapacity(0);
        aggregate.setFuelCapacity(hull.getFuelCapacity() == 0 ? 0 : hull.getFuelCapacity());
        aggregate.setColonizer(false);
        aggregate.setCost(hull.getCost());
        aggregate.setSpaceDock(-1);

        foreach (ShipDesignSlot slot in slots)
        {
            if (slot.getHullComponent() != null)
            {
                if (slot.getHullComponent() is TechEngine)
                {
                    aggregate.setEngine((TechEngine)slot.getHullComponent());
                    aggregate.setEngineName(slot.getHullComponent().getName());
                }
                // cost
                Cost cost = slot.getHullComponent().getCost().multiply(slot.getQuantity());
                cost = cost.add(aggregate.getCost());
                aggregate.setCost(cost);

                // mass
                aggregate.setMass(aggregate.getMass() + slot.getHullComponent().getMass() * slot.getQuantity());
                if (slot.getHullComponent().getArmor() != 0)
                {
                    aggregate.setArmor(aggregate.getArmor() + slot.getHullComponent().getArmor() * slot.getQuantity());
                }
                // shield
                if (slot.getHullComponent().getShield() != 0)
                {
                    aggregate.setShield(aggregate.getShield() + slot.getHullComponent().getShield() * slot.getQuantity());
                }
                // cargo
                if (slot.getHullComponent().getCargoBonus() != 0)
                {
                    aggregate.setCargoCapacity(aggregate.getCargoCapacity() + slot.getHullComponent().getCargoBonus() * slot.getQuantity());
                }
                // fuel
                if (slot.getHullComponent().getFuelBonus() != 0)
                {
                    aggregate.setFuelCapacity(aggregate.getFuelCapacity() + slot.getHullComponent().getFuelBonus() * slot.getQuantity());
                }
                // colonization
                if (slot.getHullComponent().isColonizationModule())
                {
                    aggregate.setColonizer(true);
                }
            }
            // cargo and space doc that are built into the hull
            // the space dock assumes that there is only one slot like that
            // it won't add them up
            if (slot.getHullSlot().getTypes().Contains(HullSlotType.SpaceDock))
            {
                aggregate.setSpaceDock(slot.getHullSlot().getCapacity());
            }
            if (slot.getHullSlot().getTypes().Contains(HullSlotType.Cargo))
            {
                aggregate.setCargoCapacity(aggregate.getCargoCapacity() + slot.getHullSlot().getCapacity());
            }
            if (slot.getHullSlot().getTypes().Contains(HullSlotType.Bomb))
            {
                aggregate.setKillPop(aggregate.getKillPop() + slot.getHullSlot().getCapacity());
            }
        }
        // compute the scan ranges
        computeScanRanges(owner);

    }

    /**
     * Compute the scan ranges for this ship design The formula is: (scanner1**4 + scanner2**4 + ...
     * + scannerN**4)**(.25)
     */
    private void computeScanRanges(Player owner)
    {
        long scanRange = 0L;
        long scanRangePen = 0L;

        // compute the scanner as a built in JoaT scanner if it's build in
        if (owner.getRace().getPRT() == PRT.JoaT && hull.isBuiltInScannerForJoaT())
        {
            scanRange = (long)(owner.getTechLevels().getElectronics() * Consts.builtInScannerJoaTMultiplier);
            if (!owner.getRace().hasLRT(LRT.NAS))
            {
                scanRangePen = (long)Mathf.Pow(scanRange / 2, 4);
            }
            scanRange = (long)Mathf.Pow(scanRange, 4);
        }

        // aggregate the scan range from each slot
        foreach (ShipDesignSlot slot in slots)
        {
            if (slot.getHullComponent() != null)
            {
                // scan range of None means no scanners, 0 means bat scanner
                if (slot.getHullComponent().getScanRange() != 0)
                {
                    if (scanRange == 0)
                    {
                        scanRange = 0L;
                    }
                    scanRange += (long)(Mathf.Pow(slot.getHullComponent().getScanRange(), 4) * slot.getQuantity());
                }

                // scan range of None means no scanners, 0 means bat scanner
                if (slot.getHullComponent().getScanRangePen() != 0)
                {
                    if (scanRangePen == 0)
                    {
                        scanRangePen = 0L;
                    }
                    scanRangePen += (long)((Mathf.Pow(slot.getHullComponent().getScanRangePen(), 4)) * slot.getQuantity());
                }
            }
        }

        // now quad root it
        if (scanRange != 0)
        {
            scanRange = (long)(Mathf.Pow(scanRange, 0.25f));
        }

        if (scanRangePen != 0)
        {
            scanRangePen = (long)(Mathf.Pow(scanRangePen, 0.25f));
        }

        if (scanRange != 0)
        {
            aggregate.setScanRange((int)scanRange);
        }
        else
        {
            aggregate.setScanRange(0);
        }

        if (scanRangePen != 0)
        {
            aggregate.setScanRangePen((int)scanRangePen);
        }
        else
        {
            // if we have no pen scan but we have a regular scan, set the pen scan range to 0
            if (scanRange != 0)
            {
                aggregate.setScanRangePen(0);
            }
            else
            {
                aggregate.setScanRangePen(0);
            }
        }

    }

    public string getName()
    {
        return name;
    }

    public void setName(string name)
    {
        this.name = name;
    }

    public TechHull getHull()
    {
        return hull;
    }

    public void setHull(TechHull hull)
    {
        this.hull = hull;
        if (hull == null)
        {
            this.hullName = null;
        }
        else
        {
            this.hullName = hull.getName();
        }
    }

    public string getHullName()
    {
        return hullName;
    }

    public void setHullName(string hullName)
    {
        this.hullName = hullName;
    }

    public List<ShipDesignSlot> getSlots()
    {
        return slots;
    }

    public void setSlots(List<ShipDesignSlot> slots)
    {
        this.slots = slots;
    }

    public void setAggregate(ShipDesignAggregate aggregate)
    {
        this.aggregate = aggregate;
    }

    public ShipDesignAggregate getAggregate()
    {
        return aggregate;
    }

    public void setHullSetNumber(int hullSetNumber)
    {
        this.hullSetNumber = hullSetNumber;
    }

    public int getHullSetNumber()
    {
        return hullSetNumber;
    }

    public override void prePersist()
    {
    }
}

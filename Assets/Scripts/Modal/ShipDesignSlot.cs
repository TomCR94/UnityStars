using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ShipDesignSlot : AbstractStarsObject_NonMono
{

    /**
   * This is filled in when a player's ship designs are initialized by a TechStore
   */
   [SerializeField]
    private TechHullSlot hullSlot;

    /**
     * This is filled in when a player's ship designs are initialized by a TechStore
     */
    [SerializeField]
    private TechHullComponent hullComponent;

    [SerializeField]
    private string hullComponentName;

    [SerializeField]
    private int quantity;

    public ShipDesignSlot() : base()
    {
    }

    public ShipDesignSlot(TechHullSlot hullSlot, TechHullComponent hullComponent, int quantity) : base()
    {
        this.hullSlot = hullSlot;
        this.hullComponent = hullComponent;
        if (this.hullComponent != null)
        {
            this.hullComponentName = hullComponent.getName();
        }
        this.quantity = quantity;
    }

    public TechHullSlot getHullSlot()
    {
        return hullSlot;
    }

    public void setHullSlot(TechHullSlot hullSlot)
    {
        this.hullSlot = hullSlot;
    }

    public TechHullComponent getHullComponent()
    {
        return hullComponent;
    }

    public void setHullComponent(TechHullComponent hullComponent)
    {
        this.hullComponent = hullComponent;
        if (this.hullComponent == null)
        {
            hullComponentName = null;
        }
        else
        {
            hullComponentName = this.hullComponent.getName();
        }
    }

    public String getHullComponentName()
    {
        return hullComponentName;
    }

    public void setHullComponentName(String hullComponentName)
    {
        this.hullComponentName = hullComponentName;
    }

    public int getQuantity()
    {
        return quantity;
    }

    public void setQuantity(int quantity)
    {
        this.quantity = quantity;
    }

    public override void prePersist()
    {
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ShipStack : AbstractStarsObject_NonMono {

    [SerializeField]
    private ShipDesign design;

    [SerializeField]
    private int quantity;

    public ShipStack() : base()
    {
    }

    public ShipStack(ShipDesign design, int quantity) : base()
    {
        this.design = design;
        this.quantity = quantity;
    }

    override public string ToString()
    {
        return "ShipStack [design=" + design.getName() + ", quantity=" + quantity + "]";
    }

    public ShipDesign getDesign()
    {
        return design;
    }

    public void setDesign(ShipDesign design)
    {
        this.design = design;
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

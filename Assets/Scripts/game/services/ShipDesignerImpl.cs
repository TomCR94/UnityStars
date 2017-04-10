using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipDesignerImpl : ShipDesigner
{
    
public ShipDesign designShip(TechHull hull, Player player)
{
    ShipDesign design = new ShipDesign(hull.getName(), hull);
        
    foreach (ShipDesignSlot slot in design.getSlots())
    {
        HullSlotType[] types = slot.getHullSlot().getTypes();

        if (types.Contains(HullSlotType.Engine))
        {
            design.assignSlot(slot, player.getTechs().getBestEngine(), slot.getQuantity());
        }
        else if (types.Contains(HullSlotType.Scanner))
        {
            design.assignSlot(slot, player.getTechs().getBestScanner(), slot.getQuantity());
        }
        else if (types.Contains(HullSlotType.Shield))
        {
            design.assignSlot(slot, player.getTechs().getBestShield(), slot.getQuantity());
        }
        else if (types.Contains(HullSlotType.Armor))
        {
            design.assignSlot(slot, player.getTechs().getBestArmor(), slot.getQuantity());
        }
        else if (types.Contains(HullSlotType.Weapon))
        {
            design.assignSlot(slot, player.getTechs().getBestTorpedo(), slot.getQuantity());
        }
        else if (types.Contains(HullSlotType.Mechanical))
        {
            // if this is a colony ship, put the colonization module in the mechanical slot
            if (hull.getName().Equals("Colony Ship"))
            {
                design.assignSlot(slot, StaticTechStore.getInstance().getHullComponent("Colonization Module"), slot.getQuantity());
            }
            else
            {
                design.assignSlot(slot, StaticTechStore.getInstance().getHullComponent("Fuel Tank"), slot.getQuantity());
            }
        }

        }
        design.computeAggregate(player);
        initShipDesign(design);

    return design;
}

    public void initShipDesign(ShipDesign design)
    {
        TechHull hull = StaticTechStore.getInstance().getHull(design.getHullName());
        design.setHull(hull);
        for (int slotIndex = 0; slotIndex < hull.getSlots().Count; slotIndex++)
        {          
            ShipDesignSlot designSlot = design.getSlots()[slotIndex];
            if (designSlot.getHullComponentName() == null)
                break;
            designSlot.setHullSlot(hull.getSlots()[slotIndex]);
            designSlot.setHullComponent(StaticTechStore.getInstance().getHullComponent(designSlot.getHullComponentName()));
        }
        
        design.getAggregate().setEngine(
            StaticTechStore.getInstance().
            getEngine(
                design.
                getAggregate().
                getEngineName()));
        
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Controller for automatically designing Ships for a player using the best technology available
 */
public interface ShipDesigner
{

    /**
     * Design a new ShipDesign for this hull and player using their best tech for each component. 
     * @param hull The hull
     * @param player The player
     * @return The newly created ShipDesign.
     */
    ShipDesign designShip(TechHull hull, Player player);

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTurnProcessor : MonoBehaviour, TurnProcessor
{

    /**
     * The player to process turns for
     */
    protected Player player;

public void init(Player player)
{
    this.player = player;
}

/**
 * Check if this planet can build things
 * @param planet The planet
 * @param dockSizeRequired The dock size required for this planet
 * @return true if this planet can build
 */
protected bool isBuildablePlanet(Planet planet, int dockSizeRequired)
{
    if (planet.getOwner() != null && planet.getOwner().getID() == player.getID() && planet.getStarbase() != null)
    {
        int dockSize = planet.getStarbase().getAggregate().getSpaceDock();
        if ((dockSize == 0 || dockSize >= dockSizeRequired))
        {
            return true;
        }
    }
    return false;
}

/**
 * Find the planet that is nearest to this fleet and return it
 * 
 * returns null if no planet found
 * 
 * @param fleet The fleet to check for the nearest planet
 * @param unknownPlanets A list of planets to find the closest planet for
 * @return null if no planets found, otherwise the nearest planet
 */
public Planet closestPlanet(Fleet fleet, List<Planet> unknownPlanets)
{
    Planet closest = null;
    // find the nearest planet to this fleet
    int dist = 0;
    foreach (Planet planet in unknownPlanets)
    {
        if (closest == null)
        {
            closest = planet;
            dist = planet.dist(fleet);
        }
        else
        {
            // figure out the nearest planet to this fleet
            int new_dist = planet.dist(fleet);
            if (new_dist < dist)
            {
                // this planet is closer, save it
                // and recompute the distance
                dist = new_dist;
                closest = planet;
            }
        }
    }
    return closest;
}

    public void process()
    {
    }
}
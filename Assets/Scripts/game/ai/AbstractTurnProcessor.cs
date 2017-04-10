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

    protected Game game;

public void init(Player player, Game game)
{
    this.player = player;
        this.game = game;
}

/**
 * Check if this planet can build things
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
 */
public Planet closestPlanet(Fleet fleet, List<Planet> unknownPlanets)
{
    Planet closest = null;

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
            int new_dist = planet.dist(fleet);
            if (new_dist < dist)
            {
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
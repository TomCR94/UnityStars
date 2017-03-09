using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wormhole : MapObject {

    GameGameObject game;
    Wormhole twin;
    
    void generateTwin()
    {
    }

    private bool isValidLocation(Vector2 loc, int offset)
    {
        int x = (int)loc.x;
        int y = (int)loc.y;

        List<Planet> planets = game.getGame().getPlanets();
        Dictionary<Vector2, bool> planetLocs = new Dictionary<Vector2, bool>();


        foreach(Planet p in planets)
            planetLocs.Add(new Vector2(p.getX(), p.getY()), true);

        if (planetLocs.ContainsKey(loc))
        {
            return false;
        }

        for (int yOffset = 0; yOffset < offset; yOffset++)
        {
            for (int xOffset = 0; xOffset < offset; xOffset++)
            {
                if (planetLocs.ContainsKey(new Vector2(x + xOffset, y + yOffset)))
                {
                    return false;
                }
                if (planetLocs.ContainsKey(new Vector2(x - xOffset, y + yOffset)))
                {
                    return false;
                }
                if (planetLocs.ContainsKey(new Vector2(x - xOffset, y - yOffset)))
                {
                    return false;
                }
                if (planetLocs.ContainsKey(new Vector2(x + xOffset, y - yOffset)))
                {
                    return false;
                }
            }
        }

        return true;
    }
}

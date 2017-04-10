using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Wormhole : MapObject {

    [NonSerialized]
    private WormholeGameObject wormholeGameObject;
    public WormholeGameObject WormholeGameObject
    {
        get
        {
            return wormholeGameObject;
        }

        set
        {
            wormholeGameObject = value;
        }
    }
    [SerializeField]
    string twinID;
    [SerializeField]
    bool stabiized;

    public Wormhole(string name, int x, int y) : base(name, x, y)
    {
    }

    public Wormhole getTwin()
    {
        if (twinID == null)
            generateTwin();

        return WormholeDictionary.instance.getWormholeForID(twinID);
    }

    public void setStabilized(bool stab)
    {
        stabiized = stab;
    }

    public bool getStabilized()
    {
        return stabiized;
    }

    void generateTwin()
    {
        int width, height;
        height = Consts.sizeToArea[GameGameObject.instance.game.getSize()];
        width = height;

        
        System.Random random = new System.Random();
        Dictionary<Vector2, bool> planetLocs = new Dictionary<Vector2, bool>();

        foreach (Planet planet in GameGameObject.instance.game.getPlanets())
            planetLocs.Add(new Vector2(planet.getX(), planet.getY()), true);

        
        Vector2 loc = new Vector2(random.Next(width), random.Next(height));
        
        while (!isValidLocation(loc, planetLocs, Consts.planetMinDistance))
        {
            loc = new Vector2(random.Next(width), random.Next(height));
        }


        GameObject go = GameObject.Instantiate(WormholeGameObject.gameObject, WormholeGameObject.gameObject.transform.parent);
        Wormhole wormhole = new Wormhole(getName().Replace('a', 'b'), (int)loc.x, (int)loc.y);
        go.GetComponent<WormholeGameObject>().setWormhole(wormhole);
        go.GetComponent<WormholeGameObject>().getWormhole().twinID = getID();
        twinID = go.GetComponent<WormholeGameObject>().getWormhole().getID();
        GameGameObject.instance.game.addWormholes(go.GetComponent<WormholeGameObject>().getWormhole());
        go.transform.localPosition = new Vector3(go.GetComponent<WormholeGameObject>().getWormhole().getX() - width / 2, go.GetComponent<WormholeGameObject>().getWormhole().getY() - height / 2);
        go.name = wormhole.getName();
        go.SetActive(true);
        go.transform.SetAsFirstSibling();


        Debug.Log("Wormhole: " + wormhole.getName());

    }

    /**
     * Return true if the location is not already in (or close to another planet) planet_locs
     */
    private static bool isValidLocation(Vector2 loc, Dictionary<Vector2, bool> planetLocs, int offset)
    {
        int x = (int)loc.x;
        int y = (int)loc.y;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : AbstractStarsObject {

    [SerializeField]
    protected string _name;
    [SerializeField]
    protected int x;
    [SerializeField]
    protected int y;

    public MapObject()
    {
    }

    public MapObject(string name, int x, int y)
    {
        this._name = name;
        this.x = x;
        this.y = y;
    }

    /**
     * Compute the distance between two map objects, without the square root applied
     * 
     * @return The distance
     */
    public int dist(MapObject other)
    {
        return ((x - other.x) * (x - other.x) + (y - other.y) * (y - other.y));
    }

    /**
     * Get the distance between two points
     * 
     * @param x1
     * @param y1
     * @param x2
     * @param y2
     * @return The distance as a double
     */
    public static double realDist(int x1, int y1, int x2, int y2)
    {
        return Mathf.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
    }


    public string getName()
    {
        return _name;
    }

    public void setName(string name)
    {
        this._name = name;
    }

    public int getX()
    {
        return x;
    }

    public void setX(int x)
    {
        this.x = x;
    }

    public int getY()
    {
        return y;
    }

    public void setY(int y)
    {
        this.y = y;
    }


}

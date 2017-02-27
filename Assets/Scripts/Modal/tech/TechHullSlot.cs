using System;
using UnityEngine;

[System.Serializable]
public class TechHullSlot {

    /**
     * The types of {@link TechHullComponent}s that can go in this slot
     */
    [SerializeField]
    private HullSlotType[] types = (HullSlotType[])Enum.GetValues(typeof(HullSlotType));

    /**
     * The number of {@link TechHullComponent}s that can go in the slot
     */
    [SerializeField]
    private int capacity;

    /**
     * Is this slot required to be filled? (eg, an Engine)
     */
    [SerializeField]
    private bool required;

    /**
     * The x coord to draw this slot in the UI
     */
    [SerializeField]
    private int x;

    /**
     * The y coord to draw this slot in the UI
     */
    [SerializeField]
    private int y;

    /**
     * The width of this slot
     */
    [SerializeField]
    private int width;

    /**
     * The height of this slot
     */
    [SerializeField]
    private int height;

    public TechHullSlot(HullSlotType[] types, int capacity, bool required, int x, int y, int width, int height)
    {
        this.types = types;
        this.capacity = capacity;
        this.required = required;
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    public HullSlotType[] getTypes()
    {
        return types;
    }

    public void setTypes(HullSlotType[] types)
    {
        this.types = types;
    }

    public int getCapacity()
    {
        return capacity;
    }

    public void setCapacity(int capacity)
    {
        this.capacity = capacity;
    }

    public bool isRequired()
    {
        return required;
    }

    public void setRequired(bool required)
    {
        this.required = required;
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

    public int getWidth()
    {
        return width;
    }

    public void setWidth(int width)
    {
        this.width = width;
    }

    public int getHeight()
    {
        return height;
    }

    public void setHeight(int height)
    {
        this.height = height;
    }
}
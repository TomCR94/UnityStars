using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDesignAggregate {

    /**
 * The engine used for doing
 */
    TechEngine engine;

    string engineName;
    
    private Cost cost = new Cost();

    private int mass;
    private int armor;
    private int shield;
    private int cargoCapacity;
    private int fuelCapacity;

    private int scanRange;
    private int scanRangePen;

    private bool colonizer;
    private int spaceDock;

    public Cost getCost()
    {
        return cost;
    }

    public void setCost(Cost cost)
    {
        this.cost = cost;
    }

    public int getMass()
    {
        return mass;
    }

    public void setMass(int mass)
    {
        this.mass = mass;
    }

    public int getArmor()
    {
        return armor;
    }

    public void setArmor(int armor)
    {
        this.armor = armor;
    }

    public int getShield()
    {
        return shield;
    }

    public void setShield(int shield)
    {
        this.shield = shield;
    }

    public int getCargoCapacity()
    {
        return cargoCapacity;
    }

    public void setCargoCapacity(int cargoCapacity)
    {
        this.cargoCapacity = cargoCapacity;
    }

    public int getFuelCapacity()
    {
        return fuelCapacity;
    }

    public void setFuelCapacity(int fuelCapacity)
    {
        this.fuelCapacity = fuelCapacity;
    }

    public int getScanRange()
    {
        return scanRange;
    }

    public void setScanRange(int scanRange)
    {
        this.scanRange = scanRange;
    }

    public int getScanRangePen()
    {
        return scanRangePen;
    }

    public void setScanRangePen(int scanRangePen)
    {
        this.scanRangePen = scanRangePen;
    }

    public bool isColonizer()
    {
        return colonizer;
    }

    public void setColonizer(bool colonizer)
    {
        this.colonizer = colonizer;
    }

    public int getSpaceDock()
    {
        return spaceDock;
    }

    public void setSpaceDock(int spaceDock)
    {
        this.spaceDock = spaceDock;
    }

    public TechEngine getEngine()
    {
        return engine;
    }

    public void setEngine(TechEngine engine)
    {
        this.engine = engine;
    }

    public string getEngineName()
    {
        return engineName;
    }

    public void setEngineName(string engineName)
    {
        this.engineName = engineName;
    }

}

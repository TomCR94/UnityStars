﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShipDesignAggregate {
    
    [SerializeField]
    TechEngine engine;
    [SerializeField]
    string engineName;

    [SerializeField]
    private Cost cost = new Cost();

    [SerializeField]
    private int mass;
    [SerializeField]
    private int armor;
    [SerializeField]
    private int shield;
    [SerializeField]
    private int cargoCapacity;
    [SerializeField]
    private int fuelCapacity;

    [SerializeField]
    private int scanRange;
    [SerializeField]
    private int scanRangePen;

    [SerializeField]
    private bool coloniser;
    [SerializeField]
    private int spaceDock;

    [SerializeField]
    private int killPop;
    [SerializeField]
    private int minKill;

    public int getKillPop()
    {
        return killPop;
    }

    public void setKillPop(int i)
    {
        killPop = i;
    }

    public int getMinKill()
    {
        return minKill;
    }

    public void setMinKill(int i)
    {
        minKill = i;
    }

    public bool isBomber()
    {
        return (minKill + killPop) > 0;
    }

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

    public bool isColoniser()
    {
        return coloniser;
    }

    public void setColoniser(bool coloniser)
    {
        this.coloniser = coloniser;
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

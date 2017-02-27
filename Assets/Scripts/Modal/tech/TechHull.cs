using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TechHull : Tech {

    [SerializeField]
    private int _mass;
    [SerializeField]
    private int _armor;
    [SerializeField]
    private int _fuelCapacity;
    [SerializeField]
    private int _fuelGenerationPerYear;
    [SerializeField]
    private int _fleetHealBonus;
    [SerializeField]
    private bool _doubleMineEfficiency;
    [SerializeField]
    private bool _builtInScannerForJoaT;
    [SerializeField]
    private bool _starbase;
    [SerializeField]
    private int _initiative;
    [SerializeField]
    private List<TechHullSlot> _slots = new List<TechHullSlot>();

    public TechHull() : base()
    {
    }

    public TechHull(string name, Cost cost, TechRequirements techRequirements, int ranking, TechCategory category) : base(name, cost, techRequirements, ranking, category)
    {
    }

    public int getMass()
    {
        return _mass;
    }

    public void setMass(int mass)
    {
        this._mass = mass;
    }

    public int getArmor()
    {
        return _armor;
    }

    public void setArmor(int armor)
    {
        this._armor = armor;
    }

    public int getFuelCapacity()
    {
        return _fuelCapacity;
    }

    public void setFuelCapacity(int fuelBonus)
    {
        this._fuelCapacity = fuelBonus;
    }

    public int getFuelGenerationPerYear()
    {
        return _fuelGenerationPerYear;
    }

    public void setFuelGenerationPerYear(int fuelRegenerationRate)
    {
        this._fuelGenerationPerYear = fuelRegenerationRate;
    }

    public int getInitiative()
    {
        return _initiative;
    }

    public void setInitiative(int initiative)
    {
        this._initiative = initiative;
    }

    public int getFleetHealBonus()
    {
        return _fleetHealBonus;
    }

    public void setFleetHealBonus(int fleetHealBonus)
    {
        this._fleetHealBonus = fleetHealBonus;
    }

    public bool isDoubleMineEfficiency()
    {
        return _doubleMineEfficiency;
    }

    public void setDoubleMineEfficiency(bool doubleMineEfficiency)
    {
        this._doubleMineEfficiency = doubleMineEfficiency;
    }

    public bool isBuiltInScannerForJoaT()
    {
        return _builtInScannerForJoaT;
    }

    public void setBuiltInScannerForJoaT(bool builtInScannerForJoaT)
    {
        this._builtInScannerForJoaT = builtInScannerForJoaT;
    }

    public List<TechHullSlot> getSlots()
    {
        return _slots;
    }

    public void setSlots(List<TechHullSlot> slots)
    {
        this._slots = slots;
    }

    public bool isStarbase()
    {
        return _starbase;
    }

    public void setStarbase(bool starbase)
    {
        this._starbase = starbase;
    }

    public TechHull mass(int mass)
    {
        this._mass = mass;
        return this;
    }

    public TechHull armor(int armor)
    {
        this._armor = armor;
        return this;
    }

    public TechHull fuelCapacity(int fuelCapacity)
    {
        this._fuelCapacity = fuelCapacity;
        return this;
    }

    public TechHull fuelGenerationPerYear(int fuelGenerationPerYear)
    {
        this._fuelGenerationPerYear = fuelGenerationPerYear;
        return this;
    }

    public TechHull initiative(int initiative)
    {
        this._initiative = initiative;
        return this;
    }

    public TechHull fleetHealBonus(int fleetHealBonus)
    {
        this._fleetHealBonus = fleetHealBonus;
        return this;
    }

    public TechHull doubleMineEfficiency(bool doubleMineEfficiency)
    {
        this._doubleMineEfficiency = doubleMineEfficiency;
        return this;
    }

    public TechHull builtInScannerForJoaT(bool builtInScannerForJoaT)
    {
        this._builtInScannerForJoaT = builtInScannerForJoaT;
        return this;
    }

    public TechHull starbase(bool starbase)
    {
        this._starbase = starbase;
        return this;
    }
}

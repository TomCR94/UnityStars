using System;
using UnityEngine;

[System.Serializable]
public class TechLevel {
    [SerializeField]
    protected int _energy;
    [SerializeField]
    protected int _weapons;
    [SerializeField]
    protected int _propulsion;
    [SerializeField]
    protected int _construction;
    [SerializeField]
    protected int _electronics;
    [SerializeField]
    protected int _biotechnology;
    
    public TechLevel()
    {
    }

    public TechLevel(int energy, int weapons, int propulsion, int construction, int electronics, int biotechnology)
    {
        this._energy = energy;
        this._weapons = weapons;
        this._propulsion = propulsion;
        this._construction = construction;
        this._electronics = electronics;
        this._biotechnology = biotechnology;
    }

    public new string ToString()
    {
        return "TechLevel [energy=" + _energy + ", weapons=" + _weapons + ", propulsion=" + _propulsion + ", construction=" + _construction + ", electronics="
               + _electronics + ", biotechnology=" + _biotechnology + "]";
    }

    /**
     * Set the tech level to a given value
     */
    public void setLevel(TechField field, int level)
    {
        switch (field)
        {
            case TechField.Biotechnology:
                _biotechnology = level;
                return;
            case TechField.Construction:
                _construction = level;
                return;
            case TechField.Electronics:
                _electronics = level;
                return;
            case TechField.Energy:
                _energy = level;
                return;
            case TechField.Propulsion:
                _propulsion = level;
                return;
            case TechField.Weapons:
                _weapons = level;
                return;
        }
        
    }

    /**
     * Get the value for a given TechField
     */
    public int level(TechField field)
    {
        switch (field)
        {
            case TechField.Biotechnology:
                return _biotechnology;
            case TechField.Construction:
                return _construction;
            case TechField.Electronics:
                return _electronics;
            case TechField.Energy:
                return _energy;
            case TechField.Propulsion:
                return _propulsion;
            case TechField.Weapons:
                return _weapons;
            default:
                return _energy;
        }
        
    }

    /**
     * Get the lowest field
     */
    public TechField lowest()
    {
        int lowestLevel = int.MaxValue;
        TechField lowestField = TechField.Energy;
        foreach (TechField field in Enum.GetValues(typeof(TechField)))
        {
            int value = level(field);
            if (value < lowestLevel)
            {
                lowestLevel = value;
                lowestField = field;
            }
        }

        return lowestField;
    }

    /**
     * Determine if this TechLevel is lower than another one
     */
    public bool lt(TechLevel other)
    {
        foreach (TechField field in Enum.GetValues(typeof(TechField)))
        {
            if (level(field) < other.level(field))
            {
                return true;
            }
        }
        return false;
    }

    /**
     * Determine if this TechLevel is greater than another one
     */
    public bool gt(TechLevel other)
    {
        bool foundGT = false;
        foreach (TechField field in Enum.GetValues(typeof(TechField)))
        {
            if (level(field) < other.level(field))
            {
                return false;
            }
            if (level(field) > other.level(field))
            {
                foundGT = true;
            }
        }
        return foundGT;
    }

    public bool equals(System.Object obj)
    {
        if (this == obj)
            return true;
        if (obj == null)
            return false;
        if (GetType() != obj.GetType())
            return false;
        TechLevel other = (TechLevel)obj;
        if (_biotechnology != other._biotechnology)
            return false;
        if (_construction != other._construction)
            return false;
        if (_electronics != other._electronics)
            return false;
        if (_energy != other._energy)
            return false;
        if (_propulsion != other._propulsion)
            return false;
        if (_weapons != other._weapons)
            return false;
        return true;
    }

    public TechLevel energy(int energy)
    {
        this._energy = energy;
        return this;
    }

    public TechLevel weapons(int weapons)
    {
        this._weapons = weapons;
        return this;
    }

    public TechLevel propulsion(int propulsion)
    {
        this._propulsion = propulsion;
        return this;
    }

    public TechLevel construction(int construction)
    {
        this._construction = construction;
        return this;
    }

    public TechLevel electronics(int electronics)
    {
        this._electronics = electronics;
        return this;
    }

    public TechLevel biotechnology(int biotechnology)
    {
        this._biotechnology = biotechnology;
        return this;
    }

    public int getEnergy()
    {
        return _energy;
    }

    public void setEnergy(int energy)
    {
        this._energy = energy;
    }

    public int getWeapons()
    {
        return _weapons;
    }

    public void setWeapons(int weapons)
    {
        this._weapons = weapons;
    }

    public int getPropulsion()
    {
        return _propulsion;
    }

    public void setPropulsion(int propulsion)
    {
        this._propulsion = propulsion;
    }

    public int getConstruction()
    {
        return _construction;
    }

    public void setConstruction(int construction)
    {
        this._construction = construction;
    }

    public int getElectronics()
    {
        return _electronics;
    }

    public void setElectronics(int electronics)
    {
        this._electronics = electronics;
    }

    public int getBiotechnology()
    {
        return _biotechnology;
    }

    public void setBiotechnology(int biotechnology)
    {
        this._biotechnology = biotechnology;
    }

}

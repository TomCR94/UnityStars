using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Cargo : Mineral {

    [SerializeField]
    private int fuel;
    [SerializeField]
    private int colonists;

    public Cargo() : base()
    {
        colonists = 0;
        fuel = 0;
    }

    public Cargo(int ironium, int boranium, int germanium, int colonists, int fuel) : base(ironium, boranium, germanium)
    {
        this.colonists = colonists;
        this.fuel = fuel;
    }
    
    public string ToString()
    {
        return "Cargo [fuel=" + fuel + ", population=" + colonists + ", ironium=" + ironium + ", boranium=" + boranium + ", germanium=" + germanium + "]";
    }

    /**
     * Get the total amount of cargo
     * @return The total kT of cargo
     */
    public int getTotal()
    {
        return ironium + boranium + germanium + colonists;
    }

    /**
     * For a planet the total colonists is divided by 100, for cargo terms
     * @return
     */
    public int getTotalPlanet()
    {
        return ironium + boranium + germanium + (colonists / 100);
    }

    public int getAtIndex(int index)
    {
        if (index == 3)
        {
            return colonists;
        }
        return base.getAtIndex(index);
    }

    public void setAtIndex(int index, int value)
    {
        if (index == 3)
        {
            setColonists(value);
        }
        else
        {
            base.setAtIndex(index, value);
        }
    }

    public Cargo add(Cargo cargo)
    {
        return new Cargo(ironium + cargo.getIronium(), boranium + cargo.getBoranium(), germanium + cargo.getGermanium(), colonists + cargo.getColonists(), fuel + cargo.getFuel());
    }

    public Cargo add(Mineral cargo)
    {
        return new Cargo(ironium + cargo.getIronium(), boranium + cargo.getBoranium(), germanium + cargo.getGermanium(), colonists, fuel);
    }

    public Cargo subtract(Cargo cargo)
    {
        return new Cargo(ironium - cargo.getIronium(), boranium - cargo.getBoranium(), germanium - cargo.getGermanium(), colonists - cargo.getColonists(), fuel - cargo.getFuel());
    }

    public Cargo multiply(int value)
    {
        return new Cargo(ironium * value, boranium * value, germanium * value, colonists * value, fuel * value);
    }

    public void setColonists(int colonists)
    {
        this.colonists = colonists;
    }

    public int getColonists()
    {
        return colonists;
    }

    public void setFuel(int fuel)
    {
        this.fuel = fuel;
    }

    public int getFuel()
    {
        return fuel;
    }
}

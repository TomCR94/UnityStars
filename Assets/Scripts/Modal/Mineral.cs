using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mineral {

    [SerializeField]
    protected int ironium;
    [SerializeField]
    protected int boranium;
    [SerializeField]
    protected int germanium;

    public Mineral()
    {
        ironium = boranium = germanium;
    }

    public Mineral(int ironium, int boranium, int germanium)
    {
        this.ironium = ironium;
        this.boranium = boranium;
        this.germanium = germanium;
    }

    public Mineral(Mineral m)
    {
        this.ironium = m.ironium;
        this.boranium = m.boranium;
        this.germanium = m.germanium;
    }
    
    public string toString()
    {
        return "Mineral [ironium=" + ironium + ", boranium=" + boranium + ", germanium=" + germanium + "]";
    }

    public string toTabbedString()
    {
        return ironium + " " + boranium + " " + germanium;
    }

    public int getAtIndex(int index)
    {
        if (index == 0)
        {
            return ironium;
        }
        else if (index == 1)
        {
            return boranium;
        }
        else if (index == 2)
        {
            return germanium;
        }
        else
        {
            return -1;
        }
    }

    public void setAtIndex(int index, int value)
    {
        if (index == 0)
        {
            setIronium(value);
        }
        else if (index == 1)
        {
            setBoranium(value);
        }
        else if (index == 2)
        {
            setGermanium(value);
        }
        else
        {
            
        }
    }

    public Mineral add(Mineral cost)
    {
        return new Mineral(ironium + cost.getIronium(), boranium + cost.getBoranium(), germanium + cost.getGermanium());
    }

    public Mineral add(int quantity)
    {
        return new Mineral(ironium + quantity, boranium + quantity, germanium + quantity);
    }

    public Mineral subtract(Mineral cost)
    {
        return new Mineral(ironium - cost.getIronium(), boranium - cost.getBoranium(), germanium - cost.getGermanium());
    }


    public int getIronium()
    {
        return ironium;
    }

    public void setIronium(int ironium)
    {
        this.ironium = ironium;
    }

    public void addIronium(int ironium)
    {
        this.ironium += ironium;
    }

    public int getBoranium()
    {
        return boranium;
    }

    public void setBoranium(int boranium)
    {
        this.boranium = boranium;
    }

    public void addBoranium(int boranium)
    {
        this.boranium += boranium;
    }

    public int getGermanium()
    {
        return germanium;
    }

    public void setGermanium(int germanium)
    {
        this.germanium = germanium;
    }

    public void addGermanium(int germanium)
    {
        this.germanium += germanium;
    }
}

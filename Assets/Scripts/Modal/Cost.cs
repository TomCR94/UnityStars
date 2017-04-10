using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Cost : Mineral {

    [SerializeField]
    private int resources;

    public Cost() : base()
    {
        resources = 0;
    }

    public Cost(int ironium, int boranium, int germanium, int resources) : base(ironium, boranium, germanium)
    {
        this.resources = resources;
    }

    public Cost(Cost cost) : base(cost.getIronium(), cost.getBoranium(), cost.getGermanium())
    {
        resources = cost.getResources();
    }

    public Cost(Mineral mineral, int resources) : base(mineral.getIronium(), mineral.getBoranium(), mineral.getGermanium())
    {
        this.resources = resources;
    }

    public new string toString()
    {
        return "Cost [ironium=" + ironium + ", boranium=" + boranium + ", germanium=" + germanium + ", resources=" + resources + "]";
    }

    public string toStringFormatted()
    {
        return "Ironium=" + ironium + "\nBoranium=" + boranium + "\nGermanium=" + germanium + "\nResources=" + resources;
    }


    public new int getAtIndex(int index)
    {
        if (index == 3)
        {
            return resources;
        }
        return base.getAtIndex(index);
    }

    public new void setAtIndex(int index, int value)
    {
        if (index == 3)
        {
            setResources(value);
        }
        else
        {
            base.setAtIndex(index, value);
        }
    }

    public Cost add(Cost cost)
    {
        return new Cost(ironium + cost.getIronium(), boranium + cost.getBoranium(), germanium + cost.getGermanium(), resources + cost.getResources());
    }

    public Cost subtract(Cost cost)
    {
        return new Cost(ironium - cost.getIronium(), boranium - cost.getBoranium(), germanium - cost.getGermanium(), resources - cost.getResources());
    }

    public Cost multiply(int value)
    {
        return new Cost(ironium * value, boranium * value, germanium * value, resources * value);
    }

    public int divide(Cost cost)
    {
        int numResources = 0;
        int numIronium = 0;
        int numBoranium = 0;
        int numGermanium = 0;
        numResources = numIronium = numBoranium = numGermanium = int.MaxValue;
        
        if (cost.resources > 0)
        {
            numResources = resources / cost.resources;
        }
        if (cost.ironium > 0)
        {
            numIronium = ironium / cost.getIronium();
        }
        if (cost.boranium > 0)
        {
            numBoranium = boranium / cost.getBoranium();
        }
        if (cost.germanium > 0)
        {
            numGermanium = germanium / cost.getGermanium();
        }

        int result = Mathf.Min(numResources, Mathf.Min(numIronium, Mathf.Min(numBoranium, numGermanium)));
        if (result == int.MaxValue)
        {
            throw new Boo.Lang.Runtime.RuntimeException("Divided by a Cost(0,0,0,0).");
        }

        return result;
    }

    public void setResources(int resources)
    {
        this.resources = resources;
    }

    public int getResources()
    {
        return resources;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TechEngine : TechHullComponent {

    [SerializeField]
    private static int MAX_WARP = 10;
    [SerializeField]
    private int[] fuelUsage = new int[MAX_WARP];

    public TechEngine() : base()
    {
    }

    public TechEngine(string name, Cost cost, TechRequirements techRequirements, int ranking, TechCategory category) : base(name, cost, techRequirements, ranking, category)
    {
    }

    public void setFuelUsage(int[] fuelUsage)
    {
        this.fuelUsage = fuelUsage;
    }

    public int[] getFuelUsage()
    {
        return fuelUsage;
    }

}

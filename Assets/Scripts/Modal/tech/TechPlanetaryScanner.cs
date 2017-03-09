using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TechPlanetaryScanner : Tech {
    [SerializeField]
    private int _scanRange;
    [SerializeField]
    private int _scanRangePen;

    public TechPlanetaryScanner() : base()
    {
    }

    public TechPlanetaryScanner(string name, Cost cost, TechRequirements techRequirements, int ranking, TechCategory category) : base(name, cost, techRequirements, ranking, category)
    {
    }


    public TechPlanetaryScanner scanRange(int scanRange)
    {
        this._scanRange = scanRange;
        return this;
    }


    public TechPlanetaryScanner scanRangePen(int scanRangePen)
    {
        this._scanRangePen = scanRangePen;
        return this;
    }


    public int getScanRange()
    {
        return _scanRange;
    }


    public void setScanRange(int scanRange)
    {
        this._scanRange = scanRange;
    }


    public int getScanRangePen()
    {
        return _scanRangePen;
    }


    public void setScanRangePen(int scanRangePen)
    {
        this._scanRangePen = scanRangePen;
    }

}

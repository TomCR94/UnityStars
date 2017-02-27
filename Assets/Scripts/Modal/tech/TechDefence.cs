using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechDefence : Tech {

    private int _defenseCoverage;

    public TechDefence() : base()
    {
    }

    public TechDefence(string name, Cost cost, TechRequirements techRequirements, int ranking, TechCategory category) : base(name, cost, techRequirements, ranking, category)
    {
    }

    public TechDefence defenseCoverage(int defenseCoverage)
    {
        this._defenseCoverage = defenseCoverage;
        return this;
    }

    public void setDefenseCoverage(int defenseCoverage)
    {
        this._defenseCoverage = defenseCoverage;
    }

    public int getDefenseCoverage()
    {
        return _defenseCoverage;
    }

}

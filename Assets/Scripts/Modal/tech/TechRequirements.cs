using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechRequirements : TechLevel {

    /**
    * the Primary Racial Trait required by this tech
    */
    private PRT _prtRequired;

    /**
     * the Primary Racial Trait that can't have this tech
     */
    private PRT _prtDenied;

    /**
     * The Lesser Racial Trait(s) required by this tech this is a list
     */
    private HashSet<LRT> _lrtsRequired = new HashSet<LRT>();

    /**
     * the Lesser Racial Trait(s) that are denied this tech
     */
    private HashSet<LRT> _lrtsDenied = new HashSet<LRT>();

    public TechRequirements() : base()
    {
    }

    public TechRequirements(int energy, int weapons, int propulsion, int construction, int electronics, int biotechnology) : base(energy, weapons, propulsion, construction, electronics, biotechnology)
    {
    }

    public TechRequirements(int energy, int weapons, int propulsion, int construction, int electronics, int biotechnology, PRT prtRequired, PRT prtDenied,
        HashSet<LRT> lrtsRequired, HashSet<LRT> lrtsDenied) : base(energy, weapons, propulsion, construction, electronics, biotechnology)
    {
        this._prtRequired = prtRequired;
        this._prtDenied = prtDenied;
        this._lrtsRequired = lrtsRequired;
        this._lrtsDenied = lrtsDenied;
    }

    public TechRequirements(int energy, int weapons, int propulsion, int construction, int electronics, int biotechnology, PRT prtRequired, PRT prtDenied,
        LRT lrtRequired, LRT lrtDenied) : base(energy, weapons, propulsion, construction, electronics, biotechnology)
    {
        this._prtRequired = prtRequired;
        this._prtDenied = prtDenied;
        HashSet<LRT> lrtset = new HashSet<LRT>();
        lrtset.Add(lrtRequired);
        HashSet<LRT> lrtDenyset = new HashSet<LRT>();
        lrtDenyset.Add(lrtDenied);
        this._lrtsRequired = lrtset;
        this._lrtsDenied = lrtDenyset;
    }

    public TechRequirements prtRequired(PRT prtRequired)
    {
        this._prtRequired = prtRequired;
        return this;
    }

    public TechRequirements prtDenied(PRT prtDenied)
    {
        this._prtDenied = prtDenied;
        return this;
    }

    public TechRequirements lrtsRequired(HashSet<LRT> lrtsRequired)
    {
        this._lrtsRequired = lrtsRequired;
        return this;
    }

    public TechRequirements lrtsRequired(LRT lrtRequired)
    {
        HashSet<LRT> reqs = new HashSet<LRT>();
        reqs.Add(lrtRequired);
        this._lrtsRequired = reqs;
        return this;
    }

    public TechRequirements lrtsDenied(HashSet<LRT> lrtsDenied)
    {
        this._lrtsDenied = lrtsDenied;
        return this;
    }

    public TechRequirements lrtsDenied(LRT lrtDenied)
    {
        HashSet<LRT> dens = new HashSet<LRT>();
        dens.Add(lrtDenied);
        this._lrtsRequired = dens;
        return this;
    }

    public new TechRequirements energy(int energy)
    {
        this._energy = energy;
        return this;
    }

    public new TechRequirements weapons(int weapons)
    {
        this._weapons = weapons;
        return this;
    }

    public new TechRequirements propulsion(int propulsion)
    {
        this._propulsion = propulsion;
        return this;
    }

    public new TechRequirements construction(int construction)
    {
        this._construction = construction;
        return this;
    }

    public new TechRequirements electronics(int electronics)
    {
        this._electronics = electronics;
        return this;
    }

    public new TechRequirements biotechnology(int biotechnology)
    {
        this._biotechnology = biotechnology;
        return this;
    }

    public PRT getPrtRequired()
    {
        return _prtRequired;
    }

    public void setPrtRequired(PRT prtRequired)
    {
        this._prtRequired = prtRequired;
    }

    public PRT getPrtDenied()
    {
        return _prtDenied;
    }

    public void setPrtDenied(PRT prtDenied)
    {
        this._prtDenied = prtDenied;
    }

    public HashSet<LRT> getLrtsRequired()
    {
        return _lrtsRequired;
    }

    public void setLrtsRequired(HashSet<LRT> lrtsRequired)
    {
        this._lrtsRequired = lrtsRequired;
    }

    public HashSet<LRT> getLrtsDenied()
    {
        return _lrtsDenied;
    }

    public void setLrtsDenied(HashSet<LRT> lrtsDenied)
    {
        this._lrtsDenied = lrtsDenied;
    }
}

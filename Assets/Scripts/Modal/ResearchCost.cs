using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ResearchCost {
    [SerializeField]
    private ResearchCostLevel energy;
    [SerializeField]
    private ResearchCostLevel weapons;
    [SerializeField]
    private ResearchCostLevel propulsion;
    [SerializeField]
    private ResearchCostLevel construction;
    [SerializeField]
    private ResearchCostLevel electronics;
    [SerializeField]
    private ResearchCostLevel biotechnology;

    public ResearchCost() : base()
    {
        energy = ResearchCostLevel.Standard;
        weapons = ResearchCostLevel.Standard;
        propulsion = ResearchCostLevel.Standard;
        construction = ResearchCostLevel.Standard;
        electronics = ResearchCostLevel.Standard;
        biotechnology = ResearchCostLevel.Standard;
    }

    public ResearchCost(ResearchCostLevel energy, ResearchCostLevel weapons, ResearchCostLevel propulsion, ResearchCostLevel construction,
        ResearchCostLevel electronics, ResearchCostLevel biotechnology) : base()
    {
        this.energy = energy;
        this.weapons = weapons;
        this.propulsion = propulsion;
        this.construction = construction;
        this.electronics = electronics;
        this.biotechnology = biotechnology;
    }
    
    public ResearchCost(ResearchCost researchCost)
    {
        this.energy = researchCost.energy;
        this.weapons = researchCost.weapons;
        this.propulsion = researchCost.propulsion;
        this.construction = researchCost.construction;
        this.electronics = researchCost.electronics;
        this.biotechnology = researchCost.biotechnology;
    }

    public ResearchCostLevel getAtIndex(int index)
    {
        switch (index)
        {
            case 0:
                return energy;
            case 1:
                return weapons;
            case 2:
                return propulsion;
            case 3:
                return construction;
            case 4:
                return electronics;
            case 5:
                return biotechnology;
            default:
                return energy;
        }
        
    }

    public ResearchCostLevel getForField(TechField field)
    {
        switch (field)
        {
            case TechField.Biotechnology:
                return biotechnology;
            case TechField.Construction:
                return construction;
            case TechField.Electronics:
                return electronics;
            case TechField.Energy:
                return energy;
            case TechField.Propulsion:
                return propulsion;
            case TechField.Weapons:
                return weapons;
            default:
                return energy;
        }
    }


    public ResearchCostLevel getEnergy()
    {
        return energy;
    }

    public void setEnergy(ResearchCostLevel energy)
    {
        this.energy = energy;
    }

    public ResearchCostLevel getWeapons()
    {
        return weapons;
    }

    public void setWeapons(ResearchCostLevel weapons)
    {
        this.weapons = weapons;
    }

    public ResearchCostLevel getPropulsion()
    {
        return propulsion;
    }

    public void setPropulsion(ResearchCostLevel propulsion)
    {
        this.propulsion = propulsion;
    }

    public ResearchCostLevel getConstruction()
    {
        return construction;
    }

    public void setConstruction(ResearchCostLevel construction)
    {
        this.construction = construction;
    }

    public ResearchCostLevel getElectronics()
    {
        return electronics;
    }

    public void setElectronics(ResearchCostLevel electronics)
    {
        this.electronics = electronics;
    }

    public ResearchCostLevel getBiotechnology()
    {
        return biotechnology;
    }

    public void setBiotechnology(ResearchCostLevel biotechnology)
    {
        this.biotechnology = biotechnology;
    }

}

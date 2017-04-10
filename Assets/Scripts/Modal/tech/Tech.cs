using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tech {
    
    private string name;
    
    private Cost cost = new Cost();
    
    private TechRequirements techRequirements;
    
    private int ranking;
    
    private TechCategory category;

    public Tech()
    {
    }

    public Tech(string name, Cost cost, TechRequirements techRequirements, int ranking, TechCategory category)
    {
        this.name = name;
        this.cost = cost;
        this.techRequirements = techRequirements;
        this.ranking = ranking;
        this.category = category;
    }

    public string getName()
    {
        return name;
    }

    public void setName(string name)
    {
        this.name = name;
    }

    public Cost getCost()
    {
        return cost;
    }

    public void setCost(Cost cost)
    {
        this.cost = cost;
    }

    public TechRequirements getTechRequirements()
    {
        return techRequirements;
    }

    public void setTechRequirements(TechRequirements techRequirements)
    {
        this.techRequirements = techRequirements;
    }

    public int getRanking()
    {
        return ranking;
    }

    public void setRanking(int ranking)
    {
        this.ranking = ranking;
    }

    public TechCategory getCategory()
    {
        return category;
    }

    public void setCategory(TechCategory category)
    {
        this.category = category;
    }

}

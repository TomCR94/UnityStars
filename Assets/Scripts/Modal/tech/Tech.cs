using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tech {

    /**
   * the tech name
   */
    private string name;

    /**
     * the cost of this tech
     */
    private Cost cost = new Cost();

    /**
     * the requirements of this tech
     */
    private TechRequirements techRequirements;

    /**
     * where this tech ranks in the UI list order
     */
    private int ranking;

    /**
     * the category this tech belongs to, i.e. armor, hull, etc.
     */
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

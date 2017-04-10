using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetKnowledge : AbstractStarsObject_NonMono {

    [SerializeField]
    private string planetId;
    
    [SerializeField]
    private Mineral concMinerals = new Mineral();
    
    [SerializeField]
    private Hab hab = new Hab();
    
    private Player owner;

    [SerializeField]
    private string ownerID;
    
    [SerializeField]
    private int population;
    
    [SerializeField]
    private int reportYear;

    public PlanetKnowledge()
    {
        ownerID = "";
        population = 0;
        reportYear = -1;
    }

    public PlanetKnowledge(Planet planet)
    {
        this.planetId = planet.getID();
    }
    
    public void discover(int year, Planet planet)
    {
        this.reportYear = year;
        this.population = planet.getPopulation();
        this.hab = new Hab(planet.getHab());
        this.owner = planet.getOwner();
        this.ownerID = planet.getOwnerID();
        this.concMinerals = new Mineral(planet.getConcMinerals());
    }

    public Mineral getConcMinerals()
    {
        return concMinerals;
    }

    public void setConcMinerals(Mineral concMinerals)
    {
        this.concMinerals = concMinerals;
    }

    public Hab getHab()
    {
        return hab;
    }

    public void setHab(Hab hab)
    {
        this.hab = hab;
    }

    public Player getOwner()
    {
        return owner;
    }

    public void setOwner(Player owner)
    {
        if (owner != null)
            this.ownerID = owner.getID();
        this.owner = owner;
    }

    public int getPopulation()
    {
        return population;
    }

    public void setPopulation(int population)
    {
        this.population = population;
    }

    public int getReportYear()
    {
        return reportYear;
    }

    public void setReportYear(int reportYear)
    {
        this.reportYear = reportYear;
    }

    public void setPlanetId(string planetId)
    {
        this.planetId = planetId;
    }

    public string getPlanetId()
    {
        return planetId;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

[Serializable]
public class Planet : MapObject, CargoHolder {

    private PlanetGameObject planetGameObject;

    [SerializeField]
    private ProductionQueue queue = new ProductionQueue();
    
    private List<Fleet> orbitingFleets = new List<Fleet>();

    [SerializeField]
    private int mines;
    [SerializeField]
    private int factories;
    [SerializeField]
    private int defenses;
    [SerializeField]
    private bool contributesToResearch;
    [SerializeField]
    private bool homeworld;
    [SerializeField]
    private bool scanner;

    private Player owner;

    [SerializeField]
    private string ownerID;
    
    [SerializeField]
    private string starbaseID;

    [SerializeField]
    private Cargo cargo = new Cargo();

    [SerializeField]
    private Mineral mineYears = new Mineral();

    [SerializeField]
    private Hab hab = new Hab();

    [SerializeField]
    private Mineral concMinerals = new Mineral();

    public PlanetGameObject PlanetGameObject
    {
        get
        {
            return planetGameObject;
        }

        set
        {
            planetGameObject = value;
        }
    }

    public Planet() : base()
    {
        queue.setPlanet(this);
    }

    public Planet(string name, int x, int y) : base(name, x, y)
    {
        queue.setPlanet(this);
    }

    public new string getID()
    {
        return getName();
    }

    public void clone(Planet planet)
    {
        _name = planet._name;
        setID(planet.getID());
        x = planet.x;
        y = planet.y;

        setConcMinerals(planet.getConcMinerals());
        setCargo(planet.getCargo());
        setMineYears(planet.getMineYears());
        setHab(planet.getHab());
        setMines(planet.getMines());
        setFactories(planet.getFactories());
        setDefenses(planet.getDefenses());
        setPopulation(planet.getPopulation());
        setOwner(planet.getOwner());
        setQueue(planet.getQueue());
        setScanner(planet.scanner);

    }

    public string ToString()
    {
        return "Planet [name=" + getName() + ", x=" + x + ", y=" + y + "]";
    }

    public int getCargoCapacity()
    {
        return -1;
    }

    /**
     * Add an item to the Planet's ProductionQueue
     * 
     * @param type
     * @param quantity
     */
    public void addQueueItem(QueueItemType type, int quantity)
    {
        queue.getItems().Add(new ProductionQueueItem(type, quantity));
    }

    /**
     * Add a ship to the Planet's ProductionQueue
     * 
     * @param type
     * @param quantity
     * @param shipDesign
     */
    public void addQueueItem(QueueItemType type, int quantity, ShipDesign shipDesign)
    {
        queue.getItems().Add(new ProductionQueueItem(type, quantity, shipDesign));
    }

    /**
     * Add a ship that is to be built into a fleet to the Planet's ProductionQueue
     * 
     * @param type
     * @param quantity
     * @param shipDesign
     * @param fleetName
     */
    public void addQueueItem(QueueItemType type, int quantity, ShipDesign shipDesign, string fleetName)
    {
        queue.getItems().Add(new ProductionQueueItem(type, quantity, shipDesign, fleetName));
    }

    /**
     * Make this planet a homeworld planet for a race
     * 
     * @param player The player to make this homeworld for
     * @param year The year the homeworld was founded
     */
    public void makeHomeworld(Player player, int year)
    {
        owner = player;
        ownerID = owner.getID();
        Race race = player.getRace();

        System.Random random = new System.Random();
        // generate mineral concentrations
        int min = Consts.minHWMineralConc;
        int max = Consts.maxStartingConc;
        concMinerals = new Mineral(random.Next(max) + min, random.Next(max) + min, random.Next(max) + min);
        
        // generate perfect hab range
        hab = new Hab();
        hab.setGrav(((race.getHabHigh().getGrav() - race.getHabLow().getGrav()) / 2) + race.getHabLow().getGrav());
        hab.setTemp(((race.getHabHigh().getTemp() - race.getHabLow().getTemp()) / 2) + race.getHabLow().getTemp());
        hab.setRad(((race.getHabHigh().getRad() - race.getHabLow().getRad()) / 2) + race.getHabLow().getRad());

        // generate surf minerals
        min = Consts.minStartingSurf;
        max = Consts.maxStartingSurf;
        setCargo(new Cargo(random.Next(max) + min, random.Next(max) + min, random.Next(max) + min, 0, 0));
        
        // setup the population
        setPopulation(Consts.startingPopulation);
        if (race.hasLRT(LRT.LSP))
        {
            setPopulation((int)(getPopulation() * Consts.lowStartingPopFactor));
        }

        mines = Consts.startingMines;
        factories = Consts.startingFactories;
        defenses = Consts.startingDefenses;
        homeworld = true;
        contributesToResearch = true;
        scanner = true;
    }

    /**
     * Determine the number of resources this planet generates in a year
     * 
     * @return The number of resources this planet generates in a year
     */
    public int getResourcesPerYear()
    {
        if (owner != null)
        {
            Race race = owner.getRace();

            // compute resources from population
            int resourcesFromPop = getPopulation() / race.getColonistsPerResource();

            // compute resources from factories
            int resourcesFromFactories = factories * race.getFactoryOutput() / 10;

            // return the sum
            return resourcesFromPop + resourcesFromFactories;
        }
        else
        {
            return 0;
        }

    }

    /**
     * @return The mineral output of this planet if it is owned
     */
    public Mineral getMineralOutput()
    {
        if (owner != null)
        {
            int mineOutput = owner.getRace().getMineOutput();
            return new Mineral(mineralsPerYear(concMinerals.getIronium(), mines, mineOutput), mineralsPerYear(concMinerals.getBoranium(), mines, mineOutput),
                               mineralsPerYear(concMinerals.getGermanium(), mines, mineOutput));
        }
        else
        {
            return new Mineral();
        }
    }

    /**
     * Get the amount of minerals mined in one year, for one type
     * 
     * @param mineralConcentration The concentration of minerals
     * @param mines The number of mines on the planet
     * @param mineOutput The mine output for the owner race
     * @return The mineral output for one year for one mineral conc
     */
    private int mineralsPerYear(int mineralConcentration, int mines, int mineOutput)
    {
        return (int)(((float)(mineralConcentration) / 100.0) * ((float)(mines) / 10.0) * (float)(mineOutput));
    }

    /**
     * @return the amount the population for this planet will grow next turn
     */
    public int getGrowthAmount()
    {
        if (owner != null)
        {
            Race race = owner.getRace();
            double capacity = (double)(getPopulation()) / (double)(getMaxPopulation());
            int popGrowth = (int)((double)(getPopulation()) * (race.getGrowthRate() / 100.0) * ((double)(race.getPlanetHabitability(hab)) / 100.0));

            if (capacity >= .25)
            {
                double crowdingFactor = 16.0 / 9.0 * (1.0 - capacity) * (1.0 - capacity);
                popGrowth = (int)((double)(popGrowth) * crowdingFactor);
            }

            return popGrowth;
        }
        else
        {
            return 0;
        }
    }

    /**
     * Get the max population for this planet
     * 
     * @return
     */
    public int getMaxPopulation()
    {
        return 1000000;
    }

    /**
     * Get the population density for this planet
     * 
     * @return
     */
    public double getPopulationDensity()
    {
        if (getPopulation() != null && getPopulation() > 0)
        {
            return (double)getPopulation() / getMaxPopulation();
        }
        return 0;
    }

    /**
     * Determine the number of resources this planet generates in a year
     * 
     * @return The number of resources this planet will contribute per year
     */
    public int getResourcesPerYearAvailable()
    {
        if (owner != null && contributesToResearch)
        {
            return (int)(getResourcesPerYear() * (1 - owner.getResearchAmount() / 100.0));
        }
        else
        {
            return getResourcesPerYear();
        }
    }

    /**
     * Determine the number of resources this planet generates in a year
     * 
     * @return The number of resources this planet will contribute per year
     */
    public int getResourcesPerYearResearch()
    {
        if (owner != null && contributesToResearch)
        {
            return (int)(getResourcesPerYear() * (owner.getResearchAmount() / 100.0));
        }
        else
        {
            return getResourcesPerYear();
        }
    }

    /**
     * @return The maximum number of mines this planet's population can support
     */
    public int getMaxMines()
    {
        if (owner != null)
        {
            return (int)(getPopulation() / 10000.0 * owner.getRace().getNumMines());
        }
        return 0;
    }

    /**
     * @return The maximum number of factories this planet's population can support
     */
    public int getMaxFactories()
    {
        if (owner != null)
        {
            return (int)(getPopulation() / 10000.0 * owner.getRace().getNumFactories());
        }
        return 0;
    }

    /**
     * @return The maximum number of defenses this planet's population can support
     */
    public int getMaxDefenses()
    {
        if (owner != null)
        {
            return (int)(getPopulation() / 10000.0 * 10);
        }
        return 0;
    }

    public Mineral getMineYears()
    {
        return mineYears;
    }

    public void setMineYears(Mineral mineYears)
    {
        this.mineYears = mineYears;
    }

    public int getMines()
    {
        return mines;
    }

    public void setMines(int mines)
    {
        this.mines = mines;
    }

    public int getFactories()
    {
        return factories;
    }

    public void setFactories(int factories)
    {
        this.factories = factories;
    }

    public int getDefenses()
    {
        return defenses;
    }

    public void setDefenses(int defenses)
    {
        this.defenses = defenses;
    }

    public bool isContributesToResearch()
    {
        return contributesToResearch;
    }

    public void setContributesToResearch(bool contributesToResearch)
    {
        this.contributesToResearch = contributesToResearch;
    }

    public bool isHomeworld()
    {
        return homeworld;
    }

    public void setHomeworld(bool homeworld)
    {
        this.homeworld = homeworld;
    }

    public void setOwner(Player owner)
    {
        if(owner != null)
            this.ownerID = owner.getID();
        this.owner = owner;
    }

    public Player getOwner()
    {
        return owner;
    }

    public void setOwnerID(string ID)
    {
        ownerID = ID;
    }

    public string getOwnerID()
    {
        return ownerID;
    }

    public int getPopulation()
    {
        return cargo.getColonists();
    }

    public void setPopulation(int population)
    {
        cargo.setColonists(population);
    }

    public Hab getHab()
    {
        return hab;
    }

    public void setHab(Hab hab)
    {
        this.hab = hab;
    }

    public Mineral getConcMinerals()
    {
        return concMinerals;
    }

    public void setConcMinerals(Mineral concMinerals)
    {
        this.concMinerals = concMinerals;
    }

    public void setScanner(bool scanner)
    {
        this.scanner = scanner;
    }

    public bool isScanner()
    {
        return scanner;
    }

    public void setQueue(ProductionQueue queue)
    {
        this.queue = queue;
    }

    public ProductionQueue getQueue()
    {
        return queue;
    }

    public List<Fleet> getOrbitingFleets()
    {
        return orbitingFleets;
    }

    public void setStarbase(Fleet starbase)
    {
        this.starbaseID = starbase.getID();
    }

    public Fleet getStarbase()
    {
        return FleetDictionary.instance.fleetDict[starbaseID];
    }

    public void setCargo(Cargo cargo)
    {
        this.cargo = cargo;
    }

    public Cargo getCargo()
    {
        return cargo;
    }

}

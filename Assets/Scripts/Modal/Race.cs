using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class Race : AbstractStarsObject_NonMono {

    public enum RaceType { custom, humanoid}

    [SerializeField]
    public RaceType raceType;

    [SerializeField]
    private User user;

    [SerializeField]
    private Player player;
   
    [SerializeField]
    private string name;

    [SerializeField]
    private string pluralName;

    [SerializeField]
    private PRT prt;
    [SerializeField]
    private List<LRT> lrts = new List<LRT>();
    [SerializeField]
    private Hab habLow = new Hab();


    [SerializeField]
    private Hab habHigh = new Hab();

    [SerializeField]
    private int growthRate;
    [SerializeField]
    private int colonistsPerResource;
    [SerializeField]
    private int factoryOutput;
    [SerializeField]
    private int factoryCost;
    [SerializeField]
    private int numFactories;
    [SerializeField]
    private bool factoriesCostLess;
    [SerializeField]
    private int mineOutput;
    [SerializeField]
    private int mineCost;
    [SerializeField]
    private int numMines;
    [SerializeField]
    private bool techsStartHigh;
    [SerializeField]
    private bool immuneGrav;
    [SerializeField]
    private bool immuneTemp;
    [SerializeField]
    private bool immuneRad;

    [SerializeField]
    private SpendLeftoverPointsOn spendLeftoverPointsOn;


    [SerializeField]
    private ResearchCost researchCost = new ResearchCost();

    // computed values
    [SerializeField]
    private int[] habCenter;

    [SerializeField]
    private int[] habWidth;

    public Race()
    {
        init();
    }

    /**
     * Copy constructor for initializing the race for a player
     * @param race The race to copy
     * @param player The player to assign this race to
     */
    public Race(Race race, Player player)
    {
        this.user = race.user;
        this.name = race.name;
        this.pluralName = race.pluralName;
        this.prt = race.prt;
        this.habLow = race.habLow;
        this.habHigh = race.habHigh;
        this.growthRate = race.growthRate;
        this.colonistsPerResource = race.colonistsPerResource;
        this.factoryOutput = race.factoryOutput;
        this.factoryCost = race.factoryCost;
        this.numFactories = race.numFactories;
        this.factoriesCostLess = race.factoriesCostLess;
        this.mineOutput = race.mineOutput;
        this.mineCost = race.mineCost;
        this.numMines = race.numMines;
        this.techsStartHigh = race.techsStartHigh;
        this.immuneGrav = race.immuneGrav;
        this.immuneTemp = race.immuneTemp;
        this.immuneRad = race.immuneRad;
        this.spendLeftoverPointsOn = race.spendLeftoverPointsOn;
        this.researchCost = new ResearchCost(race.researchCost);
        this.player = player;

        this.init();
    }
    
    public string toString()
    {
        return string.Format("<Race: %s (%s) PRT: %s, LRTs: %s, Hab(%s -> %s immune: (%s %s %s)), rc: %s>", name, pluralName, prt, lrts, habLow, habHigh,
                             immuneGrav, immuneTemp, immuneRad, researchCost);
    }

    /**
     * Initialize computed values, like habCenter and habWidth
     */
    public void init()
    {
        habCenter = new int[3];
        habWidth = new int[3];
        for (int i = 0; i < 3; i++)
        {
            habCenter[i] = (habHigh.getAtIndex(i) + habLow.getAtIndex(i)) / 2;
            habWidth[i] = (habHigh.getAtIndex(i) - habLow.getAtIndex(i)) / 2;
        }
    }

    /**
     * Get a copy of a humanoid Race
     * 
     * @return A Humanoid race
     */
    public static Race getHumanoid(User user)
    {
        Race race = new Race();
        race.setUser(user);
        race.setName("Humanoid");
        race.setPluralName("Humanoids");

        race.prt = PRT.JoaT;
        race.habLow = new Hab(15, 15, 15);
        race.habHigh = new Hab(85, 85, 85);
        race.growthRate = 15;
        race.colonistsPerResource = 1000;
        race.factoryOutput = 10;
        race.factoryCost = 10;
        race.numFactories = 10;
        race.factoriesCostLess = false;
        race.mineOutput = 10;
        race.mineCost = 5;
        race.numMines = 10;
        race.techsStartHigh = false;
        race.immuneGrav = false;
        race.immuneTemp = false;
        race.immuneRad = false;
        race.spendLeftoverPointsOn = SpendLeftoverPointsOn.SurfaceMinerals;
        race.researchCost = new ResearchCost(ResearchCostLevel.Standard, ResearchCostLevel.Standard, ResearchCostLevel.Standard, ResearchCostLevel.Standard,
                                             ResearchCostLevel.Standard, ResearchCostLevel.Standard);

        race.init();
        return race;
    }


    public void setHumanoid()
    {
        setName("Humanoid");
        setPluralName("Humanoids");

        prt = PRT.JoaT;
        habLow = new Hab(15, 15, 15);
        habHigh = new Hab(85, 85, 85);
        growthRate = 15;
        colonistsPerResource = 1000;
        factoryOutput = 10;
        factoryCost = 10;
        numFactories = 10;
        factoriesCostLess = false;
        mineOutput = 10;
        mineCost = 5;
        numMines = 10;
        techsStartHigh = false;
        immuneGrav = false;
        immuneTemp = false;
        immuneRad = false;
        spendLeftoverPointsOn = SpendLeftoverPointsOn.SurfaceMinerals;
        researchCost = new ResearchCost(ResearchCostLevel.Standard, ResearchCostLevel.Standard, ResearchCostLevel.Standard, ResearchCostLevel.Standard,
                                             ResearchCostLevel.Standard, ResearchCostLevel.Standard);

        init();
    }

    /**
     * Determine if this race has an LRT
     * @param lrt The LRT to check for
     * @return True if the race has the LRT, false otherwise
     */
    public bool hasLRT(LRT lrt)
    {
        return lrts.Contains(lrt);
    }

    /**
     * @return Return the center point of this hab, i.e. for hab 25 to 75 the center is 50 hab 60 to
     *         100 the center is 80
     */
    public int getHabCenter(int index)
    {
        return habCenter[index];
    }

    /**
     * Return whether this race is immune to a specific hab, by index
     * 
     * @param index The index of the hab, 0 == gravity, 1 == temp, 2 == radiation
     * @return Whether this race is immune to the specific hab type.
     */
    public bool isImmune(int index)
    {
        if (index == 0)
        {
            return immuneGrav;
        }
        else if (index == 1)
        {
            return immuneTemp;
        }
        else if (index == 2)
        {
            return immuneRad;
        }
        else
        {
            return false;
        }
    }

    /**
     * Get the habitability of this race for a given planet's hab value
     * 
     * @param planetHabData The Hab value for a planet.
     * @return The habiability of this race to that planet, with 100 being the best
     */
    public long getPlanetHabitability(Hab planetHabData)
    {
        long planetValuePoints = 0, redValue = 0, ideality = 10000;
        int habValue, habCenter, habUpper, habLower, fromIdeal, habRadius, poorPlanetMod, habRed, tmp;
        for (int habType = 0; habType < 3; habType++)
        {
            habValue = planetHabData.getAtIndex(habType);
            habCenter = this.habCenter[habType];
            habLower = this.habLow.getAtIndex(habType);
            habUpper = this.habHigh.getAtIndex(habType);

            if (isImmune(habType))
                planetValuePoints += 10000;
            else
            {
                if (habLower <= habValue && habUpper >= habValue)
                {
                    /* green planet */
                    fromIdeal = Mathf.Abs(habValue - habCenter) * 100;
                    if (habCenter > habValue)
                    {
                        habRadius = habCenter - habLower;
                        fromIdeal /= habRadius;
                        tmp = habCenter - habValue;
                    }
                    else
                    {
                        habRadius = habUpper - habCenter;
                        fromIdeal /= habRadius;
                        tmp = habValue - habCenter;
                    }
                    poorPlanetMod = ((tmp) * 2) - habRadius;
                    fromIdeal = 100 - fromIdeal;
                    planetValuePoints += fromIdeal * fromIdeal;
                    if (poorPlanetMod > 0)
                    {
                        ideality *= habRadius * 2 - poorPlanetMod;
                        ideality /= habRadius * 2;
                    }
                }
                else
                {
                    /* red planet */
                    if (habLower <= habValue)
                        habRed = habValue - habUpper;
                    else
                        habRed = habLower - habValue;

                    if (habRed > 15)
                        habRed = 15;

                    redValue += habRed;
                }
            }
        }

        if (redValue != 0)
        {
            return -redValue;
        }

        planetValuePoints = (long)(Mathf.Sqrt(planetValuePoints / 3) + 0.9f);
        planetValuePoints = planetValuePoints * ideality / 10000;

        return planetValuePoints;
    }

    public string getName()
    {
        return name;
    }

    public void setName(string name)
    {
        this.name = name;
    }

    public string getPluralName()
    {
        return pluralName;
    }

    public void setPluralName(string pluralName)
    {
        this.pluralName = pluralName;
    }

    public List<LRT> getLrts()
    {
        return lrts;
    }

    public void setLrts(List<LRT> lrts)
    {
        this.lrts = lrts;
    }

    public Hab getHabLow()
    {
        return habLow;
    }

    public void setHabLow(Hab habLow)
    {
        this.habLow = habLow;
        init();
    }

    public Hab getHabHigh()
    {
        return habHigh;
    }

    public void setHabHigh(Hab habHigh)
    {
        this.habHigh = habHigh;
        init();
    }

    public int getGrowthRate()
    {
        return growthRate;
    }

    public void setGrowthRate(int growthRate)
    {
        this.growthRate = growthRate;
    }

    public int getColonistsPerResource()
    {
        return colonistsPerResource;
    }

    public void setColonistsPerResource(int colonistsPerResource)
    {
        this.colonistsPerResource = colonistsPerResource;
    }

    public int getFactoryOutput()
    {
        return factoryOutput;
    }

    public void setFactoryOutput(int factoryOutput)
    {
        this.factoryOutput = factoryOutput;
    }

    public int getFactoryCost()
    {
        return factoryCost;
    }

    public void setFactoryCost(int factoryCost)
    {
        this.factoryCost = factoryCost;
    }

    public int getNumFactories()
    {
        return numFactories;
    }

    public void setNumFactories(int numFactories)
    {
        this.numFactories = numFactories;
    }

    public bool isFactoriesCostLess()
    {
        return factoriesCostLess;
    }

    public void setFactoriesCostLess(bool factoriesCostLess)
    {
        this.factoriesCostLess = factoriesCostLess;
    }

    public int getMineOutput()
    {
        return mineOutput;
    }

    public void setMineOutput(int mineOutput)
    {
        this.mineOutput = mineOutput;
    }

    public int getMineCost()
    {
        return mineCost;
    }

    public void setMineCost(int mineCost)
    {
        this.mineCost = mineCost;
    }

    public int getNumMines()
    {
        return numMines;
    }

    public void setNumMines(int numMines)
    {
        this.numMines = numMines;
    }

    public bool isTechsStartHigh()
    {
        return techsStartHigh;
    }

    public void setTechsStartHigh(bool techsStartHigh)
    {
        this.techsStartHigh = techsStartHigh;
    }

    public SpendLeftoverPointsOn getSpendLeftoverPointsOn()
    {
        return spendLeftoverPointsOn;
    }

    public void setSpendLeftoverPointsOn(SpendLeftoverPointsOn spendLeftoverPointsOn)
    {
        this.spendLeftoverPointsOn = spendLeftoverPointsOn;
    }

    public bool isImmuneGrav()
    {
        return immuneGrav;
    }

    public void setImmuneGrav(bool immuneGrav)
    {
        this.immuneGrav = immuneGrav;
    }

    public bool isImmuneTemp()
    {
        return immuneTemp;
    }

    public void setImmuneTemp(bool immuneTemp)
    {
        this.immuneTemp = immuneTemp;
    }

    public bool isImmuneRad()
    {
        return immuneRad;
    }

    public void setImmuneRad(bool immuneRad)
    {
        this.immuneRad = immuneRad;
    }

    public ResearchCost getResearchCost()
    {
        return researchCost;
    }

    public void setResearchCost(ResearchCost researchCost)
    {
        this.researchCost = researchCost;
    }

    public void setUser(User user)
    {
        this.user = user;
    }

    public User getUser()
    {
        return user;
    }

    public void setPRT(PRT prt)
    {
        this.prt = prt;
    }

    public void setPRT(int prt)
    {
        this.prt = (PRT)Enum.GetValues(typeof(PRT)).GetValue(prt);
    }

    public PRT getPRT()
    {
        return prt;
    }

    public void setPlayer(Player player)
    {
        this.player = player;
    }

    public Player getPlayer()
    {
        return player;
    }
    
    public string getPlayerId()
    {
        return player.getID();
    }

    /**
     * Get the research cost for a tech field/level
     * 
     * @param field The field to check
     * @param level The level to check
     * @return The cost to research that level
     */
    public int getResearchCostForLevel(TechField field, int level)
    {
        float cost = Consts.techResearchCost[level];
        ResearchCostLevel rcl = researchCost.getForField(field);

        if (rcl == ResearchCostLevel.Extra)
        {
            cost *= 1.75f;
        }
        else if (rcl == ResearchCostLevel.Less)
        {
            cost *= 0.5f;
        }

        return (int)cost;
    }
}

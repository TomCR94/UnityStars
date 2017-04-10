using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Player : AbstractStarsObject_NonMono {
    
    [SerializeField]
    private User user;
    
    [SerializeField]
    private Race race;
    
    [SerializeField]
    private string homeworldID;
    
    private List<ShipDesign> designs = new List<ShipDesign>();

    [SerializeField]
    private List<string> designIDs = new List<string>();
    
    [SerializeField]
    private List<Message> messages = new List<Message>();

    [SerializeField]
    private List<FleetKnowledge> fleetKnowledges = new List<FleetKnowledge>();

    [SerializeField]
    private List<PlanetKnowledge> planetKnowledges = new List<PlanetKnowledge>();

    [SerializeField]
    private String name;
    
    [SerializeField]
    private TechLevel techLevels = new TechLevel();
    
    [SerializeField]
    private TechLevel techLevelsSpent = new TechLevel();
    
    [SerializeField]
    private TechField currentResearchField = TechField.Energy;
    
    [SerializeField]
    private NextResearchField nextResearchField = NextResearchField.SameField;
    
    [SerializeField]
    private int researchAmount = 15;
    
    [SerializeField]
    private bool submittedTurn;
    
    [SerializeField]
    private int numFleetsBuilt;
    
    [SerializeField]
    private bool accepted = false;
    
    private PlayerTechs techs = new PlayerTechs();

    public Player()
    {
        currentResearchField = TechField.Energy;
        nextResearchField = NextResearchField.SameField;
        researchAmount = 15;
        submittedTurn = false;
        numFleetsBuilt = 0;
        submittedTurn = false;
        accepted = false;
    }

    public Player(User user)
    {
        this.user = user;
        this.name = user.getName();

    }

    private void Awake()
    {
        if (getRace().raceType == Race.RaceType.custom)
            return;
        else if (getRace().raceType == Race.RaceType.humanoid)
        {
            string filePath = Application.persistentDataPath + "/Races/Humanoid.race";
            string dataAsJson = File.ReadAllText(filePath);
            Debug.Log(dataAsJson);
            Race loadedData = JsonUtility.FromJson<Race>(dataAsJson);
            setRace(loadedData);
        }
    }

    override public void prePersist()
    {
        foreach (ShipDesign design in designs)
        {
            design.computeAggregate(this);
        }
    }



    /**
     * Initialize this player with a race by creating a copy of the race
     */
    public void initWithRace(Race race)
    {
        this.race = new Race(race, this);
    }

    /**
     * Initialize the starting tech levels If the player specifies techs to cost more and has the
     * racial trait techs_start_high, his techs start at an increased level
     */
    public void initStartingTechLevels()
    {
        if (race.getPRT() == PRT.JoaT)
        {
            foreach (TechField field in Enum.GetValues(typeof(TechField)))
            {
                techLevels.setLevel(field, Consts.startingTechLevelJoaT);
            }
        }

        int techStartVal = 0;
        if (race.isTechsStartHigh())
        {
            if (race.getPRT() == PRT.JoaT)
            {
                techStartVal = Consts.startingTechLevelExtra_joat;
            }
            else
            {
                techStartVal = Consts.startingTechLevelExtra;
            }

            foreach (TechField field in Enum.GetValues(typeof(TechField)))
            {
                if (race.getResearchCost().getForField(field) == ResearchCostLevel.Extra)
                {
                    techLevels.setLevel(field, techStartVal);
                    techLevelsSpent.setLevel(field, Consts.techResearchCost[techStartVal]);
                }
                else if (race.getPRT() == PRT.JoaT)
                {
                    techLevelsSpent.setLevel(field, Consts.techResearchCost[Consts.startingTechLevelJoaT]);
                }
            }
        }
    }

    /**
     * Return True if the player can have this tech, False otherwise
     */
    public bool hasTech(Tech tech)
    {
        return true;
        // check the PRT
        TechRequirements req = tech.getTechRequirements();
        if (req.getPrtRequired() != race.getPRT())
        {
            return false;
        }
        if (race.getPRT() == req.getPrtDenied())
        {
            return false;
        }

        foreach (LRT lrt in req.getLrtsRequired())
        {
            if (!race.getLrts().Contains(lrt))
            {
                return false;
            }
        }

        foreach (LRT lrt in req.getLrtsDenied())
        {
            if (race.getLrts().Contains(lrt))
            {
                return false;
            }
        }

        if (techLevels.lt(req))
        {
            return false;
        }

        return true;
    }
    
    public TechField getNextField()
    {
        TechField field = currentResearchField;
        
        if (nextResearchField == NextResearchField.SameField)
        {
            field = currentResearchField;
        }
        else if (nextResearchField != NextResearchField.LowestField)
        {
            field = (TechField)Enum.Parse(typeof(TechField), nextResearchField.ToString(), true);
        }
        
        int nextLevel = techLevels.level(field);
        if (nextLevel >= Consts.maxTechLevel || nextResearchField == NextResearchField.LowestField)
        {
            field = techLevels.lowest();
        }

        return field;
    }

    /**
     * Have a user 'discover' this planet, revealing it's private information as if it had been pen
     * scanned
     */
    public void discover(Game game, Planet planet)
    {
        PlanetKnowledge knowledge;
        if (!hasKnowledge(planet))
        {
            planetKnowledges.Add(new PlanetKnowledge(planet));
            Message.planetDiscovered(this, planet);
        }
        knowledge = getPlanetKnowledge(planet);

        knowledge.discover(game.getYear(), planet);
    }

    /**
     * Return true if the player has knowlege of the planet
     */
    public bool hasKnowledge(Planet planet)
    {
        foreach (PlanetKnowledge fk in planetKnowledges)
            if (fk.getPlanetId() == planet.getID())
                return true;
        return false;
    }

    /**
     * Return true if the player has knowledge of the fleet
     */
    public bool hasKnowledge(Fleet fleet)
    {
        foreach (FleetKnowledge fk in fleetKnowledges)
            if (fk.getFleetId() == fleet.getID())
                return true;
        return false;
    }

    /**
     * Get the knowledge for a player
     */
    public PlanetKnowledge getPlanetKnowledge(Planet planet)
    {
        if (hasKnowledge(planet))
        {
            foreach (PlanetKnowledge pk in planetKnowledges)
                if (pk.getPlanetId() == planet.getID())
                    return pk;
        }
        return null;
    }

    public FleetKnowledge getFleetKnowledge(Fleet fleet)
    {
        if (hasKnowledge(fleet))
        {
            foreach (FleetKnowledge pk in fleetKnowledges)
                if (pk.getFleetId() == fleet.getID())
                    return pk;
        }
        return null;
    }

    public User getUser()
    {
        return user;
    }

    public void setUser(User user)
    {
        this.user = user;
    }

    public Race getRace()
    {
        return race;
    }

    public void setRace(Race race)
    {
        this.race = race;
    }

    public TechLevel getTechLevels()
    {
        return techLevels;
    }

    public void setTechLevels(TechLevel techLevels)
    {
        this.techLevels = techLevels;
    }

    public TechLevel getTechLevelsSpent()
    {
        return techLevelsSpent;
    }

    public void setTechLevelsSpent(TechLevel techLevelsSpent)
    {
        this.techLevelsSpent = techLevelsSpent;
    }

    public TechField getCurrentResearchField()
    {
        return currentResearchField;
    }

    public void setCurrentResearchField(TechField currentResearchField)
    {
        this.currentResearchField = currentResearchField;
    }

    public NextResearchField getNextResearchField()
    {
        return nextResearchField;
    }

    public void setNextResearchField(NextResearchField nextResearchField)
    {
        this.nextResearchField = nextResearchField;
    }

    public int getResearchAmount()
    {
        return researchAmount;
    }

    public void setResearchAmount(int researchAmount)
    {
        this.researchAmount = researchAmount;
    }

    public bool isSubmittedTurn()
    {
        return submittedTurn;
    }

    public void setSubmittedTurn(bool submittedTurn)
    {
        this.submittedTurn = submittedTurn;
    }

    public int getNumFleetsBuilt()
    {
        return numFleetsBuilt;
    }

    public void setNumFleetsBuilt(int numFleetsBuilt)
    {
        this.numFleetsBuilt = numFleetsBuilt;
    }

    public bool isAccepted()
    {
        return accepted;
    }

    public void setAccepted(bool accepted)
    {
        this.accepted = accepted;
    }

    public bool isAi()
    {
        return getUser().isAi();
    }

    public void setAi(bool ai)
    {
        getUser().setAi(ai);
    }

    public void setName(string name)
    {
        this.name = name;
    }

    public string getName()
    {
        return user.getName();
    }

    public void setHomeworld(Planet homeworld)
    {
        this.homeworldID = homeworld.getID();
    }

    public Planet getHomeworld()
    {
        return PlanetDictionary.instance.planetDict[homeworldID];
    }

    public void setDesigns(List<ShipDesign> designs)
    {
        this.designs = designs;
    }

    public List<ShipDesign> getDesigns()
    {
        return designs;
    }

    public void setDesignIDs(List<string> designIDs)
    {
        this.designIDs = designIDs;
    }

    public List<string> getDesignIDs()
    {
        return designIDs;
    }

    public void setMessages(List<Message> messages)
    {
        this.messages = messages;
    }

    public List<Message> getMessages()
    {
        return messages;
    }

    public void setTechs(PlayerTechs techs)
    {
        this.techs = techs;
    }

    public PlayerTechs getTechs()
    {
        return techs;
    }

    public List<FleetKnowledge> getFleetKnowledges()
    {
        return fleetKnowledges;
    }

    public void setFleetKnowledges(List<FleetKnowledge> knowledges)
    {
        this.fleetKnowledges = knowledges;
    }

    public void setPlanetKnowledges(List<PlanetKnowledge> planetKnowledges)
    {
        this.planetKnowledges = planetKnowledges;
    }

    public List<PlanetKnowledge> getPlanetKnowledges()
    {
        return planetKnowledges;
    }

}

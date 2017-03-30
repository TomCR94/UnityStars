using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Player : AbstractStarsObject_NonMono {


    /**
     * The user of this player
     */
    [SerializeField]
    private User user;

    /**
     * The race of this player
     */
    [SerializeField]
    private Race race;

    /**
     * The homeworld of this player
     */
    [SerializeField]
    private string homeworldID;

    /**
     * The ship designs this player owns
     */
    private List<ShipDesign> designs = new List<ShipDesign>();

    [SerializeField]
    private List<string> designIDs = new List<string>();

    /**
     * The Messages for this player
     */
    [SerializeField]
    private List<Message> messages = new List<Message>();
    //TODO serialize knowledges
    [SerializeField]
    private List<FleetKnowledge> fleetKnowledges = new List<FleetKnowledge>();

    [SerializeField]
    private List<PlanetKnowledge> planetKnowledges = new List<PlanetKnowledge>();

    [SerializeField]
    private String name;

    /**
     * The current tech levels for this player
     */
    [SerializeField]
    private TechLevel techLevels = new TechLevel();

    /**
     * The amount of research points spent on each tech level.
     */
    [SerializeField]
    private TechLevel techLevelsSpent = new TechLevel();

    /**
     * The current field researching
     */
    [SerializeField]
    private TechField currentResearchField = TechField.Energy;

    /**
     * The next field to research
     */
    [SerializeField]
    private NextResearchField nextResearchField = NextResearchField.SameField;

    /**
     * The percentage of resources to spend on research
     */
    [SerializeField]
    private int researchAmount = 15;

    /**
     * Has this player submitted their turn?
     */
    [SerializeField]
    private bool submittedTurn;

    /**
     * The number of fleets this player has built
     */
    [SerializeField]
    private int numFleetsBuilt;

    /**
     * if this player has been invited to a game, default to not accept until the player actually
     * accepts the invitation
     */
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
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            Race loadedData = JsonUtility.FromJson<Race>(dataAsJson);
            setRace(loadedData);
            //race.setPlayer(this);
            //race.setUser(getUser());
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
     * 
     * @param race The race to copy
     */
    public void initWithRace(Race race)
    {
        // create a new race assigned to this player, so that the
        // user can edit the race without affecting the one in the game
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
     * 
     * @param tech The tech to check
     * @return True if this player can have this tech
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

    /**
     * @return Based on player settings, return the next field we should research
     */
    public TechField getNextField()
    {
        TechField field = currentResearchField;

        // if this is going to the same field, or the next field is specified, set it
        // to that field
        if (nextResearchField == NextResearchField.SameField)
        {
            field = currentResearchField;
        }
        else if (nextResearchField != NextResearchField.LowestField)
        {
            field = (TechField)Enum.Parse(typeof(TechField), nextResearchField.ToString(), true);
        }

        // make sure we aren't at the max of this field
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

        // discover this planet by copying the root knowledge to the user knowledge
        knowledge.discover(game.getYear(), planet);
    }

    /**
     * Return true if the player has knowlege of the planet
     * 
     * @param player The player to check knowledge for
     * @return False if the player does not have knowledge
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
     * 
     * @param player The player to check knowledge for
     * @return False if the player does not have knowledge
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
     * @param player The player
     * @return The PlanetKnowledge for this player, or null if not found
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

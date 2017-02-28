using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : AbstractStarsObject {


    [SerializeField]
    private User host;

    [SerializeField]
    private string name;

    [SerializeField]
    private Size size;

    [SerializeField]
    private Density density;

    [SerializeField]
    private int numPlayers;

    [SerializeField]
    private int year;
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    [SerializeField]
    private GameStatus status;

    [SerializeField]
    private List<Player> players = new List<Player>();

    //Dont serialize these lists
    private List<Planet> planets = new List<Planet>();
    private List<Fleet> fleets = new List<Fleet>();

    //Serialize references to their IDs instead    
    [SerializeField]
    private List<string> planetIDs = new List<string>();

    [SerializeField]
    private List<string> fleetIDs = new List<string>();

    public static Game instance;

    public Game()
    {
    }

    
    public Game(User host, string name, Size size, Density density, int numPlayers)
    {
        this.host = host;
        this.name = name;
        this.size = size;
        this.density = density;
        this.numPlayers = numPlayers;
    }

    private void Start()
    {
        instance = this;
        for (int i = 0; i < transform.childCount; i++)
            addPlayers(transform.GetChild(i).GetComponent<Player>());
    }

    override public void prePersist()
    {
        foreach (Player player in players)
        {
            player.prePersist();
        }
        foreach(Fleet fleet in fleets)
        {
            fleet.prePersist();
        }
    }

    public List<Player> getPlayers()
    {
        return players;
    }

    public void addPlayers(Player player)
    {
        this.players.Add(player);
    }

    public string getName()
    {
        return name;
    }

    public void setName(string name)
    {
        this.name = name;
    }

    public Size getSize()
    {
        return size;
    }

    public void setSize(Size size)
    {
        this.size = size;
    }

    public Density getDensity()
    {
        return density;
    }

    public void setDensity(Density density)
    {
        this.density = density;
    }

    public int getNumPlayers()
    {
        return numPlayers;
    }

    public void setNumPlayers(int numPlayers)
    {
        this.numPlayers = numPlayers;
    }

    public List<Planet> getPlanets()
    {
        return planets;
    }

    public void addPlanet(Planet planet)
    {
        this.planets.Add(planet);
        this.planetIDs.Add(planet.getID());
    }

    public void removePlanet(Planet planet)
    {
        this.planets.Remove(planet);
        this.planetIDs.Remove(planet.getID());
    }

    public List<Fleet> getFleets()
    {
        return fleets;
    }

    public void addFleet(Fleet fleet)
    {
        this.fleets.Add(fleet);
        this.fleetIDs.Add(fleet.getID());
    }

    public void removeFleet(Fleet fleet)
    {
        this.fleets.Remove(fleet);
        this.fleetIDs.Remove(fleet.getID());
    }

    public void setYear(int year)
    {
        this.year = year;
    }

    public int getYear()
    {
        return year;
    }

    public void setStatus(GameStatus status)
    {
        this.status = status;
    }

    public GameStatus getStatus()
    {
        return status;
    }

    public void setWidth(int width)
    {
        this.width = width;
    }

    public int getWidth()
    {
        return width;
    }

    public void setHeight(int height)
    {
        this.height = height;
    }

    public int getHeight()
    {
        return height;
    }

    public void setHost(User host)
    {
        this.host = host;
    }

    public User getHost()
    {
        return host;
    }

}

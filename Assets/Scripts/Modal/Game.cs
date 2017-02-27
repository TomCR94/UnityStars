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

    [SerializeField]
    private List<Planet> planets = new List<Planet>();

    [SerializeField]
    private List<Fleet> fleets = new List<Fleet>();

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

    public void setPlayers(List<Player> players)
    {
        this.players = players;
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

    public void setPlanets(List<Planet> planets)
    {
        this.planets = planets;
    }

    public List<Fleet> getFleets()
    {
        return fleets;
    }

    public void setFleets(List<Fleet> fleets)
    {
        this.fleets = fleets;
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Game : AbstractStarsObject_NonMono {
    
    [SerializeField]
    private string name = "Game1";

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

    override public void prePersist()
    {
        foreach (Player player in players)
        {
            player.prePersist();
        }
        foreach(Fleet fleet in FleetDictionary.instance.fleetDict.Values.ToList())
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
        return PlanetDictionary.instance.planetDict.Values.ToList();
    }

    public void addPlanet(Planet planet)
    {
        PlanetDictionary.instance.planetDict.Add(planet.getID(), planet);
    }

    public void removePlanet(Planet planet)
    {
        PlanetDictionary.instance.planetDict.Remove(planet.getID());
    }

    public List<Fleet> getFleets()
    {
        return FleetDictionary.instance.fleetDict.Values.ToList();
    }

    public void addFleet(Fleet fleet)
    {
        FleetDictionary.instance.fleetDict.Add(fleet.getID(), fleet);
    }

    public void removeFleet(Fleet fleet)
    {
        FleetDictionary.instance.fleetDict.Remove(fleet.getID());
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

    public Planet getPlanetByName(string name)
    {
        foreach (Planet planet in PlanetDictionary.instance.planetDict.Values.ToList())
        {
            if (planet.getName() == name)
                return planet;
        }
        return null;
    }

}

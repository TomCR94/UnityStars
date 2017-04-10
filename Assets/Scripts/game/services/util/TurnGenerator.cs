using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using UnityEngine.SceneManagement;

public class TurnGenerator
{
    private class Scanner
    {

        private MapObject mapObject;
        private int scanRange;
        private int scanRangePen;

        public Scanner(MapObject mapObject, int scanRange, int scanRangePen)
        {
            this.mapObject = mapObject;
            this.scanRange = scanRange;
            this.scanRangePen = scanRangePen;
        }

        public MapObject getMapObject()
        {
            return mapObject;
        }

        public int getScanRange()
        {
            return scanRange;
        }

        public int getScanRangePen()
        {
            return scanRangePen;
        }

    }

    private Game game;
    private FleetController fleetController;
    private PlanetController planetController;

    public TurnGenerator(Game game, FleetController fleetController, PlanetController planetController)
    {
        this.game = game;
        this.fleetController = fleetController;
        this.planetController = planetController;
    }
    /*
     * Add a year and go through each step in generating a turn for that year
     */
    public void generate()
    {

        game.setYear(game.getYear() + 1);
        
        initPlayers();

        performWaypointTasks(0);
        updateWormholeEntryPoints();
        moveFleets();
        minePlanets();
        buildAndProduce();
        grow();
        performWaypointTasks(0);

        scan(game);
        
        processTurns(game);
        VictoryCheck(game);

    }

    private void VictoryCheck(Game game)
    {
        foreach (Player player in game.getPlayers())
        {
            if (hasOver75PercentOfPlanets(game, player))
            {
                if (SceneManager.GetActiveScene().name == "Multiplayer")
                    new LogChallengeEventRequest()
                    .SetChallengeInstanceId(game.getName())
                    .SetEventKey("Win")
                    .Send((response) =>
                    {
                        if (response.HasErrors)
                        {
                        Debug.Log("Failed");
                        }
                    });
            }
        }
    }

    private bool hasOver75PercentOfPlanets(Game game, Player player)
    {

        int starsOwned = 0;

        foreach (Planet planet in game.getPlanets())
        {
            if (planet.getOwner().getID() == player.getID())
            {
                starsOwned++;
            }
        }

        int percentage = (starsOwned * 100)
                       / game.getPlanets().Count;

        if (percentage >= 75)
        {
            return true;
        }

        return false;
    }

    /**
     * init players to turn start values
     */
    private void initPlayers()
    {
        // clear out the player messages
        foreach (Player player in game.getPlayers())
        {
            player.getFleetKnowledges().Clear();
            player.setSubmittedTurn(false);
        }
    }

    /*
     * Perform waypoint tasks in the given order
     * 
     * Colonise
     * Scrap 
     * Invade
     * Unload
     * Bomb
     * Stabilise
     * Others
     * 
     */
    private void performWaypointTasks(int index)
    {
        List<Fleet> coloniseFleets = new List<Fleet>();
        List<Fleet> scrapFleets = new List<Fleet>();
        List<Fleet> otherFleets = new List<Fleet>();


        List<Fleet> invadeFleets = new List<Fleet>();
        List<Fleet> unloadCargoFleets = new List<Fleet>();
        List<Fleet> bombFleets = new List<Fleet>();
        List<Fleet> StabilizeFleets = new List<Fleet>();

        foreach (Fleet fleet in game.getFleets())
        {
            if (fleet.getWaypoints().Count > index)
                switch (fleet.getWaypoints()[index].getTask())
                {
                    case WaypointTask.Colonise:
                        coloniseFleets.Add(fleet);
                        break;
                    case WaypointTask.ScrapFleet:
                        scrapFleets.Add(fleet);
                        break;
                    case WaypointTask.Invade:
                        invadeFleets.Add(fleet);
                        break;
                    case WaypointTask.UnloadCargo:
                        unloadCargoFleets.Add(fleet);
                        break;
                    case WaypointTask.Bomb:
                        bombFleets.Add(fleet);
                        break;
                    case WaypointTask.Stabilize:
                        StabilizeFleets.Add(fleet);
                        break;
                    default:
                        otherFleets.Add(fleet);
                        break;

                }
        }

        foreach (Fleet fleet in coloniseFleets)
        {
            fleetController.doTask(fleet, fleet.getWaypoints()[index]);
        }

        foreach (Fleet fleet in scrapFleets)
        {
            fleetController.doTask(fleet, fleet.getWaypoints()[index]);
        }

        foreach (Fleet fleet in invadeFleets)
        {
            fleetController.doTask(fleet, fleet.getWaypoints()[index]);
        }

        foreach (Fleet fleet in unloadCargoFleets)
        {
            fleetController.doTask(fleet, fleet.getWaypoints()[index]);
        }

        foreach (Fleet fleet in bombFleets)
        {
            fleetController.doTask(fleet, fleet.getWaypoints()[index]);
        }

        foreach (Fleet fleet in StabilizeFleets)
        {
            fleetController.doTask(fleet, fleet.getWaypoints()[index]);
        }

        foreach (Fleet fleet in otherFleets)
        {
            fleetController.doTask(fleet, fleet.getWaypoints()[index]);
        }

        if (index != 0)
        {
            foreach (Fleet fleet in game.getFleets())
            {
                if (fleet.getWaypoints().Count > index)
                {
                    // we've arrived, remove the waypoint
                    fleet.getWaypoints().RemoveAt(0);
                    if (fleet.getWaypoints().Count == 1)
                    {
                        Message.fleetCompletedAssignedOrders(fleet.getOwner(), fleet);
                    }
                }
            }

        }

    }

    private void updateWormholeEntryPoints()
    {
        Dictionary<Vector2, bool> planetLocs = new Dictionary<Vector2, bool>();

        foreach (Planet planet in game.getPlanets())
            planetLocs.Add(new Vector2(planet.getX(), planet.getY()), true);

        foreach (Wormhole wh in WormholeDictionary.instance.WormholeDict.Values.ToList())
        {
            if (wh.getStabilized())
                break;
            Debug.Log("Moving Wormhole" + wh.getName());
            int amountX = UnityEngine.Random.Range(-50, 50);
            int amountY = UnityEngine.Random.Range(-50, 50);


            while (!(wh.getX() + amountX < game.getWidth()) && !( wh.getX() + amountX > 0))
                amountX = UnityEngine.Random.Range(-50, 50);
            while (!(wh.getY() + amountY < game.getHeight()) && !(wh.getY() + amountY > 0))
                amountY = UnityEngine.Random.Range(-50, 50);
            if (isValidLocation(new Vector2(wh.getX() + amountX, wh.getY() + amountY), planetLocs, Consts.planetMinDistance))
            {
                wh.setX(wh.getX() + amountX);
                wh.setY(wh.getY() + amountY);
            }

            wh.WormholeGameObject.transform.localPosition = new Vector3(wh.getX() - game.getWidth() / 2, wh.getY() - game.getHeight() / 2);
        }

        foreach (Fleet fl in FleetDictionary.instance.fleetDict.Values.ToList())
        {
            foreach (Waypoint wp in fl.getWaypoints())
            {
                if (wp.getTarget() != null && WormholeDictionary.instance.WormholeDict.ContainsKey(wp.getTarget().getID()))
                {
                    wp.setX(WormholeDictionary.instance.WormholeDict[wp.getTarget().getID()].getX());
                    wp.setY(WormholeDictionary.instance.WormholeDict[wp.getTarget().getID()].getY());
                }
            }
        }

    }

    /*
     * Return true if the location is not already in (or close to another planet) planet_locs
     */
    private static bool isValidLocation(Vector2 loc, Dictionary<Vector2, bool> planetLocs, int offset)
    {
        int x = (int)loc.x;
        int y = (int)loc.y;
        if (planetLocs.ContainsKey(loc))
        {
            return false;
        }

        for (int yOffset = 0; yOffset < offset; yOffset++)
        {
            for (int xOffset = 0; xOffset < offset; xOffset++)
            {
                if (planetLocs.ContainsKey(new Vector2(x + xOffset, y + yOffset)))
                {
                    return false;
                }
                if (planetLocs.ContainsKey(new Vector2(x - xOffset, y + yOffset)))
                {
                    return false;
                }
                if (planetLocs.ContainsKey(new Vector2(x - xOffset, y - yOffset)))
                {
                    return false;
                }
                if (planetLocs.ContainsKey(new Vector2(x + xOffset, y - yOffset)))
                {
                    return false;
                }
            }
        }

        return true;
    }
    

    /**
     * Mine on any planet
     */
    private void minePlanets()
    {
        foreach (Planet planet in game.getPlanets())
        {
            if (planet.getOwner() != null)
            {
                planetController.mine(planet);
            }
        }
    }

    /**
     * Build things on the planet, do research, etc.
     */
    private void buildAndProduce()
    {
        Dictionary<Player, int> leftoverResources = new Dictionary<Player, int>();
        foreach (Player player in game.getPlayers())
        {
            leftoverResources.Add(player, 0);
        }
        foreach (Planet planet in game.getPlanets())
        {
            if (planet.getOwner() != null)
            {
                planetController.mine(planet);
                int leftover = planetController.buildAndProduce(planet) + planet.getResourcesPerYearResearch();
                if (leftoverResources.ContainsKey(planet.getOwner()))
                    leftoverResources[planet.getOwner()] = leftoverResources[planet.getOwner()] + leftover;
                else
                    leftoverResources.Add(planet.getOwner(), leftoverResources[planet.getOwner()] + leftover);

                planetController.managePopulation(planet);
                planet.getOwner().discover(game, planet);
            }
        }
        
        foreach (Player player in leftoverResources.Keys)
        {
            research(player, leftoverResources[player]);
        }
    }

    /**
     * Grow population
     */
    private void grow()
    {
        // generate each planet turn
        foreach (Planet planet in game.getPlanets())
        {
            if (planet.getOwner() != null)
            {
                planetController.managePopulation(planet);
            }
        }
    }

    /**
     * Move fleets, fleets run out of fuel, etc.
     */
    private void moveFleets()
    {
        // move any fleets
        List<Fleet> fleets = new List<Fleet>(game.getFleets());
        foreach (Fleet fleet in fleets)
        {
            fleetController.moveFleetTowards(fleet);
            if (fleet.isScrapped())
            {
                foreach (Player player in game.getPlayers())
                {
                    player.getFleetKnowledges().Remove(new FleetKnowledge(fleet));
                    foreach (Message message in player.getMessages())
                    {
                        if (message.getTarget() != null && message.getTarget().getID() == fleet.getID())
                        {
                            message.setTarget(null);
                        }
                    }
                }
                game.removeFleet(fleet);
            }
        }

    }

    /**
     * Do all scan operations for the players
     */
    public void scan(Game game)
    {
        foreach (Player player in game.getPlayers())
        {
            scanPlayer(player);
        }
    }

    /**
     * Perform scanning for a player
     */
    private void scanPlayer(Player player)
    {
        
        TechPlanetaryScanner planetaryScanner = player.getTechs().getBestPlanetaryScanner();
        int planetScanRange = (int)Mathf.Pow(planetaryScanner.getScanRange(), 2);
        int planetScanRangePen = (int)Mathf.Pow(planetaryScanner.getScanRangePen(), 2);
        
        List<Fleet> fleets = new List<Fleet>();
        List<Planet> planets = new List<Planet>();
        List<Scanner> scanners = new List<Scanner>();
        
        foreach (Fleet fleet in game.getFleets())
        {
            if (fleet.getOwner().getID() == player.getID())
            {
                fleet.discover(player, true);
                if (fleet.canScan())
                {
                    scanners.Add(new Scanner(fleet, (int)Mathf.Pow(fleet.getAggregate().getScanRange(), 2), (int)Mathf.Pow(fleet.getAggregate()
                                                                                                                                    .getScanRangePen(), 2)));
                }
            }
            else
            {
                fleets.Add(fleet);
            }
        }
        
        foreach (Planet planet in game.getPlanets())
        {
            if (planet.getOwner() != null && planet.getOwner().getID() == player.getID())
            {
                player.discover(game, planet);
                if (planet.isScanner())
                {
                    scanners.Add(new Scanner(planet, planetScanRange, planetScanRangePen));
                }
            }
            else
            {
                planets.Add(planet);
            }
        }
        
        foreach (Scanner scanner in scanners)
        {
            
            int index = 0;
            while (index < planets.Count)
            {
                Planet planetToScan = planets[index];
                
                if (scanner.getMapObject().dist(planetToScan) <= scanner.getScanRangePen())
                {
                    player.discover(game, planetToScan);
                    
                    foreach (Fleet fleet in planetToScan.getOrbitingFleets())
                    {
                        Debug.Log("Fleet: " + fleet.getName());
                        Debug.Log("Fleet Owner: " + fleet.getOwner().getID());
                        if (fleet.getOwner().getID() != player.getID())
                        {
                            fleet.discover(player, true);
                            fleets.Remove(fleet);
                        }
                    }
                    planets.RemoveAt(index);
                    index--;
                }
                index++;
            }
            
            index = 0;
            while (index < fleets.Count)
            {
                Fleet fleetToScan = fleets[index];
                
                if (scanner.getMapObject().dist(fleetToScan) <= scanner.getScanRangePen())
                {
                    fleetToScan.discover(player, true);

                    fleets.RemoveAt(index);
                    index--;
                }
                index++;
            }
            
            index = 0;
            while (index < fleets.Count)
            {
                Fleet fleetToScan = fleets[index];
                
                if (scanner.getMapObject().dist(fleetToScan) <= scanner.getScanRange())
                {
                    fleetToScan.discover(player, false);

                    fleets.RemoveAt(index);
                    index--;
                }
                index++;
            }

        }

    }

    /**
     * Apply a given number of resources to research possibly gaining tech levels
     */
    void research(Player player, int resources)
    {
        int currentSpent = player.getTechLevelsSpent().level(player.getCurrentResearchField());
        int currentLevel = player.getTechLevels().level(player.getCurrentResearchField());
        int nextLevelCost = player.getRace().getResearchCostForLevel(player.getCurrentResearchField(), currentLevel + 1);
        currentSpent += resources;

        if (currentSpent >= nextLevelCost)
        {
            player.getTechLevels().setLevel(player.getCurrentResearchField(), currentLevel + 1);
            TechField nextField = player.getNextField();
            Message.techLevel(player, player.getCurrentResearchField(), currentLevel + 1, nextField);
            player.getTechLevelsSpent().setLevel(player.getCurrentResearchField(), 0);
            player.setCurrentResearchField(nextField);
            research(player, currentSpent - nextLevelCost);
        }
        else
        {
            player.getTechLevelsSpent().setLevel(player.getCurrentResearchField(), currentSpent);
        }

    }

    /**
     * Process turns using the turn processors
     */
    public void processTurns(Game game)
    {
        foreach (Player player in game.getPlayers())
        {
            if (player.isAi())
            {
                ScoutTurnProcessor processor = ScoutTurnProcessor.instance;
                processor.init(player, game);
                processor.process();

                //Will only send colony ships to planets with ideal conditions, this can mean few if any in a Tiny & Sparse universe
                ColoniserTurnProcessor cProcessor = ColoniserTurnProcessor.instance;
                cProcessor.init(player, game);
                cProcessor.process();

                if (player.isAi())
                {
                    player.setSubmittedTurn(true);
                }
            }
        }

    }

}
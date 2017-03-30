using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnGenerator
{
    /**
     * Helper class to represent a scanner for scanning
     * 
     */
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

    /**
     * <pre>
     *         Generate a turn
     * 
     *         Stars! Order of Events 
     *         
     *         Scrapping fleets (w/possible tech gain) 
     *         Waypoint 0 unload tasks 
     *         Waypoint 0 Colonization/Ground Combat resolution (w/possible tech gain) 
     *         Waypoint 0 load tasks 
     *         Other Waypoint 0 tasks * 
     *         MT moves 
     *         In-space packets move and decay 
     *         PP packets (de)terraform 
     *         Packets cause damage 
     *         Wormhole entry points jiggle 
     *         Fleets move (run out of fuel, hit minefields (fields reduce as they are hit), stargate, wormhole travel) 
     *         Inner Strength colonists grow in fleets 
     *         Mass Packets still in space and Salvage decay 
     *         Wormhole exit points jiggle 
     *         Wormhole endpoints degrade/jump 
     *         SD Minefields detonate (possibly damaging again fleet that hit minefield during movement) 
     *         Mining 
     *         Production (incl. research, packet launch, fleet/starbase construction) 
     *         SS Spy bonus obtained 
     *         Population grows/dies 
     *         Packets that just launched and reach their destination cause damage 
     *         Random events (comet strikes, etc.) 
     *         Fleet battles (w/possible tech gain) 
     *         Meet MT 
     *         Bombing 
     *         Waypoint 1 unload tasks 
     *         Waypoint 1 Colonization/Ground Combat resolution (w/possible tech gain) 
     *         Waypoint 1 load tasks 
     *         Mine Laying 
     *         Fleet Transfer 
     *         CA Instaforming 
     *         Mine sweeping 
     *         Starbase and fleet repair 
     *         Remote Terraforming
     * </pre>
     */
    public void generate()
    {
        // time this logic

        game.setYear(game.getYear() + 1);

        // initialize the players to starting turn state
        initPlayers();

        performWaypointTasks(0);
        moveMysteryTrader();
        movePackets();
        updateWormholeEntryPoints();
        moveFleets();
        growISFleets();
        decayPackets();
        updateWormholeExitPoints();
        detonateMinefields();
        mine();
        produce();
        ssSpy();
        grow();
        randomEvents();
        battle();
        mysteryTrader();
        bombing();
        performWaypointTasks(0);
        layMines();
        transferFleets();
        terraformCAPlanets();
        sweepMines();
        repairFleets();
        remoteTerraform();

        scan(game);

        // do turn processing
        processTurns(game);
        
        game.setStatus(GameStatus.WaitingForSubmit);

    }

    /**
     * Initialize the players to turn start values
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

    /**
     * Perform any waypoint tasks, including scrapping and tech gain and do it in a specific order
     * <ul>
     * <li>Scrapping fleets (w/possible tech gain)</li>
     * <li>Waypoint 0 unload tasks</li>
     * <li>Waypoint 0 Colonization/Ground Combat resolution (w/possible tech gain)</li>
     * <li>Waypoint 0 load tasks</li>
     * <li>Other Waypoint 0 tasks</li>
     * </ul>
     */
    private void performWaypointTasks(int index)
    {
        List<Fleet> colonizeFleets = new List<Fleet>();
        List<Fleet> scrapFleets = new List<Fleet>();
        List<Fleet> otherFleets = new List<Fleet>();


        List<Fleet> invadeFleets = new List<Fleet>();
        List<Fleet> unloadCargoFleets = new List<Fleet>();
        List<Fleet> terraformFleets = new List<Fleet>();
        List<Fleet> bombFleets = new List<Fleet>();
        List<Fleet> StabilizeFleets = new List<Fleet>();

        foreach (Fleet fleet in game.getFleets())
        {
            if (fleet.getWaypoints().Count > index)
                switch (fleet.getWaypoints()[index].getTask())
                {
                    case WaypointTask.Colonize:
                        colonizeFleets.Add(fleet);
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
                    case WaypointTask.Terraform:
                        terraformFleets.Add(fleet);
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

        foreach (Fleet fleet in colonizeFleets)
        {
            fleetController.processTask(fleet, fleet.getWaypoints()[index]);
        }

        foreach (Fleet fleet in scrapFleets)
        {
            fleetController.processTask(fleet, fleet.getWaypoints()[index]);
        }

        foreach (Fleet fleet in invadeFleets)
        {
            fleetController.processTask(fleet, fleet.getWaypoints()[index]);
        }

        foreach (Fleet fleet in unloadCargoFleets)
        {
            fleetController.processTask(fleet, fleet.getWaypoints()[index]);
        }

        foreach (Fleet fleet in terraformFleets)
        {
            fleetController.processTask(fleet, fleet.getWaypoints()[index]);
        }

        foreach (Fleet fleet in bombFleets)
        {
            fleetController.processTask(fleet, fleet.getWaypoints()[index]);
        }

        foreach (Fleet fleet in StabilizeFleets)
        {
            fleetController.processTask(fleet, fleet.getWaypoints()[index]);
        }

        foreach (Fleet fleet in otherFleets)
        {
            fleetController.processTask(fleet, fleet.getWaypoints()[index]);
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

    private void moveMysteryTrader()
    {
        // TODO Auto-generated method stub

    }

    private void movePackets()
    {
        // TODO Auto-generated method stub

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
            int amountX = Random.Range(-50, 50);
            int amountY = Random.Range(-50, 50);


            while (!(wh.getX() + amountX < game.getWidth()) && !( wh.getX() + amountX > 0))
                amountX = Random.Range(-50, 50);
            while (!(wh.getY() + amountY < game.getHeight()) && !(wh.getY() + amountY > 0))
                amountY = Random.Range(-50, 50);
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

    /**
     * Return true if the location is not already in (or close to another planet) planet_locs
     * 
     * @param loc The location to check
     * @param planetLocs The locations of every planet so far
     * @param offset The offset to check for
     * @return True if this location (or near it) is not already in use
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

    private void growISFleets()
    {
        // TODO Auto-generated method stub

    }

    private void decayPackets()
    {
        // TODO Auto-generated method stub

    }

    private void updateWormholeExitPoints()
    {
        // TODO Auto-generated method stub

    }

    private void detonateMinefields()
    {
        // TODO Auto-generated method stub

    }

    /**
     * Mine on any planet, mine with remote miners
     */
    private void mine()
    {
        // generate each planet turn
        foreach (Planet planet in game.getPlanets())
        {
            if (planet.getOwner() != null)
            {
                planetController.mine(planet);
            }
        }

        foreach (Fleet fleet in game.getFleets())
        {
            if (!fleet.isScrapped())
            {
                // remote mine
            }
        }
    }

    /**
     * Build things on the planet, do research, etc.
     */
    private void produce()
    {
        Dictionary<Player, int> leftoverResources = new Dictionary<Player, int>();
        foreach (Player player in game.getPlayers())
        {
            leftoverResources.Add(player, 0);
        }
        // generate each planet turn
        foreach (Planet planet in game.getPlanets())
        {
            if (planet.getOwner() != null)
            {
                planetController.mine(planet);
                int leftover = planetController.build(planet) + planet.getResourcesPerYearResearch();
                if (leftoverResources.ContainsKey(planet.getOwner()))
                    leftoverResources[planet.getOwner()] = leftoverResources[planet.getOwner()] + leftover;
                else
                    leftoverResources.Add(planet.getOwner(), leftoverResources[planet.getOwner()] + leftover);

                planetController.grow(planet);
                planet.getOwner().discover(game, planet);
            }
        }

        // research for each player
        foreach (Player player in leftoverResources.Keys)
        {
            research(player, leftoverResources[player]);
        }
    }

    private void ssSpy()
    {
        // TODO Auto-generated method stub

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
                planetController.grow(planet);
            }
        }
    }

    private void randomEvents()
    {
        // TODO Auto-generated method stub

    }

    private void battle()
    {
        // TODO Auto-generated method stub

    }

    private void mysteryTrader()
    {
        // TODO Auto-generated method stub

    }

    private void bombing()
    {
        // TODO Auto-generated method stub

    }

    private void layMines()
    {
        // TODO Auto-generated method stub

    }

    private void transferFleets()
    {
        // TODO Auto-generated method stub

    }

    private void terraformCAPlanets()
    {
        // TODO Auto-generated method stub

    }

    private void sweepMines()
    {
        // TODO Auto-generated method stub

    }

    private void repairFleets()
    {
        // TODO Auto-generated method stub

    }

    private void remoteTerraform()
    {
        // TODO Auto-generated method stub

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
            fleetController.move(fleet);
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
     * 
     * @param game The game to do scan operations for
     */
    public void scan(Game game)
    {
        foreach (Player player in game.getPlayers())
        {
            Debug.Log(player.getName());
            Debug.Log(player.getTechs().getBestPlanetaryScanner());
            scanPlayer(player);
        }
    }

    /**
     * Perform scanning for a player
     * 
     * @param player The player to scan for
     */
    private void scanPlayer(Player player)
    {

        // initialize the planetary scanner ranges
        TechPlanetaryScanner planetaryScanner = player.getTechs().getBestPlanetaryScanner();
        int planetScanRange = (int)Mathf.Pow(planetaryScanner.getScanRange(), 2);
        int planetScanRangePen = (int)Mathf.Pow(planetaryScanner.getScanRangePen(), 2);

        // get a list of all the fleets and planets we can discover
        List<Fleet> fleets = new List<Fleet>();
        List<Planet> planets = new List<Planet>();
        List<Scanner> scanners = new List<Scanner>();

        // find all fleets that need to be scanned, or who act as scanners
        foreach (Fleet fleet in game.getFleets())
        {
            if (fleet.getOwner().getID() == player.getID())
            {
                // this is our fleet, discover it
                fleet.discover(player, true);
                if (fleet.canScan())
                {
                    scanners.Add(new Scanner(fleet, (int)Mathf.Pow(fleet.getAggregate().getScanRange(), 2), (int)Mathf.Pow(fleet.getAggregate()
                                                                                                                                    .getScanRangePen(), 2)));
                }
            }
            else
            {
                // remove any existing knowledge and add this fleet to the list of unknowns
                // fleet.getFleetKnowledges().remove(player.getId());
                fleets.Add(fleet);
            }
        }

        // find all planets that need to be scanned, or who act as scanners
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

        // scan with each of our scanners
        foreach (Scanner scanner in scanners)
        {

            // scan planets
            int index = 0;
            while (index < planets.Count)
            {
                Planet planetToScan = planets[index];

                // if this planet to scan is within range, have the owner of 'planet' discover it
                if (scanner.getMapObject().dist(planetToScan) <= scanner.getScanRangePen())
                {
                    player.discover(game, planetToScan);

                    // discover any orbiting fleets
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

            // scan fleets
            index = 0;
            while (index < fleets.Count)
            {
                Fleet fleetToScan = fleets[index];

                // if this planet to scan is within range, have the owner of 'planet' discover it
                if (scanner.getMapObject().dist(fleetToScan) <= scanner.getScanRangePen())
                {
                    fleetToScan.discover(player, true);

                    fleets.RemoveAt(index);
                    index--;
                }
                index++;
            }

            // scan fleets
            index = 0;
            while (index < fleets.Count)
            {
                Fleet fleetToScan = fleets[index];

                // if this planet to scan is within range, have the owner of 'planet' discover it
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
     * 
     * @param player The player to research
     * @param resources The resouces to apply
     */
    void research(Player player, int resources)
    {
        int currentSpent = player.getTechLevelsSpent().level(player.getCurrentResearchField());
        int currentLevel = player.getTechLevels().level(player.getCurrentResearchField());
        int nextLevelCost = player.getRace().getResearchCostForLevel(player.getCurrentResearchField(), currentLevel + 1);
        currentSpent += resources;

        // if we spent enough for a new level,
        // - gain the level
        // - reset our spent amount for the current level to 0
        // - set next next research level
        // - spend any leftovers again on research
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
            // save back out how much we've spent
            player.getTechLevelsSpent().setLevel(player.getCurrentResearchField(), currentSpent);
        }

    }

    /**
     * Process turns using the turn processors
     * 
     * @param game The game to process turns for
     */
    public void processTurns(Game game)
    {
        foreach (Player player in game.getPlayers())
        {
            if (player.isAi())
            {
                Debug.Log("Processing AI turn");

                ScoutTurnProcessor processor = ScoutTurnProcessor.instance;
                processor.init(player, game);
                processor.process();

                ColonizerTurnProcessor cProcessor = ColonizerTurnProcessor.instance;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadUniverseGenerator : MonoBehaviour
{

    public GameObject basePlanet, planetParent;
    public GameObject baseFleet;
    public string GameLocation;
    public GameGameObject game;

    private FleetController fleetController;
    private PlanetController planetController;
    private ShipDesigner shipDesigner;
    private TechStore techStore;

    public void setUniverseGenerator(string location, Game game, FleetController fleetController, PlanetController planetController, ShipDesigner shipDesigner, TechStore techStore)
    {
        this.GameLocation = location;
        this.game.setGame(game);
        this.fleetController = fleetController;
        this.planetController = planetController;
        this.shipDesigner = shipDesigner;
        this.techStore = techStore;
    }

    /**
     * Generate a new universe
     */
    public void generate()
    {
        generateGame();


        planetParent.GetComponent<RectTransform>().sizeDelta = new Vector2(game.getGame().getWidth() + 40, game.getGame().getHeight() + 40);

        generatePlanets(game.getGame());
        generateOrbitingFleets(game.getGame());
        /*
        foreach (Player player in game.getPlayers())
        {

            if (player.getRace().raceType == Race.RaceType.humanoid)
                player.getRace().setHumanoid();

            // up the tech level for testing
            // TODO: Remove this!
            // player.setTechLevels(new TechLevel(10, 10, 10, 10, 10, 10));
            // int cost = Consts.techResearchCost[10];
            // player.setTechLevelsSpent(new TechLevel(cost, cost, cost, cost, cost, cost));

            Message.info(player, "Welcome to the universe, go forth and conquer!");

            foreach (Planet planet in game.getPlanets())
            {
                if (planet.getOwner() == null)
                {
                    planet.makeHomeworld(player, game.getYear());
                    player.setHomeworld(planet);
                    planet.addQueueItem(QueueItemType.AutoMine, 5);
                    planet.addQueueItem(QueueItemType.AutoFactory, 5);
                    player.getPlanetKnowledges().Add(planet.getID(), new PlanetKnowledge(planet));
                    Message.homePlanet(player, planet);
                    break;
                }
            }

            // create ship designs for the player
            player.getTechs().init(player, techStore);
            foreach (TechHull hull in player.getTechs().getHulls())
            {
                ShipDesign design = shipDesigner.designShip(hull, player);
                design.computeAggregate(player);
                player.getDesigns().Add(design);
            }
            ShipDesign starbase = shipDesigner.designShip(techStore.getHull("Space Station"), player);
            starbase.computeAggregate(player);
            player.getDesigns().Add(starbase);

            // build fleets for this player
            foreach (ShipDesign design in player.getDesigns())
            {

                // create a new fleet for this design
                Fleet fleet = fleetController.create(design.getHullName(), player.getHomeworld().getX(), player.getHomeworld().getY(), player);
                fleet.addShipStack(design, 1);
                fleet.addWaypoint(player.getHomeworld().getX(), player.getHomeworld().getY(), 5, WaypointTask.None, player.getHomeworld());
                fleet.computeAggregate();

                GameObject go = GameObject.Instantiate(Resources.Load("Fleet") as GameObject, player.getHomeworld().PlanetGameObject.transform, false);
                go.transform.position = Vector3.zero;
                go.GetComponent<FleetGameObject>().setFleet(fleet);
                go.name = fleet.getName();
                go.SetActive(true);


                if (design.getHull().isStarbase())
                {
                    // if this is a starbase, add it to the player's homeworld
                    player.getHomeworld().setStarbase(fleet);
                    fleet.setOrbiting(player.getHomeworld());

                }
                else
                {
                    fleet.setFuel(fleet.getAggregate().getFuelCapacity());
                    fleet.setOrbiting(player.getHomeworld());

                    // set it to orbiting the homeworld
                    player.getHomeworld().getOrbitingFleets().Add(fleet);
                }

                // add this fleet to the various arrays
                // game.getFleets().put(fleet.getId(), fleet);
                game.addFleet(fleet);
                player.setNumFleetsBuilt(player.getNumFleetsBuilt() + 1);
            }

        }
        /*
         * Not going to start scanning before playing turn
         * 
        TurnGenerator tg = new TurnGenerator(game, fleetController, planetController);
        // scan everything!
        tg.scan(game);

        // do turn processing
        tg.processTurns(game);

        */
        game.getGame().setStatus(GameStatus.WaitingForSubmit);
    }

    private void generateGame()
    {
        DirectoryInfo dirInf = new DirectoryInfo(GameLocation);
        Debug.Log(dirInf.ToString());
        if (dirInf.Exists)
        {
            Debug.Log("GameLocation: " + GameLocation);
            Debug.Log("dirExists");
            FileInfo[] files = dirInf.GetFiles("*.json");
            foreach (FileInfo file in files)
                Debug.Log("File fount: " + file.Name);
            game.setGame(JsonUtility.FromJson<Game>(File.ReadAllText(files[0].FullName)));

        }
    }

    /**
     * Generate a new universe with planets and all
     * 
     * @param game The game to generate planets on
     */
    private void generatePlanets(Game game)
    {
        //game.getPlanets().Clear();
        int width, height;
        height = Consts.sizeToArea[game.getSize()];
        width = height;

        planetParent.GetComponent<RectTransform>().sizeDelta = new Vector2(width + 40, height + 40);

        game.setWidth(width);
        game.setHeight(height);



        DirectoryInfo dirInf = new DirectoryInfo(GameLocation + "/Planets/");
        Debug.Log(dirInf.ToString());
        if (dirInf.Exists)
        {
            FileInfo[] files = dirInf.GetFiles("*.planet");
            foreach (FileInfo file in files)
            {
                Planet planet = JsonUtility.FromJson<Planet>(File.ReadAllText(file.FullName));

                planet.getOrbitingFleets().Clear();

                GameObject go = GameObject.Instantiate(basePlanet, planetParent.transform);
                go.GetComponent<PlanetGameObject>().setPlanet(planet);

                foreach (Player player in game.getPlayers())
                {
                    if (player.getID() == planet.getOwnerID())
                    {
                        planet.setOwner(player);
                    }
                }

                game.addPlanet(go.GetComponent<PlanetGameObject>().getPlanet());
                go.transform.localPosition = new Vector3(go.GetComponent<PlanetGameObject>().getPlanet().getX() - width / 2, go.GetComponent<PlanetGameObject>().getPlanet().getY() - height / 2);
                go.name = planet.getName();
                go.SetActive(true);
            }
        }

    }

    private void generateOrbitingFleets(Game game)
    {
        DirectoryInfo dirInf = new DirectoryInfo(GameLocation + "/Fleets/");
        Debug.Log(dirInf.ToString());
        if (dirInf.Exists)
        {
            Debug.Log("DirExists");
            DirectoryInfo[] directories = dirInf.GetDirectories();

            foreach (DirectoryInfo subDirInfo in directories)
            {
                if (subDirInfo.Name != "Empty Space")
                {
                    Debug.Log(subDirInfo.Name);
                    FileInfo[] files = subDirInfo.GetFiles("*.fleet");
                    foreach (FileInfo file in files)
                    {

                        Debug.Log(file.Name);
                        Fleet fleet = JsonUtility.FromJson<Fleet>(File.ReadAllText(file.FullName));
                        GameObject go = GameObject.Instantiate(baseFleet, game.getPlanetByName(subDirInfo.Name).PlanetGameObject.transform, false);
                        go.transform.localPosition = Vector3.zero;
                        fleet.computeAggregate();
                        go.GetComponent<FleetGameObject>().setFleet(fleet);
                        game.addFleet(fleet);
                        fleet.setOrbiting(game.getPlanetByName(subDirInfo.Name));
                        go.name = fleet.getName();
                        go.SetActive(true);
                    }
                }
                else
                {
                    Debug.Log(subDirInfo.Name);
                    FileInfo[] files = subDirInfo.GetFiles("*.fleet");
                    foreach (FileInfo file in files)
                    {

                        Debug.Log(file.Name);
                        Fleet fleet = JsonUtility.FromJson<Fleet>(File.ReadAllText(file.FullName));
                        GameObject go = GameObject.Instantiate(baseFleet, planetParent.transform, false);
                        fleet.computeAggregate();
                        go.GetComponent<FleetGameObject>().setFleet(fleet);
                        game.addFleet(fleet);
                        go.transform.localPosition = new Vector3(go.GetComponent<FleetGameObject>().getFleet().getX() - game.getWidth() / 2, go.GetComponent<FleetGameObject>().getFleet().getY() - game.getHeight() / 2);
                        go.name = fleet.getName();
                        go.SetActive(true);
                    }
                }
            }
        }
    }
}

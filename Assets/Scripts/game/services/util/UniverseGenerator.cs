using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Utility class used to create a universe with the given game, controller and tech store configuration
 */
public class UniverseGenerator: MonoBehaviour {

    public GameObject basePlanet, baseWormhole, planetParent;
    public GameObject baseFleet;
    public GameGameObject game;

    private FleetController fleetController;
    private PlanetController planetController;
    private ShipDesigner shipDesigner;
    private TechStore techStore;
    
    public void setUniverseGenerator(Game game, FleetController fleetController, PlanetController planetController, ShipDesigner shipDesigner, TechStore techStore)
    {
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
        generatePlanets(game.getGame());
        generateWormholes(game.getGame());
        game.getGame().setYear(Consts.startingYear);

        foreach (Player player in game.getGame().getPlayers())
        {

            if (player.getRace().raceType == Race.RaceType.humanoid)
                player.getRace().setHumanoid();

            Message.info(player, "Welcome to the universe, go forth and conquer!");

            foreach (Planet planet in game.getGame().getPlanets())
            {
                if (planet.getOwner() == null)
                {
                    planet.makeHomeworld(player, game.getGame().getYear());
                    player.setHomeworld(planet);
                    planet.addQueueItem(QueueItemType.AutoMine, 5);
                    planet.addQueueItem(QueueItemType.AutoFactory, 5);
                    player.getPlanetKnowledges().Add(new PlanetKnowledge(planet));
                    Message.homePlanet(player, planet);
                    break;
                }
            }
            
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
            
            foreach (ShipDesign design in player.getDesigns())
            {
                
                Fleet fleet = fleetController.makeFleet(design.getHullName(), player.getHomeworld().getX(), player.getHomeworld().getY(), player);
                fleet.addShipStack(design, 1);
                fleet.addWaypoint(player.getHomeworld().getX(), player.getHomeworld().getY(), 5, WaypointTask.None, player.getHomeworld());
                fleet.computeAggregate();
                
                GameObject go = GameObject.Instantiate(baseFleet, player.getHomeworld().PlanetGameObject.transform, false);
                go.transform.position = Vector3.zero;
                go.GetComponent<FleetGameObject>().setFleet(fleet);
                go.name = fleet.getName();
                go.SetActive(true);


                if (design.getHull().isStarbase())
                {
                    player.getHomeworld().setStarbase(fleet);
                    fleet.setOrbiting(player.getHomeworld());

                }
                else
                {
                    fleet.setFuel(fleet.getAggregate().getFuelCapacity());
                    fleet.setOrbiting(player.getHomeworld());
                }
                game.getGame().addFleet(fleet);
                player.getFleetKnowledges().Add(new FleetKnowledge(fleet));
                player.setNumFleetsBuilt(player.getNumFleetsBuilt() + 1);
            }

        }
    }


    /**
     * Generate the Planets 
     */
    private void generatePlanets(Game game)
    {
        int width, height;
        height = Consts.sizeToArea[game.getSize()];
        width = height;

        planetParent.GetComponent<RectTransform>().sizeDelta = new Vector2(width + 40, height  + 40);

        game.setWidth(width);
        game.setHeight(height);

        int numPlanets = Consts.sizeToDensity[game.getSize()][game.getDensity()];
        NameGenerator ng = new NameGenerator();

        System.Random random = new System.Random();
        Dictionary<Vector2, bool> planetLocs = new Dictionary<Vector2, bool>();
        for (int i = 0; i < numPlanets; i++)
        {
            Vector2 loc = new Vector2(random.Next(width), random.Next(height));
            
            while (!isValidLocation(loc, planetLocs, Consts.planetMinDistance))
            {
                loc = new Vector2(random.Next(width), random.Next(height));
            }
            
            planetLocs.Add(loc, true);


            GameObject go = GameObject.Instantiate(basePlanet,  planetParent.transform);
            Planet planet = planetController.makePlanet(ng.nextName(), (int)loc.x, (int)loc.y);
            go.GetComponent<PlanetGameObject>().setPlanet(planet);
            planetController.randomise(go.GetComponent<PlanetGameObject>().getPlanet());
            game.addPlanet(go.GetComponent<PlanetGameObject>().getPlanet());
            go.transform.localPosition = new Vector3(go.GetComponent<PlanetGameObject>().getPlanet().getX() - width/2, go.GetComponent<PlanetGameObject>().getPlanet().getY() - height/2);
            go.name = planet.getName();
            go.SetActive(true);
        }

    }

    /**
     * Generate the Wormholes 
     */
    private void generateWormholes(Game game)
    {
        int width, height;
        height = Consts.sizeToArea[game.getSize()];
        width = height;

        int numWormholes = 1;

        if (game.getSize() == Size.Small)
            numWormholes = 2;
        else if (game.getSize() == Size.Medium)
            numWormholes = 3;
        else if (game.getSize() == Size.Large)
            numWormholes = 4;
        else if (game.getSize() == Size.Huge)
            numWormholes = 5;

        System.Random random = new System.Random();
        Dictionary<Vector2, bool> planetLocs = new Dictionary<Vector2, bool>();

        foreach (Planet planet in game.getPlanets())
            planetLocs.Add(new Vector2(planet.getX(), planet.getY()), true);

        for (int i = 0; i < numWormholes; i++)
        {
            Vector2 loc = new Vector2(random.Next(width), random.Next(height));
            
            while (!isValidLocation(loc, planetLocs, Consts.planetMinDistance))
            {
                loc = new Vector2(random.Next(width), random.Next(height));
            }


            GameObject go = GameObject.Instantiate(baseWormhole, planetParent.transform);
            Wormhole wormhole = new Wormhole("Wormhole #" + (i + 1) + "a", (int)loc.x, (int)loc.y);
            go.GetComponent<WormholeGameObject>().setWormhole(wormhole);
            game.addWormholes(go.GetComponent<WormholeGameObject>().getWormhole());
            go.transform.localPosition = new Vector3(go.GetComponent<WormholeGameObject>().getWormhole().getX() - width / 2, go.GetComponent<WormholeGameObject>().getWormhole().getY() - height / 2);
            go.name = wormhole.getName();
            go.SetActive(true);


            Debug.Log("Wormhole: " + wormhole.getName());
        }

    }

    /**
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
}

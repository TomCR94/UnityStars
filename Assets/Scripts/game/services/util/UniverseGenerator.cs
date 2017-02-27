using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseGenerator: MonoBehaviour {

    public GameObject basePlanet, planetParent;
    public GameObject baseFleet;


    private Game game;

    private FleetController fleetController;
    private PlanetController planetController;
    private ShipDesigner shipDesigner;
    private TechStore techStore;

    public UniverseGenerator(Game game, FleetController fleetController, PlanetController planetController, ShipDesigner shipDesigner, TechStore techStore)
    {
        this.game = game;
        this.fleetController = fleetController;
        this.planetController = planetController;
        this.shipDesigner = shipDesigner;
        this.techStore = techStore;
    }

    public void setUniverseGenerator(Game game, FleetController fleetController, PlanetController planetController, ShipDesigner shipDesigner, TechStore techStore)
    {
        this.game = game;
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
        generatePlanets(game);
        game.setYear(Consts.startingYear);

        foreach (Player player in game.getPlayers())
        {
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
                Fleet fleet = fleetController.create("Fleet #" + player.getNumFleetsBuilt(), player.getHomeworld().getX(), player.getHomeworld().getY(), player);
                fleet.addShipStack(design, 1);
                fleet.addWaypoint(player.getHomeworld().getX(), player.getHomeworld().getY(), 5, WaypointTask.None, player.getHomeworld());
                fleet.computeAggregate();

                GameObject go = GameObject.Instantiate(baseFleet, player.getHomeworld().transform);
                go.transform.position = Vector3.zero;
                go.GetComponent<Fleet>().CloneFrom(fleet);
                fleet = go.GetComponent<Fleet>();
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
                game.getFleets().Add(fleet);
                player.setNumFleetsBuilt(player.getNumFleetsBuilt() + 1);
            }

        }

        TurnGenerator tg = new TurnGenerator(game, fleetController, planetController);
        // scan everything!
        tg.scan(game);

        // do turn processing
        tg.processTurns(game);

        game.setStatus(GameStatus.WaitingForSubmit);
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

            // make sure this location is ok
            while (!isValidLocation(loc, planetLocs, Consts.planetMinDistance))
            {
                loc = new Vector2(random.Next(width), random.Next(height));
            }

            // add a new planet
            planetLocs.Add(loc, true);


            GameObject go = GameObject.Instantiate(basePlanet,
                planetParent.transform);
            Planet planet = planetController.create(ng.nextName(), (int)loc.x, (int)loc.y);
            go.GetComponent<Planet>().clone(planet);
            planetController.randomize(go.GetComponent<Planet>());
            game.getPlanets().Add(go.GetComponent<Planet>());
            go.transform.localPosition = new Vector3(go.GetComponent<Planet>().getX() - width/2, go.GetComponent<Planet>().getY() - height/2);
            go.name = planet.getName();
            go.SetActive(true);


           // Debug.Log("Planet: " + planet.getName());
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
}

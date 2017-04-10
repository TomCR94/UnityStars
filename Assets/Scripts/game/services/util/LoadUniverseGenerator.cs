using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
/*
 * Utility class used to load a universe with the given game, controller and tech store configuration
 */
public class LoadUniverseGenerator : MonoBehaviour
{

    public GameObject basePlanet, baseWormhole, planetParent;
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
     * Generate the universe
     */
    public void generate()
    {
        generateGame();


        planetParent.GetComponent<RectTransform>().sizeDelta = new Vector2(game.getGame().getWidth() + 40, game.getGame().getHeight() + 40);

        generatePlanets(game.getGame());
        generateWormholes(game.getGame());
        generateOrbitingFleets(game.getGame());
    }
    /**
     * Generate the game from the given save
     */
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
            getShipDesigns();
            getPlayerTechs();
        }
    }
    /**
     * Generate the ship designs from the given save
     */
    private void getShipDesigns()
    {
        DirectoryInfo dirInf = new DirectoryInfo(GameLocation + "/ShipDesigns/");
        Debug.Log(dirInf.ToString());
        if (dirInf.Exists)
        {
            FileInfo[] files = dirInf.GetFiles("*.design");
            foreach (FileInfo file in files)
            {
                Debug.Log(file.FullName);
                ShipDesign design = JsonUtility.FromJson<ShipDesign>(File.ReadAllText(file.FullName));

                foreach (Player player in game.getGame().getPlayers())
                {
                    if (player.getDesignIDs().Contains(design.getID()))
                    {
                        Debug.Log("Giving " + player.getName() + " design of " + design.getID());
                        player.getDesigns().Add(design);
                    }
                }
            }
        }
    }
    /**
     * Generate the playertech from the given save
     */
    private void getPlayerTechs()
    {
        DirectoryInfo dirInf = new DirectoryInfo(GameLocation + "/Techs/");
        Debug.Log(dirInf.ToString());
        if (dirInf.Exists)
        {
            FileInfo[] files = dirInf.GetFiles("*.techs");
            foreach (FileInfo file in files)
            {
                Debug.Log(file.FullName);
                PlayerTechs techs = JsonUtility.FromJson<PlayerTechs>(File.ReadAllText(file.FullName));

                foreach (Player player in game.getGame().getPlayers())
                {
                    if (file.Name.Contains(player.getName()))
                    {
                        Debug.Log("Giving " + player.getName() + " techs");
                        player.setTechs(techs);
                    }
                }
            }
        }
    }

    /**
     * Generate the planets from the given save
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

    /**
     * Generate the Wormholes from the given save
     */
    private void generateWormholes(Game game)
    {
        DirectoryInfo dirInf = new DirectoryInfo(GameLocation + "/Wormholes/");
        Debug.Log(dirInf.ToString());
        if (dirInf.Exists)
        {
            FileInfo[] files = dirInf.GetFiles("*.wormhole");
            foreach (FileInfo file in files)
            {
                Wormhole wormhole = JsonUtility.FromJson<Wormhole>(File.ReadAllText(file.FullName));
                
                GameObject go = GameObject.Instantiate(baseWormhole, planetParent.transform);
                go.GetComponent<WormholeGameObject>().setWormhole(wormhole);
                
                game.addWormholes(go.GetComponent<WormholeGameObject>().getWormhole());
                go.transform.localPosition = new Vector3(go.GetComponent<WormholeGameObject>().getWormhole().getX() - game.getWidth() / 2, go.GetComponent<WormholeGameObject>().getWormhole().getY() - game.getHeight() / 2);
                go.name = wormhole.getName();
                go.SetActive(true);
            }
        }

    }
    /**
     * Generate the Fleets from the given save
     */
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

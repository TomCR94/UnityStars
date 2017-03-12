using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameGameObject game;
    public GameObject baseFleet;
    public Transform mapObject;
    FleetController fc = new FleetControllerImpl();
    PlanetController pc;
    ShipDesigner sd = new ShipDesignerImpl();

    public bool load = false;

    // Use this for initialization

    private void Awake()
    {
        if (FindObjectOfType<StaticTechStore>() == null)
        {
            GameObject go = new GameObject("TechStore", typeof(StaticTechStore));
        }
        if (NewGameInit.instance != null)
        {
            load = NewGameInit.instance.load;
        }
    }

    void Start () {
        pc = new PlanetControllerImpl(fc, mapObject, game, baseFleet);
        if (!load)
        {
            UniverseGenerator gen = GetComponent<UniverseGenerator>();

            if (NewGameInit.instance != null)
            {
                game.getGame().setSize(NewGameInit.instance.size);
                game.getGame().setDensity(NewGameInit.instance.density);

                game.getGame().setName(NewGameInit.instance.gameName);

                game.getGame().getPlayers()[0].setRace(NewGameInit.instance.races[0]);
                game.getGame().getPlayers()[1].setRace(NewGameInit.instance.races[1]);

            }
            gen.setUniverseGenerator(game.getGame(), fc, pc, new ShipDesignerImpl(), StaticTechStore.getInstance());

            gen.generate();
        }
        else
        {
            LoadUniverseGenerator loadGen = GetComponent<LoadUniverseGenerator>();

            if (NewGameInit.instance != null)
            {
                game.getGame().setSize(NewGameInit.instance.size);
                game.getGame().setDensity(NewGameInit.instance.density);

                game.getGame().getPlayers()[0].setRace(NewGameInit.instance.races[0]);
                game.getGame().getPlayers()[1].setRace(NewGameInit.instance.races[1]);

                loadGen.setUniverseGenerator(Application.persistentDataPath + "/Game/" + NewGameInit.instance.gameName, game.getGame(), fc, pc, new ShipDesignerImpl(), StaticTechStore.getInstance());
            }
            else
                loadGen.setUniverseGenerator(Application.persistentDataPath + "/Game/Game1/", game.getGame(), fc, pc, new ShipDesignerImpl(), StaticTechStore.getInstance());

            loadGen.generate();
        }
        Settings.instance.playerID = game.getGame().getPlayers()[0].getID();
    }

    public void processTurn()
    {
        TurnGenerator tg = new TurnGenerator(game.getGame(), fc, pc);
        // do turn processing
        tg.generate();
    }

    public void writeGameToJson()
    {
        SaveGame();
        foreach (Planet planet in game.getGame().getPlanets())
            SaveGamePlanet(planet);
        foreach (Fleet fleet in game.getGame().getFleets())
            SaveGameFleet(fleet);
    }

    public void SaveGame()
    {
        Debug.Log(JsonUtility.ToJson(game.getGame(), true));

        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Game/"+game.getGame().getName()+"/");
        if (!dirInf.Exists)
        {
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }
        else
        {
            dirInf.Delete(true);
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }

        File.WriteAllText(Application.persistentDataPath + "/Game/" + game.getGame().getName() + "/" + game.getGame().getName() + ".json", JsonUtility.ToJson(game.getGame(), true));
    }

    public void SaveGamePlanet(Planet planet)
    {
        Debug.Log(JsonUtility.ToJson(planet, true));

        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Game/" + game.getGame().getName() + "/Planets/");
        if (!dirInf.Exists)
        {
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }

        File.WriteAllText(Application.persistentDataPath + "/Game/" + game.getGame().getName() + "/Planets/" + planet.getName() + ".planet", JsonUtility.ToJson(planet, true));
    }

    public void SaveGameFleet(Fleet fleet)
    {
        Debug.Log(JsonUtility.ToJson(fleet, true));

        if (fleet.getOrbiting() != null)
        {
            DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Game/" + game.getGame().getName() + "/Fleets/" + fleet.getOrbiting().getName());
            if (!dirInf.Exists)
            {
                Debug.Log("Creating subdirectory");
                dirInf.Create();
            }

            File.WriteAllText(Application.persistentDataPath + "/Game/" + game.getGame().getName() + "/Fleets/" + fleet.getOrbiting().getName() + "/" + fleet.getName() + ".fleet", JsonUtility.ToJson(fleet, true));
        }
        else
        {
            DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Game/" + game.getGame().getName() + "/Fleets/Empty Space");
            if (!dirInf.Exists)
            {
                Debug.Log("Creating subdirectory");
                dirInf.Create();
            }

            File.WriteAllText(Application.persistentDataPath + "/Game/" + game.getGame().getName() + "/Fleets/Empty Space/" + fleet.getName() + "_" + fleet.getOwner().getName() + ".fleet", JsonUtility.ToJson(fleet, true));
        }
    }


}

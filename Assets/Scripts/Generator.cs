using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Generator : MonoBehaviour {

    public Game game;
    FleetController fc = new FleetControllerImpl();
    PlanetController pc;
    ShipDesigner sd = new ShipDesignerImpl();
    // Use this for initialization

    private void Awake()
    {
        if (FindObjectOfType<StaticTechStore>() == null)
        {
            GameObject go = new GameObject("TechStore", typeof(StaticTechStore));
        }
    }

    void Start () {
        pc = new PlanetControllerImpl(fc);
        UniverseGenerator gen = GetComponent<UniverseGenerator>();

        if (NewGameInit.instance != null)
        {
            game.setSize(NewGameInit.instance.size);
            game.setDensity(NewGameInit.instance.density);
        }
        gen.setUniverseGenerator(game, fc, pc, new ShipDesignerImpl(), StaticTechStore.getInstance());

        gen.generate();
	}

    public void processTurn()
    {
        TurnGenerator tg = new TurnGenerator(game, fc, pc);
        // do turn processing
        tg.generate();
    }

    public void writeGameToJson()
    {
        SaveGame();
        foreach (Planet planet in game.getPlanets())
            SaveGamePlanet(planet);
        foreach (Fleet fleet in game.getFleets())
            SaveGameFleet(fleet);
    }

    public void SaveGame()
    {
        Debug.Log(JsonUtility.ToJson(game, true));

        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Game/Game1/");
        if (!dirInf.Exists)
        {
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }

        File.WriteAllText(Application.persistentDataPath + "/Game/Game1/Game1.json", JsonUtility.ToJson(game, true));

        Debug.Log(Application.persistentDataPath);
    }

    public void SaveGamePlanet(Planet planet)
    {
        Debug.Log(JsonUtility.ToJson(planet, true));

        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Game/Game1/Planets/");
        if (!dirInf.Exists)
        {
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }

        File.WriteAllText(Application.persistentDataPath + "/Game/Game1/Planets/" + planet.getName() + ".planet", JsonUtility.ToJson(planet, true));

        Debug.Log(Application.persistentDataPath);
    }

    public void SaveGameFleet(Fleet fleet)
    {
        Debug.Log(JsonUtility.ToJson(fleet, true));

        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Game/Game1/Fleets/" + fleet.getOrbiting());
        if (!dirInf.Exists)
        {
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }

        File.WriteAllText(Application.persistentDataPath + "/Game/Game1/Fleets/" + fleet.getOrbiting() + "/"+ fleet.getName() + ".fleet", JsonUtility.ToJson(fleet, true));

        Debug.Log(Application.persistentDataPath);
    }


}

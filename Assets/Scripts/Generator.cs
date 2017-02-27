using System.Collections;
using System.Collections.Generic;
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
        // scan everything!
        tg.scan(game);

        // do turn processing
        tg.generate();
    }
	
}

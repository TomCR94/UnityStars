using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetPopulator : MonoBehaviour {

    public Game game;
    public List<MapObject> objects = new List<MapObject>();
    public List<string> objectNames = new List<string>();
	// Use this for initialization
	void Start () {

        foreach (Planet p in game.getPlanets())
        {
            objects.Add(p);
            objectNames.Add(p.getName());
        }
        foreach (Fleet f in game.getFleets())
        {
            objects.Add(f);
            objectNames.Add(f.getName());
        }

        Dropdown d = GetComponent<Dropdown>();

        d.ClearOptions();

        foreach (MapObject mo in objects)
        {
            d.AddOptions(objectNames);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetPopulator : MonoBehaviour {


    public GameObject planetParent;
    public GameObject planetSource;

    int count = 0;
    Game game;
	// Use this for initialization
    /*
	void Start () {
        game = GetComponent<Game>();
	}
	
	// Update is called once per frame
	void Update () {
        if (count != game.getPlanets().Count)
        {
            count = game.getPlanets().Count;


            foreach (Planet planet in game.getPlanets())
            {
                GameObject go = GameObject.Instantiate(planetSource, planetParent.transform);
                go.GetComponent<Planet>().clone(planet);
                go.transform.localPosition= new Vector3(planet.getX(), planet.getY());
                go.SetActive(true);
            }

        }
	}*/
}

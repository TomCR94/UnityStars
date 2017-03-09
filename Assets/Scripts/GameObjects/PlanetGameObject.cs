using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetGameObject : MonoBehaviour {

    public GameGameObject game;

    [SerializeField]
    public Planet planet;

    private Color[] playerColors = { Color.green, Color.red };

    // Use this for initialization
    void Start () {
		
	}

    private void Update()
    {
        if (planet.getOwner() != null)
        {
            for (int i = 0; i < game.getGame().getPlayers().Count; i++)
            {
                if (planet.getOwnerID() == game.getGame().getPlayers()[i].getID())
                {
                    GetComponent<Image>().color = playerColors[i];
                    break;
                }
            }
        }
    }

    public Planet getPlanet()
    {
        return planet;
    }

    public void setPlanet(Planet planet)
    {
        planet.PlanetGameObject = this;
        this.planet = planet;
    }

    public string toJson()
    {
        return JsonUtility.ToJson(planet, true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetReport : MonoBehaviour {
    public int playerIndex;
    public GameGameObject game;
    public GameObject baseGO;
    // Use this for initialization
    private void Start()
    {
        init();
    }
    public void Clear()
    {
        foreach (Transform t in baseGO.transform.parent)
        {
            if (t.name != "Title")
                Destroy(t.gameObject);
        }
    }

    public void init()
    {
        Clear();
        foreach (PlanetKnowledge pk in game.getGame().getPlayers()[playerIndex].getPlanetKnowledges())
        {
            if (PlanetDictionary.instance.getPlanetForID(pk.getPlanetId()).getOwner() != null && PlanetDictionary.instance.getPlanetForID(pk.getPlanetId()).getOwner().getID() == game.getGame().getPlayers()[playerIndex].getID())
            {
                GameObject planetInfo = GameObject.Instantiate(baseGO, baseGO.transform.parent, false);
                planetInfo.transform.GetChild(0).GetComponentInChildren<Text>().text = pk.getPlanetId();
                planetInfo.transform.GetChild(1).GetComponentInChildren<Text>().text = PlanetDictionary.instance.getPlanetForID(pk.getPlanetId()).getPopulation().ToString();
                planetInfo.transform.GetChild(2).GetComponentInChildren<Text>().text = PlanetDictionary.instance.getPlanetForID(pk.getPlanetId()).getDefenses() + "%";
                planetInfo.transform.GetChild(3).GetComponentInChildren<Text>().text = PlanetDictionary.instance.getPlanetForID(pk.getPlanetId()).getResourcesPerYear().ToString();
                planetInfo.transform.GetChild(4).GetComponentInChildren<Text>().text = PlanetDictionary.instance.getPlanetForID(pk.getPlanetId()).getOrbitingFleets().Count.ToString();
                if (PlanetDictionary.instance.getPlanetForID(pk.getPlanetId()).getStarbase() != null)
                    planetInfo.transform.GetChild(5).GetComponentInChildren<Text>().text = PlanetDictionary.instance.getPlanetForID(pk.getPlanetId()).getStarbase().getName();
                else
                    planetInfo.transform.GetChild(5).GetComponentInChildren<Text>().text = "None";
                planetInfo.name = pk.getPlanetId();
            }
        }
	}
}

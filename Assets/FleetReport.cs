using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FleetReport : MonoBehaviour
{
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
        Debug.Log("FleetReport, FleetKnowledges: " + game.getGame().getPlayers()[playerIndex].getFleetKnowledges().Count);
        foreach (FleetKnowledge fk in game.getGame().getPlayers()[playerIndex].getFleetKnowledges())
        {
            Debug.Log("FleetOwnerID: " + FleetDictionary.instance.getFleetForID(fk.getFleetId()).getOwner().getID());
            Debug.Log("PlayerID: " + game.getGame().getPlayers()[playerIndex].getID());
            if (FleetDictionary.instance.getFleetForID(fk.getFleetId()).getOwner() != null && (FleetDictionary.instance.getFleetForID(fk.getFleetId()).getOwner().getID() == game.getGame().getPlayers()[playerIndex].getID()))
            {
                GameObject fleetInfo = GameObject.Instantiate(baseGO, baseGO.transform.parent, false);
                fleetInfo.transform.GetChild(0).GetComponentInChildren<Text>().text = FleetDictionary.instance.getFleetForID(fk.getFleetId()).getName();
                if (FleetDictionary.instance.getFleetForID(fk.getFleetId()).getOrbiting() != null)
                    fleetInfo.transform.GetChild(1).GetComponentInChildren<Text>().text = FleetDictionary.instance.getFleetForID(fk.getFleetId()).getOrbiting().getName();
                else
                    fleetInfo.transform.GetChild(1).GetComponentInChildren<Text>().text = "None";
                if (FleetDictionary.instance.getFleetForID(fk.getFleetId()).getWaypoints().Count > 1)
                {
                    fleetInfo.transform.GetChild(2).GetComponentInChildren<Text>().text = FleetDictionary.instance.getFleetForID(fk.getFleetId()).getWaypoints()[0].getTask().ToString();
                    fleetInfo.transform.GetChild(3).GetComponentInChildren<Text>().text = FleetDictionary.instance.getFleetForID(fk.getFleetId()).getWaypoints()[0].getTarget().getName();
                }
                else
                    fleetInfo.transform.GetChild(2).GetComponentInChildren<Text>().text = fleetInfo.transform.GetChild(3).GetComponentInChildren<Text>().text = "None";

                fleetInfo.name = fk.getFleetId();
            }
        }
    }
}

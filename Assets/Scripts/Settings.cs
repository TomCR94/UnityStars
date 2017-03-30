using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

    public static Settings instance;
    public string playerID;
    public bool setTarget = false;
    public FleetManager fleetManager;
    public Fleet selectedFleet;
    public Planet selectedPlanet;
    public Wormhole selectedWormhole;

    // Use this for initialization
    void Awake () {
        instance = this;
        selectedFleet = null;
        selectedPlanet = null;
    }
	

    public void SetNoSelected()
    {
        if (selectedFleet != null)
        {
            selectedFleet = null;
        }
        if (selectedPlanet != null)
        {
            selectedPlanet = null;
        }
    }

    public void setSelected(GameObject go)
    {
        if (go.GetComponent<PlanetGameObject>() != null)
            SetSelected(go.GetComponent<PlanetGameObject>().getPlanet());

        if (go.GetComponent<FleetGameObject>() != null)
            SetSelected(go.GetComponent<FleetGameObject>().getFleet());

        if (go.GetComponent<WormholeGameObject>() != null)
            SetSelected(go.GetComponent<WormholeGameObject>().getWormhole());
    }

    public void SetSelected(Planet obj)
    {
        if(setTarget)
        {
            fleetManager.target = obj;
            fleetManager.setButtonText(obj.getName());
            setTarget = false;
            return;
        }
        selectedPlanet = obj;
    }

    public void SetSelected(Wormhole obj)
    {
        if (setTarget)
        {
            fleetManager.target = obj;
            fleetManager.setButtonText(obj.getName());
            setTarget = false;
            return;
        }
        selectedWormhole = obj;
    }

    public void SetSelected(Fleet obj)
    {
        if (setTarget)
        {
            fleetManager.target = obj;
            fleetManager.setButtonText(obj.getName());
            setTarget = false;
            return;
        }
        selectedFleet = obj;

    }

    public void setTargetOn()
    {
        setTarget = !setTarget;
    }
}

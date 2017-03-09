using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

    public static Settings instance;
    public bool setTarget = false;
    public FleetManager fleetManager;
    public Fleet selectedFleet;
    public Planet selectedPlanet;

	// Use this for initialization
	void Start () {
        instance = this;
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
    }

    public void SetSelected(Planet obj)
    {
        if (!setTarget)
            selectedPlanet = obj;
        else
        {
            fleetManager.target = obj;
            fleetManager.setButtonText(obj.getName());
        }

        setTarget = false;
    }

    public void SetSelected(Fleet obj)
    {
        if (!setTarget)
            selectedFleet = obj;
        else
        {
            fleetManager.target = obj;
            fleetManager.setButtonText(obj.getName());
        }

        setTarget = false;
    }

    public void setTargetOn()
    {
        setTarget = !setTarget;
    }
}

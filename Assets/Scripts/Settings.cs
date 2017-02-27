using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

    public static Settings instance;
    public bool setTarget = false;
    public FleetManager fleetManager;
    public GameObject selected;

	// Use this for initialization
	void Start () {
        instance = this;
	}
	

    public void SetNoSelected()
    {
        if (selected != null)
        {
            selected = null;
        }
    }

    public void SetSelected(GameObject obj)
    {
        if (!setTarget)
            selected = obj;
        else
        {
            fleetManager.target = obj.GetComponent<MapObject>();
            fleetManager.setButtonText(obj.GetComponent<MapObject>().getName());
        }

        setTarget = false;

    }

    public void setTargetOn()
    {
        setTarget = !setTarget;
    }
}

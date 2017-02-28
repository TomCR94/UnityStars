using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FleetsInOrbit : MonoBehaviour {


    public List<string> fleetNames = new List<string>();
    public Button gotoButton, cargoButton;
    public Dropdown fleets;
	// Use this for initialization
	void Start () {
        fleets.ClearOptions();
    }
	
	// Update is called once per frame
	void Update () {
		gotoButton.interactable = cargoButton.interactable = (Settings.instance.selected != null && Settings.instance.selected.GetComponent<Planet>() != null);
    }

    public void init()
    {

        if (Settings.instance.selected != null && Settings.instance.selected.GetComponent<Planet>() != null)
        {
            fleetNames.Clear();
            fleets.ClearOptions();

            Planet planet = Settings.instance.selected.GetComponent<Planet>();

            for (int i = 0; i < planet.transform.childCount; i++)
            {
                if (planet.transform.GetChild(i).GetComponent<Fleet>() != null)
                {
                    fleetNames.Add(planet.transform.GetChild(i).GetComponent<Fleet>().getName());
                }
            }
            fleets.AddOptions(fleetNames);
        }
    }

    public void Goto()
    {
        Settings.instance.selected = Settings.instance.selected.GetComponent<Planet>().transform.GetChild(fleets.value).gameObject;
    }

    public void Cargo()
    {
        CargoTransfer.instance.init(Settings.instance.selected.GetComponent<Planet>().transform.GetChild(fleets.value).GetComponent<Fleet>());
    }

}

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
		gotoButton.interactable = cargoButton.interactable = (fleets.options.Count > 0 && Settings.instance.selectedPlanet != null);
    }

    public void init()
    {

        if (Settings.instance.selectedPlanet != null)
        {
            fleetNames.Clear();
            fleets.ClearOptions();
            
            foreach (Fleet fleet in Settings.instance.selectedPlanet.getOrbitingFleets())
            {
                fleetNames.Add(fleet.getName());
            }
            
            fleets.AddOptions(fleetNames);
        }
    }

    public void Goto()
    {
        Settings.instance.selectedFleet = Settings.instance.selectedPlanet.getOrbitingFleets()[fleets.value];
    }

    public void Cargo()
    {
        CargoTransfer.instance.init(Settings.instance.selectedPlanet.getOrbitingFleets()[fleets.value]);
    }

}

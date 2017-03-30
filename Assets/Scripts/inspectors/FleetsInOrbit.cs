using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FleetsInOrbit : MonoBehaviour {

    public GameGameObject game;
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
        fleetNames.Clear();
        fleets.ClearOptions();

        if (Settings.instance.selectedPlanet != null)
        {
            foreach (Fleet fleet in Settings.instance.selectedPlanet.getOrbitingFleets())
            {
                if(fleet.getOwner().getID() == game.getGame().getPlayers()[MessagePanel.instance.playerIndex].getID())
                fleetNames.Add(fleet.getName());
            }
            
            fleets.AddOptions(fleetNames);
            if (fleets.options.Count > 0)
            {
                fleets.value = 0;
                Goto();
            }
            else
            {
                Settings.instance.selectedFleet = null;
            }
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

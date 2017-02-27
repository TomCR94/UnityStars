using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CargoSummary : MonoBehaviour {

    public Cargo cargo;

    public Text Fuel;
    public Text Colonists;
    public Text ironium, boranium, germanium;


    private void Update()
    {
        if (Settings.instance.selected != null && Settings.instance.selected.GetComponent<Cargo>() != null)
        {
            cargo = Settings.instance.selected.GetComponent<Cargo>();

            Fuel.text = "Fuel:" + cargo.getFuel();
            Colonists.text = "Colonists: " + cargo.getColonists();
            ironium.text = "Ironium: " + cargo.getIronium().ToString();
            boranium.text = "Boranium: " + cargo.getBoranium().ToString();
            germanium.text = "Germanium: " + cargo.getGermanium().ToString();
        }
        else
        {
            Fuel.text = "Fuel:";
            Colonists.text = "Colonists: ";
            ironium.text = "Ironium: ";
            boranium.text = "Boranium: ";
            germanium.text = "Germanium: ";
        }
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CargoTransfer : MonoBehaviour {

    public static CargoTransfer instance;

    Fleet SelectedFleet;
    public Text planet, fleet;

    [Space]
    //FleetValues
    [Header("Fleet Values")]
    public Text FleetIronium;
    public Text FleetBoranium;
    public Text FleetGermanium;
    public Text FleetColonists;
    [Space]
    //PlanetValues
    [Header("Planet Values")]
    public Text PlanetIronium;
    public Text PlanetBoranium;
    public Text PlanetGermanium;
    public Text PlanetColonists;

    // Use this for initialization
    void Start () {
        instance = this;
	}
	
    public void init(Fleet selectedFleet)
    {
        SelectedFleet = selectedFleet;
        planet.text = selectedFleet.getOrbiting().getName();
        fleet.text = selectedFleet.getName();

        FleetIronium.text = selectedFleet.getCargo().getIronium() + "kT";
        FleetBoranium.text = selectedFleet.getCargo().getBoranium() + "kT";
        FleetGermanium.text = selectedFleet.getCargo().getGermanium() + "kT";
        FleetColonists.text = selectedFleet.getCargo().getColonists() + "kT";

        PlanetIronium.text = selectedFleet.getOrbiting().getCargo().getIronium() + "kT";
        PlanetBoranium.text = selectedFleet.getOrbiting().getCargo().getBoranium() + "kT";
        PlanetGermanium.text = selectedFleet.getOrbiting().getCargo().getGermanium() + "kT";
        PlanetColonists.text = selectedFleet.getOrbiting().getCargo().getColonists() + "kT";
    }

    //From Ship to Planet
    public void toLeft(int index)
    {
        switch (index)
        {
            case 0:
                if (SelectedFleet.getCargo().getIronium() >= 100)
                {
                    SelectedFleet.getCargo().addIronium(-100);
                    SelectedFleet.getOrbiting().getCargo().addIronium(100);

                    FleetIronium.text = SelectedFleet.getCargo().getIronium() + "kT";
                    PlanetIronium.text = SelectedFleet.getOrbiting().getCargo().getIronium() + "kT";
                }
                break;
            case 1:
                if (SelectedFleet.getCargo().getBoranium() >= 100)
                {
                    SelectedFleet.getCargo().addBoranium(-100);
                    SelectedFleet.getOrbiting().getCargo().addBoranium(100);

                    FleetBoranium.text = SelectedFleet.getCargo().getBoranium() + "kT";
                    PlanetBoranium.text = SelectedFleet.getOrbiting().getCargo().getBoranium() + "kT";
                }
                break;
            case 2:
                if (SelectedFleet.getCargo().getGermanium() >= 100)
                {
                    SelectedFleet.getCargo().addGermanium(-100);
                    SelectedFleet.getOrbiting().getCargo().addGermanium(100);

                    FleetGermanium.text = SelectedFleet.getCargo().getGermanium() + "kT";
                    PlanetGermanium.text = SelectedFleet.getOrbiting().getCargo().getGermanium() + "kT";
                }
                break;
            case 3:
                if (SelectedFleet.getCargo().getColonists() >= 100)
                {
                    SelectedFleet.getCargo().addColonists(-100);
                    SelectedFleet.getOrbiting().getCargo().addColonists(100);

                    FleetColonists.text = SelectedFleet.getCargo().getColonists() + "kT";
                    PlanetColonists.text = SelectedFleet.getOrbiting().getCargo().getColonists() + "kT";
                }
                break;
            default:
                break;
        }
    }

    //From Planet to Ship
    public void toRight(int index)
    {
        switch (index)
        {
            case 0:
                if (SelectedFleet.getOrbiting().getCargo().getIronium() >= 100)
                {
                    SelectedFleet.getCargo().addIronium(100);
                    SelectedFleet.getOrbiting().getCargo().addIronium(-100);

                    FleetIronium.text = SelectedFleet.getCargo().getIronium() + "kT";
                    PlanetIronium.text = SelectedFleet.getOrbiting().getCargo().getIronium() + "kT";
                }
                break;
            case 1:
                if (SelectedFleet.getOrbiting().getCargo().getBoranium() >= 100)
                {
                    SelectedFleet.getCargo().addBoranium(100);
                    SelectedFleet.getOrbiting().getCargo().addBoranium(-100);

                    FleetBoranium.text = SelectedFleet.getCargo().getBoranium() + "kT";
                    PlanetBoranium.text = SelectedFleet.getOrbiting().getCargo().getBoranium() + "kT";
                }
                break;
            case 2:
                if (SelectedFleet.getOrbiting().getCargo().getGermanium() >= 100)
                {
                    SelectedFleet.getCargo().addGermanium(100);
                    SelectedFleet.getOrbiting().getCargo().addGermanium(-100);

                    FleetGermanium.text = SelectedFleet.getCargo().getGermanium() + "kT";
                    PlanetGermanium.text = SelectedFleet.getOrbiting().getCargo().getGermanium() + "kT";
                }
                break;
            case 3:
                if (SelectedFleet.getOrbiting().getCargo().getColonists() >= 100)
                {
                    SelectedFleet.getCargo().addColonists(100);
                    SelectedFleet.getOrbiting().getCargo().addColonists(-100);

                    FleetColonists.text = SelectedFleet.getCargo().getColonists() + "kT";
                    PlanetColonists.text = SelectedFleet.getOrbiting().getCargo().getColonists() + "kT";
                }
                break;
            default:
                break;
        }
    }
}

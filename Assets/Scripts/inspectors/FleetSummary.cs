using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FleetSummary : MonoBehaviour {

    public Fleet fleet;

    public Text name;
    public Text xy;
    public Text planet;

    private void Update()
    {
        if (Settings.instance.selectedFleet != null )
        {
            fleet = Settings.instance.selectedFleet;

            name.text = "Name:" + fleet.getName();
            xy.text = "XY: " + fleet.getX() + ", " + fleet.getY();
            if (fleet.getOrbiting() != null)
                planet.text = "Planet: " + fleet.getOrbiting().getName();
            else
                planet.text = "Planet: ";
        }
        else
        {
            name.text = "Name:";
            xy.text = "XY: ";
            planet.text = "Planet: ";
        }
    }

}

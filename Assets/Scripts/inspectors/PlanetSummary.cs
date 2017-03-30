using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlanetSummary : MonoBehaviour {
    public GameGameObject game;
    public Planet planet;

    public Text title;
    public Text population;
    public Text gravity, temperature, radiation;

    private void Update()
    {
        if (Settings.instance.selectedPlanet != null)
        {
            planet = Settings.instance.selectedPlanet;
            if (game.getGame().getPlayers()[MessagePanel.instance.playerIndex].hasKnowledge(planet))
            {
                title.text = planet.getName() + " Summary";
                population.text = "Population: " + planet.getPopulation();
                gravity.text = "Gravity: " + planet.getHab().getGrav();
                temperature.text = "Temperature: " + planet.getHab().getTemp();
                radiation.text = "Radiation: " + planet.getHab().getRad();
            }
            else
            {
                title.text = "Unknown Planet Summary";
                population.text = "Population: Unknown";
                gravity.text = "Gravity: Unknown";
                temperature.text = "Temperature: Unknown";
                radiation.text = "Radiation: Unknown";
            }
        }
        else
        {
            title.text = "Planet Summary";
            population.text = "Population: ";
            gravity.text = "Gravity: ";
            temperature.text = "Temperature: ";
            radiation.text = "Radiation: ";
        }
    }

}

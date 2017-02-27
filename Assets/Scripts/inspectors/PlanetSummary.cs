﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlanetSummary : MonoBehaviour {

    public Planet planet;

    public Text title;
    public Text population;
    public Text gravity, temperature, radiation;

    private void Update()
    {
        if (Settings.instance.selected != null && Settings.instance.selected.GetComponent<Planet>() != null)
        {
            planet = Settings.instance.selected.GetComponent<Planet>();

            title.text = planet.getName() + " Summary";
            population.text = "Population: " + planet.getPopulation();
            gravity.text = "Gravity: " + planet.getHab().getGrav();
            temperature.text = "Temperature: " + planet.getHab().getTemp();
            radiation.text = "Radiation: " + planet.getHab().getRad();
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
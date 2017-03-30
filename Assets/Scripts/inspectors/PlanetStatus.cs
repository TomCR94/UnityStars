using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlanetStatus : MonoBehaviour
{

    public GameGameObject game;
    public Planet planet;

    public Text title;
    public Text Population;
    public Text ResourcesYear;
    public Text ScannerType, ScannerRange, Defenses, DefenseType, DefCoverage;

    private void Update()
    {
        if (Settings.instance.selectedPlanet != null )
        {
            planet = Settings.instance.selectedPlanet;

            if (game.getGame().getPlayers()[MessagePanel.instance.playerIndex].hasKnowledge(planet))
            {
                title.text = "Minerals On Hand";
                Population.text = "Population: " + planet.getPopulation();
                ResourcesYear.text = "Resources/Year: " + planet.getResourcesPerYearAvailable();
                Defenses.text = "Defenses: " + planet.getDefenses() + " of " + planet.getMaxDefenses();
                DefenseType.text = "Defense Type: " + "Surface";
                DefCoverage.text = "Def Coverage: " + ((float)planet.getDefenses() / (float)planet.getMaxDefenses() * 100) + "%";
            }
            else
            {
                title.text = "Minerals On Hand";
                Population.text = "Population: Unknown";
                ResourcesYear.text = "Resources/Year: Unknown";
                Defenses.text = "Defenses: Unknown";
                DefenseType.text = "Defense Type: Unknown";
                DefCoverage.text = "Def Coverage: Unknown";
            }
        }
        else
        {
            title.text = "Minerals On Hand";
            Population.text = "Population: ";
            ResourcesYear.text = "Resources/Year: ";
            ScannerType.text = "";
            ScannerRange.text = "";
            Defenses.text = "Defenses: ";
            DefenseType.text = "Defense Type: ";
            DefCoverage.text = "Def Coverage: ";
        }
    }

}

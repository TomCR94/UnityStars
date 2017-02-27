using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlanetStatus : MonoBehaviour
{

    public Planet planet;

    public Text title;
    public Text Population;
    public Text ResourcesYear;
    public Text ScannerType, ScannerRange, Defenses, DefenseType, DefCoverage;

    private void Update()
    {
        if (Settings.instance.selected != null && Settings.instance.selected.GetComponent<Planet>() != null)
        {
            planet = Settings.instance.selected.GetComponent<Planet>();

            title.text = "Minerals On Hand";
            Population.text = "Population: " + planet.getPopulation();
            ResourcesYear.text = "Resources/Year: " + planet.getResourcesPerYearAvailable();
            //ScannerType.text = "Scanner Type: " + planet.getOwner()!= null?planet.getOwner().getTechs().getBestPlanetaryScanner().getName():"";
            //ScannerRange.text = "Scanner Range: " + planet.getOwner() != null ? planet.getOwner().getTechs().getBestPlanetaryScanner().getScanRange().ToString():"";
            Defenses.text = "Defenses: " + planet.getDefenses() + " of " + planet.getMaxDefenses();
            DefenseType.text = "Defense Type: " + "type";
            DefCoverage.text = "Def Coverage: " + "coverage";
        }
        else
        {
            title.text = "Minerals On Hand";
            Population.text = "Population: ";
            ResourcesYear.text = "Resources/Year: ";
            ScannerType.text = "Scanner Type: ";
            ScannerRange.text = "Scanner Range: ";
            Defenses.text = "Defenses: ";
            DefenseType.text = "Defense Type: ";
            DefCoverage.text = "Def Coverage: ";
        }
    }

}

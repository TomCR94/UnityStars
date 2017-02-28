﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
[ExecuteInEditMode]
public class RaceEditor : MonoBehaviour {
    public Race race;
    [Space]
    public InputField singleName, pluralName;
    public Dropdown spendRemainingPoints;
    public ToggleGroup predefinedRaceCharac;
    public ToggleGroup lrts;
    // Use this for initialization
    void Start () {
        race = new Race();
        race.setHumanoid();
        spendRemainingPoints.ClearOptions();
        List<string> list = new List<string>();
        list.AddRange(Enum.GetNames(typeof(SpendLeftoverPointsOn)));
        spendRemainingPoints.AddOptions(list);

        singleName.text = race.getName();
        pluralName.text = race.getPluralName();

        foreach (Toggle tg in predefinedRaceCharac.ActiveToggles())
        {
            if (tg.name == Enum.GetName(typeof(PRT), race.getPRT()))
                tg.isOn = true;
            else
                tg.isOn = false;
        }

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
        transform.GetChild(5).gameObject.SetActive(false);

    }

    public void setRaceName(string name)
    {
        race.setName(name);
    }
    public void setRacePluralName(string name)
    {
        race.setPluralName(name);
    }
    public void setRacePassword(string pass)
    {
    }

    public void setLeftoverPoints(int index)
    {
        race.setSpendLeftoverPointsOn((SpendLeftoverPointsOn)index);
    }
    public void setPrimaryRaceTrait(int index)
    {
        race.setPRT((PRT)index);
    }

    public void setLesserRaceTrait(int index)
    {
        if (race.getLrts().Contains((LRT)index))
            race.getLrts().Remove((LRT)index);
        else
            race.getLrts().Add((LRT)index);
    }

    public void SetResourcesGenPerColonist(int amount)
    {
        race.setColonistsPerResource(amount);
    }

    public void FactoriesProduce(int amount)
    {
        race.setFactoryOutput(amount);
    }

    public void FactoriesCost(int amount)
    {
        race.setFactoryCost(amount);
    }

    public void setNumFactories(int amount)
    {
        race.setNumFactories(amount);
    }

    public void FactoriesCostLess(bool costLess)
    {
        race.setFactoriesCostLess(costLess);
    }

    public void MinesProduce(int amount)
    {
        race.setMineOutput(amount);
    }

    public void MinesCost(int amount)
    {
        race.setMineCost(amount);
    }

    public void colonistsForMine(int amount)
    {
        race.setNumMines(amount);
    }

    public void setEnergyResearchLevel(int level)
    {
        race.getResearchCost().setEnergy((ResearchCostLevel)level);
    }

    public void setWeaponsResearchLevel(int level)
    {
        race.getResearchCost().setWeapons((ResearchCostLevel)level);
    }

    public void setPropulsionResearchLevel(int level)
    {
        race.getResearchCost().setPropulsion((ResearchCostLevel)level);
    }


    public void setConstructionResearchLevel(int level)
    {
        race.getResearchCost().setConstruction((ResearchCostLevel)level);
    }

    public void setElectronicsResearchLevel(int level)
    {
        race.getResearchCost().setElectronics((ResearchCostLevel)level);
    }

    public void setBiotechnologyResearchLevel(int level)
    {
        race.getResearchCost().setBiotechnology((ResearchCostLevel)level);
    }

    public void Finish()
    {
        Debug.Log(JsonUtility.ToJson(race, true));

        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Races/");
        if (!dirInf.Exists)
        {
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }

        File.WriteAllText(Application.persistentDataPath + "/Races/" + race.getName() + ".race", JsonUtility.ToJson(race, true));

        Debug.Log(Application.persistentDataPath);
    }
}
using System.Collections;
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
        race = new global::Race();
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

        foreach (Toggle tg in lrts.ActiveToggles())
        {
            if (race.getLrts().Count > 0)
                foreach (LRT lrt in race.getLrts())
                {

                    Debug.Log(tg.name);
                    Debug.Log(Enum.GetName(typeof(LRT), lrt));
                    if (tg.name == Enum.GetName(typeof(LRT), lrt))
                    {
                        tg.isOn = true;
                        break;
                    }
                    else
                        tg.isOn = false;
                }
            else
            {
                tg.isOn = false;
            }
        }

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
        transform.GetChild(5).gameObject.SetActive(false);

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

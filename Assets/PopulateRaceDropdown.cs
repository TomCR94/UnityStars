using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PopulateRaceDropdown : MonoBehaviour {

    public List<Race> races = new List<Race>();
    public List<string> raceStrings = new List<string>();

    //For multiplayer Only
    public InviteCreator inviteCreator;

    public inviteAccepter inviteAccept;

    // Use this for initialization
    void Start () {
        getValues();
    }

    private void Update()
    {
        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Races/");
        if (dirInf.Exists)
        {
            FileInfo[] files = dirInf.GetFiles("*.race");
            if (files.Length != races.Count)
                getValues();
        }
    }

    void getValues()
    {
        races.Clear();
        raceStrings.Clear();
        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Races/");
        if (dirInf.Exists)
        {
            FileInfo[] files = dirInf.GetFiles("*.race");
            foreach (FileInfo file in files)
            {
                Race race = JsonUtility.FromJson<Race>(File.ReadAllText(Application.persistentDataPath + "/Races/" + file.Name));
                if (race.getName() != "Humanoid")
                {
                    races.Add(race);
                    raceStrings.Add(race.getName());
                }
            }
        }
        races.Insert(0, Race.getHumanoid());
        raceStrings.Insert(0, "Humanoid");

        GetComponent<Dropdown>().ClearOptions();
        GetComponent<Dropdown>().AddOptions(raceStrings);
    }

    public void setPlayerRace(int playerNumber)
    {
        NewGameInit.instance.setPlayerRace(races[GetComponent<Dropdown>().value], playerNumber);
    }

    public void setInviteRace(int raceIndex)
    {
        inviteCreator.setRace(races[GetComponent<Dropdown>().value]);
    }

    public void setAcceptRace(int raceIndex)
    {
        inviteAccept.setRace(races[GetComponent<Dropdown>().value]);
    }
}

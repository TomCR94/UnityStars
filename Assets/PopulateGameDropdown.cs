using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGameDropdown : MonoBehaviour {
    
    public List<string> gameStrings = new List<string>();
    public Button finish;
    // Use this for initialization
    void Start()
    {
        getValues();
    }

    private void Update()
    {
        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Game/");
        if (dirInf.Exists)
        {
            DirectoryInfo[] dirs = dirInf.GetDirectories();
            if (dirs.Length != gameStrings.Count)
                getValues();
        }

        GetComponent<Dropdown>().interactable = finish.interactable = dirInf.GetDirectories().Length > 0;
    }

    void getValues()
    {
        gameStrings.Clear();
        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Game/");
        if (dirInf.Exists)
        {
            DirectoryInfo[] dirs = dirInf.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                gameStrings.Add(dir.Name);
            }
        }
        GetComponent<Dropdown>().ClearOptions();
        GetComponent<Dropdown>().AddOptions(gameStrings);
        NewGameInit.instance.setGameName(gameStrings[0]);
    }

    public void setGameName(int playerNumber)
    {
        Debug.Log(playerNumber + ":" + gameStrings[playerNumber]);
        NewGameInit.instance.setGameName(gameStrings[playerNumber]);
    }
}

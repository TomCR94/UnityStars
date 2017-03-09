using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class NewGameInit : MonoBehaviour {

    public static NewGameInit instance;

    [SerializeField]
    public Size size = Size.Tiny;
    [SerializeField]
    public Density density = Density.Sparse;

    public List<Race> races = new List<Race>();
    public List<string> playerNames = new List<string>();
    public bool load;
    public string gameName;

    private void Awake()
    {
        Race humanoid = new Race().setHumanoid();
        RaceEditor.writeToRaceFile(humanoid);
        setPlayerRace(humanoid, 0);
        setPlayerRace(humanoid, 1);
        if (FindObjectOfType<StaticTechStore>() == null)
        {
            GameObject go = new GameObject("TechStore", typeof(StaticTechStore));
        }
    }

    // Use this for initialization
    void Start () {
        instance = this;
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void loadOnlineGame()
    {
        SceneManager.LoadScene(2);
    }

    public void setSize(int index)
    {
        size = (Size)Enum.GetValues(typeof(Size)).GetValue(index);
    }

    public void setDensity(int index)
    {
        density = (Density)Enum.GetValues(typeof(Density)).GetValue(index);
    }

    public void setPlayerRace(Race race, int playerNumber)
    {
        if (races.Count > playerNumber)
            races.RemoveAt(playerNumber);
        races.Insert(playerNumber, race);
    }

    public void setPlayerName(string name, int playerNumber)
    {
        if (playerNames.Count > playerNumber)
            playerNames.RemoveAt(playerNumber);
        playerNames.Insert(playerNumber, name);
    }

    public void setGameName(string name)
    {
        gameName = name;
    }

    public void setLoading(bool load)
    {
        this.load = load;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGameObject : MonoBehaviour {

    [SerializeField]
    public Game game;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Game getGame()
    {
        return game;
    }

    public void setGame(Game game)
    {
        this.game = game;
    }

    public string toJson()
    {
        return JsonUtility.ToJson(game, true);
    }
}

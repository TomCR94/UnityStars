using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGameObject : MonoBehaviour {

    public static GameGameObject instance;

    [SerializeField]
    public Game game;

    // Use this for initialization
    private void Awake()
    {
        instance = this;
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

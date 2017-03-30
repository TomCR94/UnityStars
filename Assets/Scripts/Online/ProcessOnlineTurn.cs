using GameSparks.Api.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessOnlineTurn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void processTurn()
    {

    }

    void saveTurn()
    {

    }

    void zipTurn()
    {

    }

    void takeTurn()
    {
        new LogChallengeEventRequest()
            .SetChallengeInstanceId(GameSparksManager.getInstance().getChallengeID())
            .SetEventKey("TT")
            .SetEventAttribute("GD", "set from c#: " + GameSparksManager.getInstance().getPlayerName())
            .Send((response) =>
            {
                if (response.HasErrors)
                {
                    Debug.Log("Failed");
                }
                else
                {
                    Debug.Log("Successful");
                }
            });
    }

}

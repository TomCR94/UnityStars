using GameSparks.Api.Requests;
using GameSparks.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateFriends : MonoBehaviour {

    public Button refresh;
    private System.Object thisLock = new System.Object();

    // Use this for initialization
    void Start () {
        if(GS.Authenticated)
            GetFriends();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clear()
    {
        //Every time we call GetChallengeInvites we'll refresh the list
        for (int i = 1; i < transform.childCount; i++)
        {
            //Destroy all gameInvite gameObjects currently in the scene
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void GetFriends()
    {
        lock (thisLock)
        {
            refresh.interactable = false;
            new LogEventRequest_getOnlinePlayers()
                .Send((response) =>
                {
                    Clear();
                    foreach (GSData player in response.ScriptData.GetGSDataList("players"))
                        if (player.GetString("id") != GameSparksManager.getInstance().getPlayerID())
                        {
                            GameObject go = GameObject.Instantiate(transform.GetChild(0).gameObject, transform);
                            go.GetComponent<Friend>().friendName = player.GetString("displayName");
                            go.GetComponent<Friend>().userID = player.GetString("id");
                            go.SetActive(true);
                        }
                    refresh.interactable = true;
                });
        }
    }
}

using GameSparks.Api.Requests;
using GameSparks.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateInvites : MonoBehaviour {

    public Button refresh;
    private System.Object thisLock = new System.Object();

    // Use this for initialization
    void Start () {
        if(GS.Authenticated)
            GetChallengeInvites();
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

    public void GetChallengeInvites()
    {
        lock (thisLock)
        {
            refresh.interactable = false;
            //We send a ListChallenge Request with the shortcode of our challenge, we set this in our GameSparks Portal
            new ListChallengeRequest().SetShortCode("MP")
                    .SetState("RECEIVED") //We only want to get games that we've received
                    .SetEntryCount(50) //We want to pull in the first 50 we find
                    .Send((response) =>
                    {
                        Clear();
                        //For every challenge we get
                        foreach (var challenge in response.ChallengeInstances)
                        {
                        //Create a new gameObject, add invitePrefab as a child of the invite Grid GameObject
                        GameObject go = GameObject.Instantiate(transform.GetChild(0).gameObject, transform);

                        //Update all the gameObject's variables
                        go.GetComponent<GameInvite>().challengeId = challenge.ChallengeId;
                            go.GetComponent<GameInvite>().inviteName = challenge.Challenger.Name;
                            go.GetComponent<GameInvite>().inviteExpiry = challenge.EndDate.ToString();
                            go.GetComponent<GameInvite>().uniDensity = challenge.ScriptData.GetString("d");
                            go.GetComponent<GameInvite>().uniSize = challenge.ScriptData.GetString("s");
                            go.GetComponent<GameInvite>().challengerRace = JsonUtility.FromJson<Race>(challenge.ScriptData.GetString("cR1"));
                            go.SetActive(true);
                        }
                        refresh.interactable = true;
                    });
        }
    }

}

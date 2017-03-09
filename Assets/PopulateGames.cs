using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateGames : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        GetChallengeAccepted();
        InvokeRepeating("GetChallengeAccepted", 15, 15);

    }

    // Update is called once per frame
    void Update()
    {

    }

    List<string> states = new List<string> { "RUNNING", "WAITING"};

    public void Clear()
    {
        //Every time we call GetChallengeInvites we'll refresh the list
        for (int i = 1; i < transform.childCount; i++)
        {
            //Destroy all gameInvite gameObjects currently in the scene
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void GetChallengeAccepted()
    {
        Clear();
        //We send a ListChallenge Request with the shortcode of our challenge, we set this in our GameSparks Portal
        new ListChallengeRequest().SetShortCode("MP")
                .SetStates(states)
                .SetEntryCount(50) //We want to pull in the first 50 we find
                .Send((response) =>
                {
                    //For every challenge we get
                    foreach (var challenge in response.ChallengeInstances)
                    {
                        //Create a new gameObject, add invitePrefab as a child of the invite Grid GameObject
                        GameObject go = GameObject.Instantiate(transform.GetChild(0).gameObject, transform);

                        

                        //Update all the gameObject's variables
                        go.GetComponent<ChallengeGame>().challengeId = challenge.ChallengeId;
                        foreach (GameSparks.Api.Responses.ListChallengeResponse._Challenge._PlayerDetail playerDetails in challenge.Challenged)
                        {
                            go.GetComponent<ChallengeGame>().challenged = playerDetails.Name;
                        }
                        go.GetComponent<ChallengeGame>().challenger = challenge.Challenger.Name;
                        go.SetActive(true);
                    }
                });
    }

}

using GameSparks.Api.Requests;
using GameSparks.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InviteCreator : MonoBehaviour {
    [SerializeField]
    string playerID;

    [SerializeField]
    int density, size;
    [SerializeField]
    string race;

    private void Start()
    {
        size = 0;
        density = 0;
        race = JsonUtility.ToJson(Race.getHumanoid(), true);
    }

    public void setPlayerID(string id)
    {
        playerID = id;
    }

    public void setDensity(int i)
    {
        density = i;
    }

    public void setSize(int i)
    {
        size = i;
    }

    public void SendInvite()
    {
        Debug.Log("PlayerID: " + playerID + "\nDensity: " + density + "\nSize: " + size + "\nRace: " + race);
        List<string> userIDs = new List<string>();
        userIDs.Add(playerID);
        ChallengeUser(userIDs, size.ToString(), density.ToString(), race);
    }

    public void setRace(Race race)
    {
        this.race = JsonUtility.ToJson(race);
    }

    public void ChallengeUser(List<string> userIds, string size, string density, string raceJson)
    {

        Debug.Log(userIds[0]);
        //we use CreateChallengeRequest with the shortcode of our challenge, we set this in our GameSparks Portal
        new CreateChallengeRequest().SetChallengeShortCode("MP")
                .SetEndTime(DateTime.Today.AddDays(1))
                .SetUsersToChallenge(userIds) //We supply the userIds of who we wish to challenge
                .SetChallengeMessage("I've challenged you to Stars!") // We can send a message along with the invite
                .Send((response) =>
                {
                    if (response.HasErrors)
                    {
                        Debug.Log(response.Errors.JSON.ToString());
                    }
                    else
                    {
                        Debug.Log("MessageSent");
                        Debug.Log("ChallengeInstanceID: " + response.ChallengeInstanceId);

                        new LogEventRequest()
                        .SetEventKey("GSD")
                        .SetEventAttribute("challengeInstanceId", response.ChallengeInstanceId)
                        .SetEventAttribute("d", density)
                        .SetEventAttribute("s", size)
                        .SetEventAttribute("cR1", race)
                        .Send((logResponse) => {
                            if (logResponse.HasErrors)
                            {
                                Debug.Log(logResponse.Errors.JSON.ToString());
                            }
                            else
                            {
                                Debug.Log("Challenge Setup");
                            }
                        });
                    }
                });
    }
}

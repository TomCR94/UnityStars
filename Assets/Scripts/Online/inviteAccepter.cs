using GameSparks.Api.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inviteAccepter : MonoBehaviour {

    public PopulateInvites popInvites;

    [SerializeField]
    string density, size;

    [SerializeField]
    Race challengerRace;

    [SerializeField]
    string race;

    [SerializeField]
    string challengeId;

    public Text densityLabel, sizeLabel, challengerRaceLabel, challengerName;

    // Use this for initialization
    void Start ()
    {
        race = JsonUtility.ToJson(Race.getHumanoid(), true);
    }
    
    public void setRace(Race race)
    {
        this.race = JsonUtility.ToJson(race);
    }

    public void setChallengeID(string id)
    {
        challengeId = id;
    }

    public void setChallengerName(string cName)
    {
        challengerName.text = "Invite from: " + cName;
    }

    public void setUniverse(string density, string size)
    {
        densityLabel.text = ((Density)Enum.GetValues(typeof(Density)).GetValue(int.Parse(density))).ToString();
        sizeLabel.text = ((Size)Enum.GetValues(typeof(Size)).GetValue(int.Parse(size))).ToString();
    }

    public void setChallengerRace(Race race)
    {
        challengerRace = race;
        challengerRaceLabel.text = race.getName();
    }

    public void AcceptChallenge()
    {
        new AcceptChallengeRequest().SetChallengeInstanceId(challengeId)
                .SetMessage("You're goin down!")
                .Send((response) =>
                {
                    if (response.HasErrors)
                    {
                        Debug.Log(response.Errors);
                    }
                    else
                    {
                        new LogEventRequest()
                        .SetEventKey("IRD")
                        .SetEventAttribute("challengeInstanceId", response.ChallengeInstanceId)
                        .SetEventAttribute("cR2", race)
                        .Send((logResponse) => {
                            if (logResponse.HasErrors)
                            {
                                Debug.Log(logResponse.Errors.JSON.ToString());
                            }
                            else
                            {
                                Debug.Log("Challenge Setup and Accepted");
                                popInvites.GetChallengeInvites();
                            }
                        });
                    }
                });
    }
}

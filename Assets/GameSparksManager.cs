using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameSparksManager : MonoBehaviour
{

    private static GameSparksManager instance = null;
    [SerializeField]
    private string playerName;
    [SerializeField]
    private string playerID;
    [SerializeField]
    private string challengeId;

    void Awake()
    {
        if (instance == null) // check to see if the instance has a reference
        {
            instance = this; // if not, give it a reference to this class...
            DontDestroyOnLoad(this.gameObject); // and make this object persistent as we load new scenes
        }
        else // if we already have a reference then remove the extra manager from the scene
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
            new AccountDetailsRequest()
                .Send((response) => {
                    if (!response.HasErrors)
                    {
                        playerName = response.DisplayName;
                        playerID = response.UserId;
                    }
                    else
                    {
                        GS.Reset();
                    }
                });
    }

    public static GameSparksManager getInstance()
    {
        return instance;
    }

    public string getPlayerName()
    {
        return playerName;
    }

    public string getPlayerID()
    {
        return playerID;
    }

    public string getChallengeID()
    {
        return challengeId;
    }

    public void setChallengeID(string id)
    {
        challengeId = id;
    }

    public RegistrationResponse RegisterPlayer(string displayName, string password, Text errorText)
    {
        RegistrationResponse result = null;
         new GameSparks.Api.Requests.RegistrationRequest()
           .SetDisplayName(displayName)
           .SetUserName(displayName)
           .SetPassword(password)
           .Send((response) =>
           {
               if (!response.HasErrors)
               {
                   result = response;
                   errorText.color = Color.black;
                   errorText.text = "Player Registered: " + response.DisplayName;
               }
               else
               {
                   errorText.color = Color.red;
                   errorText.text = "Error registering Player...";
               }
           });
        return result;
    }
    public delegate void resetEvent();

    public AuthenticationResponse LoginPlayer(string displayName, string password, Text errorText, resetEvent reset)
    {
        AuthenticationResponse result = null;
        new GameSparks.Api.Requests.AuthenticationRequest()
                .SetUserName(displayName)
                .SetPassword(password)
                .Send((response) =>
                {
                    if (!response.HasErrors)
                    {
                        result = response;
                        playerName = response.DisplayName;
                        playerID = response.UserId;
                        errorText.transform.parent.parent.GetChild(1).gameObject.SetActive(true);
                        errorText.transform.parent.gameObject.SetActive(false);
                        reset.Invoke();
                    }
                    else
                    {
                        Debug.Log("Error authenticating player\n" + response.Errors.JSON.ToString());
                        errorText.color = Color.red;
                        errorText.text = "Error Authenticating Player";
                    }
                });
        return result;
    }
}

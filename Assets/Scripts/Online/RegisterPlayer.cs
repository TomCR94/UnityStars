using GameSparks.Api.Responses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPlayer : MonoBehaviour {
    public Text errorText;
    private string displayName, password;
    public Button finishButton;
    public System.Object thisLock = new System.Object();

    public void setDisplayName(string name)
    {
        displayName = name;
    }

    public void setPassword(string pass)
    {
        password = pass;
    }

    public void FinishButton()
    {
        lock (thisLock)
        {
            Debug.Log("Registering Player...");
            RegistrationResponse response = GameSparksManager.getInstance().RegisterPlayer(displayName, password, errorText, finishButton);
        }
    }
}

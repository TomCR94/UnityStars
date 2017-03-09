using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api;
using GameSparks.Core;
using UnityEngine.UI;
using GameSparks.Api.Requests;

public class LoginLogOut : MonoBehaviour {

    public static LoginLogOut instance;
    private string loggedInName;
    public Button logIn, logOut;
    public Text loggedIn;
    
    private void Update()
    {
        logIn.interactable = logOut.interactable = false;

        if (GS.Authenticated)
        {
            loggedIn.text = GameSparksManager.getInstance().getPlayerName() + " is logged in";
            logIn.gameObject.SetActive(false);
            logOut.gameObject.SetActive(true);
        }
        else
        {
            loggedIn.text = "Player is not logged in";
            logIn.gameObject.SetActive(true);
            logOut.gameObject.SetActive(false);
        }

        logIn.interactable = logOut.interactable = true;


    }

    public void LogOut()
    {
        GS.Reset();
    }
}

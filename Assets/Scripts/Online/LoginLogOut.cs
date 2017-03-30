using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api;
using GameSparks.Core;
using UnityEngine.UI;
using GameSparks.Api.Requests;

public class LoginLogOut : MonoBehaviour {

    public static LoginLogOut instance;
    public Button logIn, logOut;
    public Text loggedIn;
    [Space]
    public Button Online;
    public Button[] refreshButtons;
    public System.Object thisLock = new System.Object();

    private void Start()
    {
        InvokeRepeating("loggingIn", 0f, 5f);
    }

    public void loggingIn()
    {
        logIn.interactable = logOut.interactable = false;

        Online.interactable = GS.Available;

        foreach (Button button in refreshButtons)
            button.interactable = GS.Authenticated;

        if (GS.Authenticated && !string.IsNullOrEmpty(GameSparksManager.getInstance().getPlayerName()))
        {
            loggedIn.text = GameSparksManager.getInstance().getPlayerName() + " is logged in";
            logIn.gameObject.SetActive(false);
            logOut.gameObject.SetActive(true);
            foreach (Button b in refreshButtons)
                b.onClick.Invoke();
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
        lock (thisLock)
        {
            GS.Reset();
        }
    }
}

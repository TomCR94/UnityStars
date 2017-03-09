using GameSparks.Api.Requests;
using GameSparks.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Friend : MonoBehaviour {

    public InviteCreator inviteCreator;
    public string friendName;
    public string userID;
    public Text nameText;
    public Button invite, delete;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        nameText.text = friendName;
	}

    public void showInviteCreator()
    {
        inviteCreator.gameObject.SetActive(true);
        inviteCreator.setPlayerID(userID);
    }

}

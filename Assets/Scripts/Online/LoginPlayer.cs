using GameSparks.Api.Responses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPlayer : MonoBehaviour
{
    public Text errorText;
    public InputField user, pass;
    private string displayName, password;
    public Button finishButton;
    public Button[] refresh;
    public Text loggedIn;
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
            Debug.Log("Authenticating Player...");
            AuthenticationResponse response = GameSparksManager.getInstance().LoginPlayer(displayName, password, errorText, resetFields, finishButton, refresh, loggedIn);
        }
    }

    void resetFields()
    {
        user.text = "";
        pass.text = "";
        errorText.text = "";
    }
}

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
        Debug.Log("Authenticating Player...");
        AuthenticationResponse response = GameSparksManager.getInstance().LoginPlayer(displayName, password, errorText, resetFields);
    }

    void resetFields()
    {
        user.text = "";
        pass.text = "";
        errorText.text = "";
    }
}

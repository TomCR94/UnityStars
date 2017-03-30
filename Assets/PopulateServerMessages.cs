using GameSparks.Api.Requests;
using GameSparks.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateServerMessages : MonoBehaviour {
    [Serializable]
    public struct ServerMessage
    {
        [SerializeField]
        public string messageID;
        [SerializeField]
        public string messageText;
    }
    public int index = 0;
    public Text messageText;
    [Space]
    public Button[] buttons;
    public List<ServerMessage> messages = new List<ServerMessage>();
    public int messageCount = 0;

    private System.Object thisLock = new System.Object();
    // Use this for initialization
    void Start()
    {
        if(GS.Authenticated)
            GetMessages();
    }



    public void Clear()
    {
        messages.Clear();
        setMessage(0);
    }

    public void Next()
    {
        index++;
        setMessage(index);
    }

    public void Previous()
    {
        index--;
        setMessage(index);
    }

    private void Update()
    {
        buttons[0].interactable = messages.Count > 0;

        if (index == 0)
            buttons[1].interactable = false;
        else
            buttons[1].interactable = true;

        if (index < messages.Count - 1)
            buttons[2].interactable = true;
        else
            buttons[2].interactable = false;
    }

    public void setMessage(int i)
    {
        if (messages.Count > i)
            messageText.text = messages[i].messageText;
        else if (messages.Count > 0)
            messageText.text = messages[0].messageText;
        else
            messageText.text = "No available messages";
    }

    public void GetMessages()
    {
        lock (thisLock)
        {
            Clear();
            new ListMessageRequest()
                .Send((response) =>
                {

                    if (response.HasErrors)
                        Debug.Log(response.Errors.JSON);
                    else
                    {
                        foreach (GSData message in response.MessageList)
                        {
                            ServerMessage sMessage = new ServerMessage();
                            sMessage.messageID = message.GetString("messageId");
                            sMessage.messageText = message.GetString("summary");
                            messages.Add(sMessage);
                        }
                        index = 0;
                        setMessage(index);
                    }
                });
        }
    }


    public void DismissMessage()
    {
        new DismissMessageRequest()
            .SetMessageId(messages[index].messageID)
            .Send((response) => {
                if (response.HasErrors)
                    Debug.Log(response.Errors.JSON);
                else
                    Debug.Log("Deleted message");
            });
        GetMessages();
    }
}

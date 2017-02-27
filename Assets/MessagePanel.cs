using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour {

    public Player player;
    public Text textBox;
    public Button nextButton;
    public Button lastButton;

    int index;
    int count = 0;
    // Use this for initialization
    void Start()
    {
        index = 0;
        if (player.getMessages().Count > 0)
        {
            textBox.text = player.getMessages()[index].getText();
            lastButton.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (player.getMessages().Count != count)
        {
            index = 0;
            count = player.getMessages().Count;
        }

        if (player.getMessages().Count > 0)
        {
            textBox.text = player.getMessages()[index].getText();
            if (index == 0)
                lastButton.interactable = false;
            else
                lastButton.interactable = true;

            if (index == player.getMessages().Count - 1)
                nextButton.interactable = false;
            else
                nextButton.interactable = true;
        }
        else
        {
            textBox.text = "No new messages";
            lastButton.interactable = false;
            nextButton.interactable = false;
        }
    }

    public void Next()
    {
        index++;
    }

    public void Last()
    {
        index--;
    }
}

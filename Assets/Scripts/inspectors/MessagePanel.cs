using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour {

    public int playerIndex;
    public Text title;
    public Text textBox;
    public Button nextButton;
    public Button lastButton;

    int index;
    int count = 0;

    // Update is called once per frame
    void Update()
    {

        if (Game.instance.getPlayers()[playerIndex].getMessages().Count != count)
        {
            index = 0;
            count = Game.instance.getPlayers()[playerIndex].getMessages().Count;
        }

        if (Game.instance.getPlayers()[playerIndex].getMessages().Count > 0)
        {
            textBox.text = Game.instance.getPlayers()[playerIndex].getMessages()[index].getText();
            if (index == 0)
                lastButton.interactable = false;
            else
                lastButton.interactable = true;

            if (index == Game.instance.getPlayers()[playerIndex].getMessages().Count - 1)
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

        title.text = "Messages (" + (index + 1) + " of " + count + ")";

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

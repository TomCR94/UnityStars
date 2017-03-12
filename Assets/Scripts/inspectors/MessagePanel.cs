using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour {

    public GameGameObject game;

    public int playerIndex;
    public Text title;
    public Text textBox;
    public Button nextButton;
    public Button lastButton;
    public Button gotoButton;
    public ScrollRectEnsureVisible ensureVisable;

    int index;
    int count = 0;

    // Update is called once per frame
    void Update()
    {

        if (game.getGame().getPlayers()[playerIndex].getMessages().Count != count)
        {
            index = 0;
            count = game.getGame().getPlayers()[playerIndex].getMessages().Count;
        }

        if (game.getGame().getPlayers()[playerIndex].getMessages().Count > 0)
        {
            textBox.text = game.getGame().getPlayers()[playerIndex].getMessages()[index].getText();
            if (index == 0)
                lastButton.interactable = false;
            else
                lastButton.interactable = true;

            if (index == game.getGame().getPlayers()[playerIndex].getMessages().Count - 1)
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

        gotoButton.interactable = game.getGame().getPlayers()[playerIndex].getMessages()[index] != null && game.getGame().getPlayers()[playerIndex].getMessages()[index].getTarget() != null;
    }

    public void Next()
    {
        index++;
    }

    public void Last()
    {
        index--;
    }

    public void Goto()
    {
        if (game.getGame().getPlayers()[playerIndex].getMessages().Count > 0)
        {
            MapObject mapObject = game.getGame().getPlayers()[playerIndex].getMessages()[index].getTarget();
            if (FleetDictionary.instance.getFleetForID(mapObject.getID()) != null)
            {
                Fleet fleet = FleetDictionary.instance.getFleetForID(mapObject.getID());
                if (fleet.FleetGameObject.GetComponent<Button>() != null)
                {
                    ensureVisable.CenterOnItem(fleet.FleetGameObject.GetComponent<Button>().image.rectTransform);
                    fleet.FleetGameObject.gameObject.GetComponent<Button>().onClick.Invoke();
                    fleet.FleetGameObject.gameObject.GetComponent<Button>().image.color = fleet.FleetGameObject.gameObject.GetComponent<Button>().colors.highlightedColor;
                }
                else
                    Settings.instance.selectedFleet = fleet;
            }
            else if (PlanetDictionary.instance.getPlanetForID(mapObject.getID()) != null)
            {
                Planet planet = PlanetDictionary.instance.getPlanetForID(mapObject.getID());
                if (planet.PlanetGameObject.GetComponent<Button>() != null)
                {
                    ensureVisable.CenterOnItem(planet.PlanetGameObject.GetComponent<Button>().image.rectTransform);
                    planet.PlanetGameObject.gameObject.GetComponent<Button>().onClick.Invoke();
                    planet.PlanetGameObject.gameObject.GetComponent<Button>().image.color = planet.PlanetGameObject.gameObject.GetComponent<Button>().colors.highlightedColor;
                }
                else
                    Settings.instance.selectedPlanet = planet;
            }
        }
    }
}

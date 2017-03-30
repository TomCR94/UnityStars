using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour {

    public static MessagePanel instance;

    public GameGameObject game;

    public int playerIndex;
    public Text title;
    public Text textBox;
    public Button nextButton;
    public Button lastButton;
    public Button gotoButton;
    public Button dismissButton;
    public ScrollRectEnsureVisible ensureVisable;

    private Button lastGoto;
    int index;
    int count = 0;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

        if (game.getGame().getPlayers()[playerIndex].getMessages().Count != count)
        {
            index = 0;
            count = game.getGame().getPlayers()[playerIndex].getMessages().Count;
        }

        dismissButton.interactable = true;
        title.text = "Messages (" + (index + 1) + " of " + count + ")";
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
            gotoButton.interactable = game.getGame().getPlayers()[playerIndex].getMessages()[index] != null && game.getGame().getPlayers()[playerIndex].getMessages()[index].getTarget() != null;
        }
        else
        {
            textBox.text = "No new messages";
            lastButton.interactable = false;
            nextButton.interactable = false;
            dismissButton.interactable = false;
            gotoButton.interactable = false;
            title.text = "Messages (0 of 0)";
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

    public void Goto()
    {
        if (game.getGame().getPlayers()[playerIndex].getMessages().Count > 0)
        {
            MapObject mapObject = game.getGame().getPlayers()[playerIndex].getMessages()[index].getTarget();
            Debug.Log(mapObject.getName());
            if (FleetDictionary.instance.getFleetForID(mapObject.getID()) != null)
            {
                Fleet fleet = FleetDictionary.instance.getFleetForID(mapObject.getID());
                Debug.Log(fleet.getName() + "is a fleet");
                if (fleet.FleetGameObject.GetComponent<Button>() != null)
                {
                    if (lastGoto != null)
                    {
                        lastGoto.image.color = lastGoto.colors.normalColor;
                    }
                    ensureVisable.CenterOnItem(fleet.FleetGameObject.GetComponent<Button>().image.rectTransform);
                    fleet.FleetGameObject.gameObject.GetComponent<Button>().onClick.Invoke();
                    fleet.FleetGameObject.gameObject.GetComponent<Button>().image.color = fleet.FleetGameObject.gameObject.GetComponent<Button>().colors.highlightedColor;
                    lastGoto = fleet.FleetGameObject.gameObject.GetComponent<Button>();
                }
                else
                    Settings.instance.selectedFleet = fleet;
            }
            else if (PlanetDictionary.instance.getPlanetForID(mapObject.getName()) != null)
            {
                Planet planet = PlanetDictionary.instance.getPlanetForID(mapObject.getName());
                Debug.Log(planet.getName() + "is a planet");
                if (planet.PlanetGameObject.GetComponent<Button>() != null)
                {
                    if (lastGoto != null)
                    {
                        lastGoto.image.color = lastGoto.colors.normalColor;
                    }
                    ensureVisable.CenterOnItem(planet.PlanetGameObject.GetComponent<Button>().image.rectTransform);
                    planet.PlanetGameObject.gameObject.GetComponent<Button>().onClick.Invoke();
                    planet.PlanetGameObject.gameObject.GetComponent<Button>().image.color = planet.PlanetGameObject.gameObject.GetComponent<Button>().colors.highlightedColor;
                    lastGoto = planet.PlanetGameObject.gameObject.GetComponent<Button>();
                }
                else
                    Settings.instance.selectedPlanet = planet;
            }
            Debug.Log(mapObject.getName() + "is a nothing");
        }
    }

    public void dismiss()
    {
        game.getGame().getPlayers()[playerIndex].getMessages().RemoveAt(index);
    }
}

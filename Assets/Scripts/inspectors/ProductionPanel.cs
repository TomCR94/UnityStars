using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProductionPanel : MonoBehaviour {

    public GameGameObject game;
    public RectTransform currentContentPanel;
    public Button manageButton;

    public static ProductionPanel instance;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update () {
        manageButton.interactable = (Settings.instance.selectedPlanet != null) && Settings.instance.selectedPlanet.getOwnerID() == game.getGame().getPlayers()[MessagePanel.instance.playerIndex].getID();	
	}

    public void init()
    {
        for (int i = 0; i < currentContentPanel.transform.childCount; i++)
        {
            if (currentContentPanel.transform.GetChild(i).name != "base")
                GameObject.Destroy(currentContentPanel.transform.GetChild(i).gameObject);
        }
        if ((Settings.instance.selectedPlanet != null) && Settings.instance.selectedPlanet.getOwnerID() == game.getGame().getPlayers()[MessagePanel.instance.playerIndex].getID())
        {
            List<string> prodNames = Settings.instance.selectedPlanet.getQueue().getItems().Select(e => GetProductionItemName(e)).ToList();

            foreach (string prodName in prodNames)
            {
                GameObject go = GameObject.Instantiate(currentContentPanel.transform.GetChild(0).gameObject, currentContentPanel.transform, false);
                go.name = prodName;
                go.GetComponentInChildren<Text>().text = prodName;
                go.SetActive(true);
            }
        }
    }


    string GetProductionItemName(ProductionQueueItem pqi)
    {
        if (pqi.getType() != QueueItemType.Fleet)
            return pqi.getType().ToString();
        else
            return pqi.getShipDesign().getHullName();
    }
}

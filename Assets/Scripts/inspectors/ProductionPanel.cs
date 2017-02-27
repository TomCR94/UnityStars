using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProductionPanel : MonoBehaviour {
    
    public RectTransform currentContentPanel;
    public Button manageButton;

    public static ProductionPanel instance;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update () {
        manageButton.interactable = (Settings.instance.selected != null && Settings.instance.selected.GetComponent<Planet>() != null);	
	}

    public void init()
    {
        if (Settings.instance.selected != null && Settings.instance.selected.GetComponent<Planet>() != null)
        {
            for (int i = 0; i < currentContentPanel.transform.childCount; i++)
            {
                if (currentContentPanel.transform.GetChild(i).name != "base")
                    GameObject.Destroy(currentContentPanel.transform.GetChild(i).gameObject);
            }

            List<string> prodNames = Settings.instance.selected.GetComponent<Planet>().getQueue().getItems().Select(e => GetProductionItemName(e)).ToList();

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

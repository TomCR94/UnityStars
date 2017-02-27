using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
public class ProductionQueueManager : MonoBehaviour {

    public Dropdown currentProductionItems;
    public Dropdown availibleProductionItems;
    public RectTransform currentContentPanel;
    public RectTransform availibleContentPanel;
    public Text availibleCost;
    public Text currentCost;

    public Planet selectedPlanet;
    public List<string> availibleItems = new List<string>();

    public List<ProductionQueueItem> productionItems = new List<ProductionQueueItem>();
	// Use this for initialization
	void Start () {

        //foreach (ShipDesign design in Game.instance.getPlayers()[0].getDesigns())
        //    availibleItems.Add(design.getHullName());
        availibleProductionItems.ClearOptions();
        availibleItems.AddRange(Enum.GetNames(typeof(QueueItemType)));

        List<ShipDesign> ships = Game.instance.getPlayers()[0].getDesigns();
        ships.Reverse();
        foreach (ShipDesign design in ships)
        {
            availibleItems.Insert(0, design.getName());
        }
        ships.Reverse();
        availibleItems.Remove("Fleet");
        availibleItems.Remove("Starbase");
        availibleProductionItems.AddOptions(availibleItems);

        foreach (string availibleItem in availibleItems)
        {
            GameObject go = GameObject.Instantiate(availibleContentPanel.transform.GetChild(0).gameObject, availibleContentPanel.transform, false);
            go.name = availibleItem;
            go.GetComponentInChildren<Text>().text = availibleItem;
            go.SetActive(true);
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
        updatePopulate();
        if (availibleProductionItems.value >= Game.instance.getPlayers()[0].getDesigns().Count)
            availibleCost.text = new ProductionQueueItem((QueueItemType)availibleProductionItems.value - Game.instance.getPlayers()[0].getDesigns().Count, 1).getCostOfOne(Game.instance.getPlayers()[0].getRace()).toStringFormatted();
        else
        {
            availibleCost.text = new ProductionQueueItem(QueueItemType.Fleet, 1, Game.instance.getPlayers()[0].getDesigns()[availibleProductionItems.value]).getCostOfOne(Game.instance.getPlayers()[0].getRace()).toStringFormatted();
        }

        if (selectedPlanet != null && selectedPlanet.getQueue().getItems().Count > currentProductionItems.value)
        {
            currentCost.text = selectedPlanet.getQueue().getItems()[currentProductionItems.value].getCostOfOne(Game.instance.getPlayers()[0].getRace()).toStringFormatted();
        }
        else
            currentCost.text = "N/A";

    }

    void updatePopulate()
    {
        if (Settings.instance.selected == null)
            return;
        if (Settings.instance.selected.GetComponent<Planet>() == selectedPlanet)
            return;
        populateList();
    }

    public void populateList()
    {
        productionItems.Clear();
        currentProductionItems.ClearOptions();
        for (int i = 0; i < currentContentPanel.transform.childCount; i++)
        {
            if (currentContentPanel.transform.GetChild(i).name != "base")
                GameObject.Destroy(currentContentPanel.transform.GetChild(i).gameObject);
        }
        if (Settings.instance.selected == null)
            return;
        selectedPlanet = Settings.instance.selected.GetComponent<Planet>();
        if (selectedPlanet != null)
        {
            productionItems.AddRange(selectedPlanet.getQueue().getItems());
            List<string> prodNames = productionItems.Select(e => GetProductionItemName(e)).ToList();
            currentProductionItems.AddOptions(prodNames);
            //currentProductionItems.value = 0;


            foreach (string prodName in prodNames)
            {
                GameObject go = GameObject.Instantiate(currentContentPanel.transform.GetChild(0).gameObject, currentContentPanel.transform, false);
                go.name = prodName;
                go.GetComponentInChildren<Text>().text = prodName;
                go.SetActive(true);
            }
        }
        ProductionPanel.instance.init();
    }

    string GetProductionItemName(ProductionQueueItem pqi)
    {
        if (pqi.getType() != QueueItemType.Fleet)
            return pqi.getType().ToString();
        else
            return pqi.getShipDesign().getHullName();
    }

    public void selectCurrentProductionValueForChildPosition(Transform transform)
    {
        Debug.Log("CurrentProd: " + transform.GetSiblingIndex());
        currentProductionItems.value = transform.GetSiblingIndex() - 1;
    }

    public void selectAvailibleProductionValueForChildPosition(Transform transform)
    {
        availibleProductionItems.value = transform.GetSiblingIndex() - 1;
    }

    public void Add()
    {
        if (selectedPlanet == null)
            return;
        if(availibleProductionItems.value >= Game.instance.getPlayers()[0].getDesigns().Count)
            selectedPlanet.getQueue().getItems().Add(new ProductionQueueItem((QueueItemType)availibleProductionItems.value - Game.instance.getPlayers()[0].getDesigns().Count, 1));
        else 
        {
            selectedPlanet.getQueue().getItems().Add(new ProductionQueueItem(QueueItemType.Fleet, 1, Game.instance.getPlayers()[0].getDesigns()[availibleProductionItems.value]));
        }
        populateList();
    }

    public void remove()
    {
        if (selectedPlanet == null)
            return;

        if (selectedPlanet.getQueue().getItems().Count > 0)
        {
            selectedPlanet.getQueue().getItems().RemoveAt(currentProductionItems.value);
            populateList();
        }
    }

    public void ItemUp()
    {
        if (selectedPlanet == null || selectedPlanet.getQueue().getItems().Count < 2)
            return;

        if (currentProductionItems.value > 0)
        {
            ProductionQueueItem item = selectedPlanet.getQueue().getItems()[currentProductionItems.value];
            selectedPlanet.getQueue().getItems().RemoveAt(currentProductionItems.value);
            selectedPlanet.getQueue().getItems().Insert(currentProductionItems.value - 1, item);
            populateList();
        }
    }

    public void ItemDown()
    {
        if (selectedPlanet == null || selectedPlanet.getQueue().getItems().Count < 2)
            return;

        if (currentProductionItems.value < selectedPlanet.getQueue().getItems().Count - 1)
        {
            ProductionQueueItem item = selectedPlanet.getQueue().getItems()[currentProductionItems.value];
            selectedPlanet.getQueue().getItems().RemoveAt(currentProductionItems.value);
            selectedPlanet.getQueue().getItems().Insert(currentProductionItems.value + 1, item);
            populateList();
        }
    }

    public void Clear()
    {
        if (selectedPlanet == null)
            return;
        selectedPlanet.getQueue().getItems().Clear();
        populateList();
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
public class ProductionQueueManager : MonoBehaviour {

    public GameGameObject game;
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

        List<ShipDesign> ships = game.getGame().getPlayers()[0].getDesigns();
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
        if (availibleProductionItems.value >= game.getGame().getPlayers()[0].getDesigns().Count)
            availibleCost.text = new ProductionQueueItem((QueueItemType)availibleProductionItems.value - game.getGame().getPlayers()[0].getDesigns().Count, 1).getCostOfOne(game.getGame().getPlayers()[0].getRace()).toStringFormatted();
        else
        {
            availibleCost.text = new ProductionQueueItem(QueueItemType.Fleet, 1, game.getGame().getPlayers()[0].getDesigns()[availibleProductionItems.value]).getCostOfOne(game.getGame().getPlayers()[0].getRace()).toStringFormatted();
        }

        if (selectedPlanet != null && selectedPlanet.getQueue().getItems().Count > currentProductionItems.value)
        {
            currentCost.text = selectedPlanet.getQueue().getItems()[currentProductionItems.value].getCost(game.getGame().getPlayers()[0].getRace()).toStringFormatted();
        }
        else
            currentCost.text = "N/A";

    }

    void updatePopulate()
    {
        if (Settings.instance.selectedPlanet == null)
            return;
        if (Settings.instance.selectedPlanet == selectedPlanet)
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
        if (Settings.instance.selectedPlanet == null)
            return;
        selectedPlanet = Settings.instance.selectedPlanet;
        if (selectedPlanet != null)
        {
            productionItems.AddRange(selectedPlanet.getQueue().getItems());
            List<string> prodNames = productionItems.Select(e => GetProductionItemName(e)).ToList();
            currentProductionItems.AddOptions(prodNames);
            //currentProductionItems.value = 0;


            for (int i = 0; i < prodNames.Count; i++)
            {
                string prodName = prodNames[i];
                GameObject go = GameObject.Instantiate(currentContentPanel.transform.GetChild(0).gameObject, currentContentPanel.transform, false);
                go.name = prodName;
                go.GetComponentInChildren<Text>().text = prodName + " (" + productionItems[i].getQuantity() + ")";
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
        if (availibleProductionItems.value >= game.getGame().getPlayers()[0].getDesigns().Count)
        {
            if (productionQueueHasType(selectedPlanet.getQueue().getItems(), (QueueItemType)availibleProductionItems.value - game.getGame().getPlayers()[0].getDesigns().Count))
            {
                selectedPlanet.getQueue().getItems()[productionQueueIndex(selectedPlanet.getQueue().getItems(), (QueueItemType)availibleProductionItems.value - game.getGame().getPlayers()[0].getDesigns().Count)].incrementQuantity();
                currentProductionItems.value = productionQueueIndex(selectedPlanet.getQueue().getItems(), (QueueItemType)availibleProductionItems.value - game.getGame().getPlayers()[0].getDesigns().Count);

            }
            else
            {
                selectedPlanet.getQueue().getItems().Add(new ProductionQueueItem((QueueItemType)availibleProductionItems.value - game.getGame().getPlayers()[0].getDesigns().Count, 1));
                currentProductionItems.value = currentProductionItems.options.Count - 1;
            }
        }
        else
        {
            /*if (productionQueueHasType(selectedPlanet.getQueue().getItems(), (QueueItemType)availibleProductionItems.value))
            {
                selectedPlanet.getQueue().getItems()[productionQueueIndex(selectedPlanet.getQueue().getItems(), (QueueItemType)availibleProductionItems.value)].incrementQuantity();
            }
            else*/
                selectedPlanet.getQueue().getItems().Add(new ProductionQueueItem(QueueItemType.Fleet, 1, game.getGame().getPlayers()[0].getDesigns()[availibleProductionItems.value]));
        }
        populateList();
    }

    bool productionQueueHasType(List<ProductionQueueItem> items, QueueItemType type)
    {
        bool result = false; 
        foreach (ProductionQueueItem item in items)
        {
            result |= (item.getType() == type);
        }
        return result;
    }

    int productionQueueIndex(List<ProductionQueueItem> items, QueueItemType type)
    {
        int index = 0;
        foreach (ProductionQueueItem item in items)
        {
            if (item.getType() == type)
            {
                break;
            }
            index++;

        }
        return index;
    }

    public void remove()
    {
        if (selectedPlanet == null)
            return;

        if (selectedPlanet.getQueue().getItems().Count > 0)
        {
            if (selectedPlanet.getQueue().getItems()[currentProductionItems.value].getQuantity() == 1)
            {
                selectedPlanet.getQueue().getItems().RemoveAt(currentProductionItems.value);
                if (currentProductionItems.value >= currentProductionItems.options.Count)
                    currentProductionItems.value = currentProductionItems.options.Count - 1;
            }
            else
            {
                ProductionQueueItem pqi = selectedPlanet.getQueue().getItems()[currentProductionItems.value];
                pqi.addQuantity(-1);
                selectedPlanet.getQueue().getItems().RemoveAt(currentProductionItems.value);
                selectedPlanet.getQueue().getItems().Insert(currentProductionItems.value, pqi);
            }
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

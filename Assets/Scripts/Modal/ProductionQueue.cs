using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ProductionQueue : AbstractStarsObject_NonMono {

    [SerializeField]
    private string planetID;

    [SerializeField]
    private Cost allocated = new Cost();


    [SerializeField]
    private List<ProductionQueueItem> items = new List<ProductionQueueItem>();

    public ProductionQueue() : base()
    {
    }

    public ProductionQueue(Planet planet) : base()
    {
        this.planetID = planet.getID();
    }

    public Cost getAllocated()
    {
        return allocated;
    }

    public void setAllocated(Cost allocated)
    {
        this.allocated = allocated;
    }

    public List<ProductionQueueItem> getItems()
    {
        return items;
    }

    public void setItems(List<ProductionQueueItem> items)
    {
        this.items = items;
    }

    public void setPlanet(Planet planet)
    {
        this.planetID = planet.getID();
    }

    public Planet getPlanet()
    {
        return PlanetDictionary.instance.planetDict[planetID];
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ProductionQueueItem : AbstractStarsObject_NonMono {

    [SerializeField]
    private QueueItemType type;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private ShipDesign shipDesign;

    [SerializeField]
    private string fleetName;

    public ProductionQueueItem() : base()
    {
    }

    public ProductionQueueItem(QueueItemType type, int quantity) : base()
    {
        this.type = type;
        this.quantity = quantity;
    }

    public ProductionQueueItem(QueueItemType type, int quantity, ShipDesign shipDesign) : base()
    {
        this.type = type;
        this.quantity = quantity;
        this.shipDesign = shipDesign;
    }

    public ProductionQueueItem(QueueItemType type, int quantity, ShipDesign shipDesign, string fleetName) : base()
    {
        this.type = type;
        this.quantity = quantity;
        this.shipDesign = shipDesign;
        this.fleetName = fleetName;
    }

    override public string ToString()
    {
        return "ProductionQueueItem [type=" + type + ", quantity=" + quantity + ", shipDesign=" + shipDesign + "]";
    }

    /**
     * Determine the cost of 1 item of this ProductionQueueItem
     */
    public Cost getCostOfOne(Race race)
    {
        Cost cost = new Cost();
        if (type == QueueItemType.Mine || type == QueueItemType.AutoMine)
        {
            cost.setResources(race.getMineCost());
        }
        else if (type == QueueItemType.Factory || type == QueueItemType.AutoFactory)
        {
            cost.setResources(race.getFactoryCost());
            cost.setGermanium(Consts.factoryCostGermanium);
            if (race.isFactoriesCostLess())
            {
                cost.setGermanium(cost.getGermanium() - 1);
            }
        }
        else if (type == QueueItemType.Defense || type == QueueItemType.AutoDefense)
        {
            cost = Consts.defenseCost;
        }
        else if (type == QueueItemType.Alchemy || type == QueueItemType.AutoAlchemy)
        {
            if (race.getLrts().Contains(LRT.MA))
            {
                cost.setResources(Consts.mineralAlchemyLRTCost);
            }
            else
            {
                cost.setResources(Consts.mineralAlchemyCost);
            }
        }
        else if (type == QueueItemType.Fleet || type == QueueItemType.Starbase)
        {
            cost = shipDesign.getAggregate().getCost();
        }

        else if (type == QueueItemType.Terraform || type == QueueItemType.AutoTerraform)
        {
            cost = Consts.terraformCost;
        }

        return cost;
    }

    /**
     * Get the cost of this entire production queue item quanity
     */
    public Cost getCost(Race race)
    {
        return getCostOfOne(race).multiply(quantity);
    }

    public QueueItemType getType()
    {
        return type;
    }

    public void setType(QueueItemType type)
    {
        this.type = type;
    }

    public int getQuantity()
    {
        return quantity;
    }

    public void setQuantity(int quantity)
    {
        this.quantity = quantity;
    }

    public void addQuantity(int quantity)
    {
        this.quantity += quantity;
    }

    public void incrementQuantity()
    {
        this.quantity++;
    }

    public ShipDesign getShipDesign()
    {
        return shipDesign;
    }

    public void setShipDesign(ShipDesign shipDesign)
    {
        this.shipDesign = shipDesign;
    }

    public string getFleetName()
    {
        return fleetName;
    }

    public void setFleetName(string fleetName)
    {
        this.fleetName = fleetName;
    }
}

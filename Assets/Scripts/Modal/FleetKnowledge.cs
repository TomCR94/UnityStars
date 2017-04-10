using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FleetKnowledge : AbstractStarsObject_NonMono
{

    [SerializeField]
    private string fleetId;
    
    private bool pen;

    public FleetKnowledge()
    {
    }

    public FleetKnowledge(Fleet fleet)
    {
        this.fleetId = fleet.getID();
    }

    public void discover(bool pen)
    {
        this.pen = pen;
    }

    public void setPen(bool pen)
    {
        this.pen = pen;
    }

    public bool isPen()
    {
        return pen;
    }

    public void setFleetId(string fleetId)
    {
        this.fleetId = fleetId;
    }

    public string getFleetId()
    {
        return fleetId;
    }

    public override void prePersist()
    {
    }
}

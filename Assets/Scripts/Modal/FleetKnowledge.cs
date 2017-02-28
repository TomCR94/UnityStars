using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetKnowledge : AbstractStarsObject {

    
    private string fleetId;

    /**
     * Do we have knowledge of this fleet by penetrating scanners? We don't keep knowledge of
     * through turns, they are discarded each turn
     */
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

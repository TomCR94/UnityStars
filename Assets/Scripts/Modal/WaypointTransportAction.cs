﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointTransportAction : AbstractStarsObject {
    
    private WaypointTaskTransportType type;
    
    private WaypointTaskTransportAction action;
    
    private int value;

    public WaypointTransportAction() : base()
    {
    }

    public WaypointTransportAction(WaypointTaskTransportType type, WaypointTaskTransportAction action, int value) : base()
    {
        this.type = type;
        this.action = action;
        this.value = value;
    }


    public WaypointTaskTransportType getType()
    {
        return type;
    }


    public void setType(WaypointTaskTransportType type)
    {
        this.type = type;
    }


    public WaypointTaskTransportAction getAction()
    {
        return action;
    }


    public void setAction(WaypointTaskTransportAction action)
    {
        this.action = action;
    }


    public int getValue()
    {
        return value;
    }


    public void setValue(int value)
    {
        this.value = value;
    }
}

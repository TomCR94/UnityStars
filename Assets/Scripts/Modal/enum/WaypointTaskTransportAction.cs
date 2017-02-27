using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The transport action for Transport waypoint tasks 
 */
public enum WaypointTaskTransportAction
{
    None,
    LoadAll,
    UnloadAll,
    LoadAmount,
    UnloadAmount,
    FillPercent,
    WaitForPercent,
    LoadDunnage,
    SetAmountTo,
    SetWaypointTo,
    SetToFleetCapacityPercent,
    DropAndLoad,
}
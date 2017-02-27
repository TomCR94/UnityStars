using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Waypoint : AbstractStarsObject_NonMono
{

    /**
   * The target of this waypoint, if a mapobject
   */
    [SerializeField]
    private MapObject target;

    [SerializeField]
    private int x;
    [SerializeField]
    private int y;
    [SerializeField]
    private int speed;

    /**
     * The task to perform at this waypoint
     */
    [SerializeField]
    private WaypointTask task = WaypointTask.None;

    [SerializeField]
    private Dictionary<WaypointTaskTransportType, WaypointTransportAction> transportActions = new Dictionary<WaypointTaskTransportType, WaypointTransportAction>();

    public Waypoint() : base()
    {
    }

    public Waypoint(int x, int y, int speed, WaypointTask task) : base()
    {
        this.x = x;
        this.y = y;
        this.speed = speed;
        this.task = task;
    }

    public Waypoint(int x, int y, int speed, WaypointTask task, MapObject target) : base()
    {
        this.x = x;
        this.y = y;
        this.speed = speed;
        this.task = task;
        this.target = target;
    }

    public void addTransportAction(WaypointTaskTransportType type, WaypointTaskTransportAction action, int value)
    {
        transportActions.Add(type, new WaypointTransportAction(type, action, value));
    }

    override public string ToString()
    {
        return "Waypoint [target=" + target + ", x=" + x + ", y=" + y + ", speed=" + speed + ", task=" + task + "]";
    }

    public int getX()
    {
        return x;
    }

    public void setX(int x)
    {
        this.x = x;
    }

    public int getY()
    {
        return y;
    }

    public void setY(int y)
    {
        this.y = y;
    }

    public int getSpeed()
    {
        return speed;
    }

    public void setSpeed(int speed)
    {
        this.speed = speed;
    }

    public WaypointTask getTask()
    {
        return task;
    }

    public void setTask(WaypointTask task)
    {
        this.task = task;
    }

    public MapObject getTarget()
    {
        return target;
    }

    public void setTarget(MapObject target)
    {
        this.target = target;
    }

    public void setTransportActions(Dictionary<WaypointTaskTransportType, WaypointTransportAction> transportActions)
    {
        this.transportActions = transportActions;
    }

    public Dictionary<WaypointTaskTransportType, WaypointTransportAction> getTransportActions()
    {
        return transportActions;
    }
}

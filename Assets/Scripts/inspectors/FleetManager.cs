using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FleetManager : MonoBehaviour
{
    [System.NonSerialized]
    public MapObject target;
    public WaypointTask task;
    public Text fleetText;
    public Text buttonText;

    [Space]
    public Button targetButton, add;
    public Dropdown dropdown;

    bool isOn;

    private void Start()
    {
        target = null;
    }

    private void Update()
    {
        add.interactable = target != null; 
        isOn = Settings.instance.selectedFleet != null;

        targetButton.interactable = dropdown.interactable = isOn;

        if (isOn)
        {
            fleetText.text = "Fleet: " + Settings.instance.selectedFleet.getName();
        }
        else
            fleetText.text = "Fleet: ";
    }

    public void setWaypointTask(int i)
    {
        task = (WaypointTask)i;
    }


    public void AddTask()
    {
        if (isOn)
        {
            Settings.instance.selectedFleet.addWaypoint(target.getX(), target.getY(), 5, task, target);
            target = null;
            dropdown.value = 0;
        }
    }

    public void setText()
    {
        if (Settings.instance.setTarget)
        {
            setButtonText("Setting Target");
        }
        else
        {
            setButtonText("Click then select target");
        }

    }

    public void setButtonText(string text)
    {
        buttonText.text = text;
    }

}
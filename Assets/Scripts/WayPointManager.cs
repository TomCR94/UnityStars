using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WayPointManager : MonoBehaviour {

    public GameObject basePanel;
    public GameObject contentPanel;
    List<Waypoint> wpList = new List<Waypoint>();
    public Fleet selectedFleet;

    bool isOn;

	// Update is called once per frame
	void Update () {
        isOn = Settings.instance.selectedFleet != null;
        
        if (isOn)
        {
            if (selectedFleet != Settings.instance.selectedFleet)
            {
                clear();
                selectedFleet = Settings.instance.selectedFleet;
            }
            foreach (Waypoint wp in Settings.instance.selectedFleet.getWaypoints())
            {
                if (!wpList.Contains(wp) && wp.getTarget() !=null)
                {
                    GameObject go = GameObject.Instantiate(basePanel, contentPanel.transform);
                    go.transform.GetChild(0).GetComponent<Text>().text = wp.getTarget().getName();
                    go.transform.GetChild(1).GetComponent<Text>().text = wp.getTask().ToString();
                    go.SetActive(true);
                    wpList.Add(wp);
                }
            }
        }
        else
        {
            clear();
        }

    }

    void clear()
    {
        for (int i = 0; i < contentPanel.transform.childCount; i++)
        {
            if (!contentPanel.transform.GetChild(i).gameObject.Equals(basePanel))
            {
                GameObject.Destroy(contentPanel.transform.GetChild(i).gameObject);
            }
        }
        wpList.Clear();
    }


    public void removeWaypoint(Transform transform)
    {
        Settings.instance.selectedFleet.getWaypoints().RemoveAt(transform.GetSiblingIndex() - 1);

        clear();
    }
}

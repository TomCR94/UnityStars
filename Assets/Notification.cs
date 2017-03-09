using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour {

    public static Notification Instance { get; set; }

	// Use this for initialization
	void Start () {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        bool enableImage = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            enableImage |= transform.GetChild(i).gameObject.activeSelf;
        }
        GetComponent<Image>().enabled = enableImage;
    }

    public GameObject OpenNotificationPanelSingle(string name, string text)
    {
        return OpenNotificationPanelSingle(name, "Notification", text, false);
    }

    public void SimpleNotification(string text)
    {
        OpenNotificationPanelSingle("SimpleNotification", text);
    }

    public GameObject OpenNotificationPanelSingle(string name, string title, string text, bool buttonActive)
    {
        GameObject newNotification;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == name)
            {
                newNotification = transform.GetChild(i).gameObject;
                newNotification.SetActive(true);

                newNotification.transform.GetChild(0).GetChild(1).GetComponentInChildren<Text>().text = title;
                newNotification.transform.GetChild(0).GetChild(2).GetComponentInChildren<Text>().text = text;


                newNotification.transform.GetChild(0).GetChild(3).gameObject.SetActive(buttonActive);

                
                newNotification.transform.SetAsFirstSibling();
                return newNotification;
            }
        }

        newNotification = GameObject.Instantiate(transform.Find("NotificationPanel").gameObject, transform);
        newNotification.SetActive(true);
        newNotification.name = name;
        newNotification.transform.GetChild(0).GetChild(1).GetComponentInChildren<Text>().text = title;
        newNotification.transform.GetChild(0).GetChild(2).GetComponentInChildren<Text>().text = text;
        newNotification.transform.GetChild(0).GetChild(3).gameObject.SetActive(buttonActive);
        
        newNotification.transform.SetAsFirstSibling();

        return newNotification;

    }
}

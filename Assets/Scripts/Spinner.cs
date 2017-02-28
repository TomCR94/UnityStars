using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
public class Spinner : MonoBehaviour {

    public int maxValue, minValue, step = 1;
    public int value;
    Button up, down;
    [Space]
    [SerializeField]
    public SpinnerCallback method;

	// Use this for initialization
	void Start ()
    {
        up = transform.GetChild(0).GetComponent<Button>();
        down = transform.GetChild(1).GetComponent<Button>();
        value = minValue;
        GetComponent<Text>().text = value.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        if (EventSystem.current.currentSelectedGameObject != null)
            if (EventSystem.current.currentSelectedGameObject.Equals(up.gameObject) && value + step  <= maxValue)
            {
                value+=step;
                GetComponent<Text>().text = value.ToString();
                method.Invoke(value);
                EventSystem.current.SetSelectedGameObject(null);
            }
            else if (EventSystem.current.currentSelectedGameObject.Equals(down.gameObject) && value - step >= minValue)
            {
                value-=step;
                GetComponent<Text>().text = value.ToString();
                method.Invoke(value);
                EventSystem.current.SetSelectedGameObject(null);
            }


    }
}

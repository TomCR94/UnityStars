using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SetOutlineSize : MonoBehaviour {

    Settings settings;

	// Use this for initialization
	void Start () {
        settings = GameObject.Find("Settings").GetComponent<Settings>();
        Material mat = transform.FindChild("outline").GetComponent<MeshRenderer>().material;
        Transform trans = this.GetComponent<Transform>();
        mat.SetFloat("_Outline", (float)(trans.lossyScale.x / 75));
	}

    // Update is called once per frame
    void Update () {
        if (!(EventSystem.current.IsPointerOverGameObject()))
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "select")
                    {
                        settings.SetSelected(hit.collider.gameObject);
                        hit.collider.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    else if (!(hit.collider.gameObject.tag == "UI"))
                    {
                        settings.SetNoSelected();
                    }
                }
                else
                {
                    settings.SetNoSelected();
                }
            }
        }
    }
}

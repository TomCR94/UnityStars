using UnityEngine;
using System.Collections;

public class ActiveStateToggler : MonoBehaviour {

    public GameObject go;

	public void ToggleActive () {
        go.SetActive (!go.activeSelf);
	}
}

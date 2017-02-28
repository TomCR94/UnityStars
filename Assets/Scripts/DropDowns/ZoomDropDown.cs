using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;

public class ZoomDropDown : MonoBehaviour
{
    public string[] strings;
    public Zoom zoom;

    [SerializeField]
    Transform menuPanel;
    [SerializeField]
    GameObject buttonPrefab;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < strings.Length; i++)
        {
            GameObject button = (GameObject)Instantiate(buttonPrefab, menuPanel, false);
            button.GetComponentInChildren<Text>().text = strings[i];
            button.GetComponent<Button>().onClick.AddListener(
                () => { setZoom(button.GetComponentInChildren<Text>().text); }
                );
            button.GetComponent<Button>().onClick.AddListener(
                 () => { menuPanel.gameObject.SetActive(false); }
                 );
        }
    }

    void setZoom(string command)
    {
        switch (command)
        {
            case "50x":
                zoom.SetZoom(0.5f);
                break;
            case "100x":
                zoom.SetZoom(1f);
                break;
            case "150x":
                zoom.SetZoom(1.5f);
                break;
            case "200x":
                zoom.SetZoom(2f);
                break;
            case "400x":
                zoom.SetZoom(4f);
                break;
            default:
                break;
        }
    }
}
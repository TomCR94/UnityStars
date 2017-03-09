using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class FileDropDown : MonoBehaviour
{
    public string[] strings;
    public UnityEvent[] events;

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
                () => { ShowObject(button.GetComponentInChildren<Text>().text); }
                );
            button.GetComponent<Button>().onClick.AddListener(
                 () => { menuPanel.gameObject.SetActive(false); }
                 );
        }
    }


    void ShowObject(string command)
    {
        switch (command)
        {
            case "Save":
                events[0].Invoke();
                break;
            case "Exit":
                SceneManager.LoadScene(0);
                break;
            case "Open Save Location":
                Application.OpenURL(Application.persistentDataPath);
                break;
            default:
                break;
        }
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;

public class FileDropDown : MonoBehaviour
{

    public string[] strings;

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
        }
    }

    void ShowObject(string command)
    {
        switch (command)
        {
            case "Exit":
                SceneManager.LoadScene(0);
                break;
            case "Open Race File":
                Application.OpenURL(Application.persistentDataPath + "/Races/Humanoid.race");
                break;
            default:
                break;
        }
    }
}
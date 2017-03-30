using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Linq;

public class DropDownMenu : MonoBehaviour
{

    public GameObject[] objects;

    [SerializeField]
    Transform menuPanel;
    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    GameObject UIPanel;

    // Use this for initialization
    void Start()
    {
        objects = objects.OrderBy(t => t.name).ToArray<GameObject>();

        for (int i = 0; i < objects.Length; i++)
        {
            GameObject button = (GameObject)Instantiate(buttonPrefab, menuPanel, false);
            button.GetComponentInChildren<Text>().text = AddSpacesToSentence(objects[i].name, true);
            int index = i;
            button.GetComponent<Button>().onClick.AddListener(
                () => { ShowObject(index); }
                );
            button.GetComponent<Button>().onClick.AddListener(
                 () => { menuPanel.gameObject.SetActive(false); }
                 );
        }
    }

    void ShowObject(int index)
    {
        objects[index].transform.SetParent(UIPanel.transform);
        objects[index].transform.localPosition = Vector2.zero;
        objects[index].GetComponentInChildren<DragPanel>().enabled = true;
        if (objects[index].GetComponentInChildren<ResizePanel>() != null)
            objects[index].GetComponentInChildren<ResizePanel>().Open();
    }

    string AddSpacesToSentence(string text, bool preserveAcronyms)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;
        StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]))
                if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                    (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                     i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                    newText.Append(' ');
            newText.Append(text[i]);
        }
        return newText.ToString();
    }

}
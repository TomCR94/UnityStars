using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LayoutManager : MonoBehaviour {
    [System.Serializable]
    public class LayoutCollection
    {
        [SerializeField]
        public List<Layout> layouts;
    }

    [System.Serializable]
    public struct Layout
    {
        [SerializeField]
        public string parent;
        [SerializeField]
        public string name;
    }
    
    public GameGameObject game;

    public GameObject SidebarContent, BottomBarContent, ClosedUI;

    public GameObject[] panels;

    public LayoutCollection layoutsForPlayer1 = new LayoutCollection();
    public LayoutCollection layoutsForPlayer2 = new LayoutCollection();

    // Use this for initialization
    void Awake () {
        LoadLayout();
        ProcessLayouts();
	}

    public void LoadLayout()
    {
        string LayoutsLocation;
        if (SceneManager.GetActiveScene().name == "Multiplayer")
            LayoutsLocation = Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/Layouts/";
        else
            LayoutsLocation = Application.persistentDataPath + "/Game/" + game.getGame().getName() + "/Layouts/";
        DirectoryInfo dirInf = new DirectoryInfo(LayoutsLocation);
        Debug.Log(dirInf.ToString());
        if (dirInf.Exists)
        {
            Debug.Log("LayoutsLocation: " + LayoutsLocation);
            Debug.Log("dirExists");
            FileInfo[] files = dirInf.GetFiles("*.layout");
            foreach (FileInfo file in files)
            {
                if (file.Name.Contains("player1"))
                    layoutsForPlayer1 = JsonUtility.FromJson<LayoutCollection>(File.ReadAllText(file.FullName));
                else
                    layoutsForPlayer2 = JsonUtility.FromJson<LayoutCollection>(File.ReadAllText(file.FullName));
            }
        }
    }

    public void ProcessLayouts()
    {
        if (MessagePanel.instance.playerIndex == 0)
            foreach (Layout layout in layoutsForPlayer1.layouts)
            {
                foreach (GameObject obj in panels)
                {
                    if (obj.name == layout.name)
                    {
                        if (layout.parent == "SideBar")
                            obj.transform.parent = SidebarContent.transform;
                        else if (layout.parent == "BottomBar")
                            obj.transform.parent = BottomBarContent.transform;
                        else
                            obj.transform.parent = ClosedUI.transform;
                    }
                }
            }
        else
            foreach (Layout layout in layoutsForPlayer2.layouts)
            {
                foreach (GameObject obj in panels)
                {
                    if (obj.name == layout.name)
                    {
                        if (layout.parent == "SideBar")
                            obj.transform.parent = SidebarContent.transform;
                        else if (layout.parent == "BottomBar")
                            obj.transform.parent = BottomBarContent.transform;
                        else
                            obj.transform.parent = ClosedUI.transform;
                    }
                }
            }
    }

    public void SaveLayout()
    {

        if (MessagePanel.instance.playerIndex == 0)
            layoutsForPlayer1.layouts.Clear();
        else
            layoutsForPlayer2.layouts.Clear();

        foreach (GameObject panel in panels)
        {
            Layout layout = new Layout();
            layout.name = panel.name;
            if(panel.transform.parent.name == "ClosedPanel")
                layout.parent = "ClosedPanel";
            else if (panel.transform.parent.parent.parent.parent.name == "SideBar")
                layout.parent = "SideBar";
            else
                layout.parent = "BottomBar";

            if (MessagePanel.instance.playerIndex == 0)
                layoutsForPlayer1.layouts.Add(layout);
            else
                layoutsForPlayer2.layouts.Add(layout);

        }

        

        if (SceneManager.GetActiveScene().name == "Multiplayer")
        {

            DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/Layouts/");
            if (!dirInf.Exists)
            {
                dirInf.Create();
            }
            else
            {
                dirInf.Delete(true);
                dirInf.Create();
            }

            File.WriteAllText(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/Layouts/player1.layout", JsonUtility.ToJson(layoutsForPlayer1, true));
            File.WriteAllText(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/Layouts/player2.layout", JsonUtility.ToJson(layoutsForPlayer2, true));
        }
        else
        {

            DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Game/" + game.getGame().getName() + "/Layouts/");
            if (!dirInf.Exists)
            {
                dirInf.Create();
            }
            else
            {
                dirInf.Delete(true);
                dirInf.Create();
            }

            File.WriteAllText(Application.persistentDataPath + "/Game/" + game.getGame().getName() + "/Layouts/player1.layout", JsonUtility.ToJson(layoutsForPlayer1, true));
            File.WriteAllText(Application.persistentDataPath + "/Game/" + game.getGame().getName() + "/Layouts/player2.layout", JsonUtility.ToJson(layoutsForPlayer2, true));
        }

    }
}

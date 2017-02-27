using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class NewGameInit : MonoBehaviour {

    public static NewGameInit instance;

    [SerializeField]
    public Size size = Size.Tiny;
    [SerializeField]
    public Density density = Density.Sparse;

    private void Awake()
    {
        if (FindObjectOfType<StaticTechStore>() == null)
        {
            GameObject go = new GameObject("TechStore", typeof(StaticTechStore));
        }
    }

    // Use this for initialization
    void Start () {
        instance = this;
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loadNewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void setSize(int index)
    {
        size = (Size)Enum.GetValues(typeof(Size)).GetValue(index);
    }

    public void setDensity(int index)
    {
        density = (Density)Enum.GetValues(typeof(Density)).GetValue(index);
    }
}

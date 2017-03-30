using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetDictionary : MonoBehaviour {

    public static PlanetDictionary instance;

    public Dictionary<string, Planet> planetDict = new Dictionary<string, Planet>();

    private void Awake()
    {
        instance = this;
    }

    public Planet getPlanetForID(string ID)
    {
        if (planetDict.ContainsKey(ID))
            return planetDict[ID];
        else
            return null;
    }

}

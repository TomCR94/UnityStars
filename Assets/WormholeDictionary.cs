using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormholeDictionary : MonoBehaviour {

    public static WormholeDictionary instance;

    public Dictionary<string, Wormhole> WormholeDict = new Dictionary<string, Wormhole>();

    private void Awake()
    {
        instance = this;
    }

    public Wormhole getWormholeForID(string ID)
    {
        if (WormholeDict.ContainsKey(ID))
            return WormholeDict[ID];
        else
            return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetDictionary : MonoBehaviour
{

    public static FleetDictionary instance;

    public Dictionary<string, Fleet> fleetDict = new Dictionary<string, Fleet>();

    private void Awake()
    {
        instance = this;
    }

    public Fleet getFleetForID(string ID)
    {
        if (fleetDict.ContainsKey(ID))
            return fleetDict[ID];
        else
            return null;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormholeGameObject : MonoBehaviour {

    public Wormhole wormhole;

    public void setWormhole(Wormhole wormhole)
    {
        wormhole.WormholeGameObject = this;
        this.wormhole = wormhole;
    }

    public Wormhole getWormhole()
    {
        return wormhole;
    }

}

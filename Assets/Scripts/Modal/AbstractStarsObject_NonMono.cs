using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractStarsObject_NonMono {
    [SerializeField]
    protected string id = Guid.NewGuid().ToString();

    public virtual void prePersist()
    {

    }

    public void setID(string id)
    {
        this.id = id;
    }

    public string getID()
    {
        if (id == null || id == "")
            id = Guid.NewGuid().ToString();

        return id;
    }

}

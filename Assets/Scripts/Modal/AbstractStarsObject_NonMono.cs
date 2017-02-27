using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractStarsObject_NonMono {
    protected long id;

    public virtual void prePersist()
    {

    }

    public void setID(long id)
    {
        this.id = id;
    }

    public long getID()
    {
        return id;
    }
}

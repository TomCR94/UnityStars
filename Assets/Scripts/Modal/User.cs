using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class User : AbstractStarsObject_NonMono {

    [SerializeField]
    private string name;
    
    /**
	 * Is this an AI user
	 */
    [SerializeField]
    private bool ai;

    public User()
    {

    }

    public User(String name) : base()
    {
        this.name = name;
    }

    override public string ToString()
    {
        return "User [name=" + name + ", ai=" + ai + "]";
    }

    public String getName()
    {
        return name;
    }

    public void setName(String name)
    {
        this.name = name;
    }

    public void setAi(bool ai)
    {
        this.ai = ai;
    }

    public bool isAi()
    {
        return ai;
    }

    public override void prePersist()
    {
    }
}

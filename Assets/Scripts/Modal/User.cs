using System.Collections.Generic;
using UnityEngine;
using System;

public class User : AbstractStarsObject {

    [SerializeField]
    private string name;

    [SerializeField]
    private string password;

    [SerializeField]
    private List<Role> roles = new List<Role>();

    /**
	 * Is this an AI user
	 */
    [SerializeField]
    private bool ai;

    public User()
    {

    }

    public User(String name, String password, List<Role> roles) : base()
    {
        this.name = name;
        this.password = password;
        this.roles = roles;
    }

    override public string ToString()
    {
        return "User [name=" + name + ", roles=" + roles + ", ai=" + ai + "]";
    }

    public String getName()
    {
        return name;
    }

    public void setName(String name)
    {
        this.name = name;
    }

    public String getPassword()
    {
        return password;
    }

    public void setPassword(String password)
    {
        this.password = password;
    }

    public List<Role> getRoles()
    {
        return roles;
    }

    public void setRoles(List<Role> roles)
    {
        this.roles = roles;
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
        throw new NotImplementedException();
    }
}

using System.Collections.Generic;
using UnityEngine;
using System;

public class UserSessionData : AbstractStarsObject {
    User user;

    string uuid;
    
    private DateTime createDate;

    public UserSessionData()
    {

    }

    public UserSessionData(User user) : base()
    {
        this.user = user;
        uuid = "temp";
        createDate = new DateTime();
    }

    public User getUser()
    {
        return user;
    }

    public void setUser(User user)
    {
        this.user = user;
    }

    public String getUuid()
    {
        return uuid;
    }

    public void setUuid(String uuid)
    {
        this.uuid = uuid;
    }

    public void setCreateDate(DateTime createDate)
    {
        this.createDate = createDate;
    }

    public DateTime getCreateDate()
    {
        return createDate;
    }

    public override void prePersist()
    {
        throw new NotImplementedException();
    }
}

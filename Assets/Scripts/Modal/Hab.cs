using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hab {

    [SerializeField]
    private int grav;
    [SerializeField]
    private int temp;
    [SerializeField]
    private int rad;

    public Hab()
    {
        grav = temp = rad = 0;
    }

    public Hab(int grav, int temp, int rad)
    {
        this.grav = grav;
        this.temp = temp;
        this.rad = rad;
    }

    /**
     * Copy constructor
     * 
     * @param hab The instance of the hab to copy
     */
    public Hab(Hab hab)
    {
        this.grav = hab.grav;
        this.temp = hab.temp;
        this.rad = hab.rad;
    }

    override public string ToString()
    {
        return "Hab [grav=" + grav + ", temp=" + temp + ", rad=" + rad + "]";
    }

    public string gravString()
    {
        int result, tmp = Mathf.Abs(grav - 50);
        if (tmp <= 25)
            result = (tmp + 25) * 4;
        else
            result = tmp * 24 - 400;
        if (grav < 50)
            result = 10000 / result;

        return string.Format("{0}.{1}dg", result / 100, result % 100);
    }

    public string tempString(int temp)
    {
        int result;
        result = (temp - 50) * 4;

        return string.Format("{0}C", result);
    }

    public string radString()
    {
        return rad + "mR";
    }


    public int getAtIndex(int index)
    {
        if (index == 0)
        {
            return grav;
        }
        else if (index == 1)
        {
            return temp;
        }
        else if (index == 2)
        {
            return rad;
        }
        else
            return -1;
    }

    public void setAtIndex(int index, int value)
    {
        if (index == 0)
        {
            setGrav(value);
        }
        else if (index == 1)
        {
            setTemp(value);
        }
        else if (index == 2)
        {
            setRad(value);
        }
        else
        {
        }
    }
    

    public int getGrav()
    {
        return grav;
    }

    public void setGrav(int grav)
    {
        this.grav = grav;
    }

    public int getTemp()
    {
        return temp;
    }

    public void setTemp(int temp)
    {
        this.temp = temp;
    }

    public int getRad()
    {
        return rad;
    }

    public void setRad(int rad)
    {
        this.rad = rad;
    }
}

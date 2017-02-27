using System.Collections;
using System;
using UnityEngine;
public class TechComparator<T>: IComparer where T : Tech {

    public int Compare(object x, object y)
    {
        T left = (T)x;
        T right = (T)y;
        int diff = left.getRanking() - right.getRanking();
        if (diff < 0)
            return -1;
        if (diff > 0)
            return 1;
        else
            return 0;
    }
}

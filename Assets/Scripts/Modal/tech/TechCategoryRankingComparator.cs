using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechCategoryRankingComparator<T>: IComparer where T : Tech {
    public int Compare(object x, object y)
    {
        T left = (T)x;
        T right = (T)y;
        int diff = left.getCategory().ToString().CompareTo(right.getCategory().ToString());
        if (diff == 0)
        {
            diff = left.getRanking() - right.getRanking();
        }
        if (diff < 0)
            return -1;
        if (diff > 0)
            return 1;
        else
            return 0;
    }

}

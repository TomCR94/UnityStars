using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Util Class to return an array of type T containing Enum values
 */
public class EnumSet<T>  {
    
	public static T[] of(params T[] args) {
        return args;
	}
}

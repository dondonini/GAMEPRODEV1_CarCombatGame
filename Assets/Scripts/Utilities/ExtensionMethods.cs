using UnityEngine;
using System.Collections;

public static class ExtensionMethods {

	public static int RoundToClosest10(int i)
    {
        return (Mathf.RoundToInt(i / 10)) * 10;
    }
}

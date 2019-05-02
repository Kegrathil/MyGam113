using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static partial class TransformExtionsionMethods
{
    public static void CopyLocalsFrom(this Transform trans, Transform from)
    {
        trans.localScale = from.localScale;
        trans.localRotation = from.localRotation;
        trans.localPosition = from.localPosition;
    }
    public static void CopyFrom(this Transform trans, Transform from)
    {
        trans.localScale = from.localScale;
        trans.rotation = from.rotation;
        trans.position = from.position;
    }

    public static IEnumerable<Transform> Children(this Transform t)
    {
        foreach (Transform c in t)
            yield return c;
    }
}

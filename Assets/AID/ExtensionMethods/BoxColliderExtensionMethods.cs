using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class BoxColliderExtensionMethods
{
    public static void EncapsulateGameObject(this BoxCollider b, GameObject target)
    {
        var res = target.GetComponentsInChildren<Renderer>();

        Bounds bounds = new Bounds();

        for (int i = 0; i < res.Length; i++)
        {
            var t = res[i].bounds;

            if (i == 0)
                bounds = t;
            else
                bounds.Encapsulate(t);
        }

        b.center = bounds.center - b.transform.localPosition;
        b.size = bounds.extents*2;
    }
}

using UnityEngine;
using System.Collections;

public static class Vector4ExtensionMethods
{
	public static Vector4 FromFloatArray(this Vector4 v, float[] fs)
    {        
        if(fs != null)
        {
            if(fs.Length > 0)
                v.x = fs[0];
            if (fs.Length > 1)
                v.y = fs[1];
            if (fs.Length > 2)
                v.z = fs[2];
            if (fs.Length > 3)
                v.w = fs[3];
        }

        return v;
    }
}

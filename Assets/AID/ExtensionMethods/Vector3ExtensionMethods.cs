using UnityEngine;
using System.Collections;

public static class Vector3ExtensionMethods
{

	public static Vector3 EulerAngleLerp(this Vector3 v3, Vector3 from, Vector3 to, float percent)
	{
		return new Vector3(Mathf.LerpAngle(from.x, to.x, percent),
			Mathf.LerpAngle(from.y, to.y, percent),
			Mathf.LerpAngle(from.z, to.z, percent));
	}
    public static void Abs(this Vector3 vec3)
    {
        vec3.x = Mathf.Abs(vec3.x);
        vec3.y = Mathf.Abs(vec3.y);
        vec3.z = Mathf.Abs(vec3.z);
    }

    public static Vector3 Capped(this Vector3 vec3, float len)
    {
        if (vec3.sqrMagnitude > len * len)
            return vec3.normalized * len;
        return vec3;
    }

    public static Vector3 Inverse(this Vector3 vec3)
    {
        return new Vector3(1.0f / vec3.x, 1.0f / vec3.y, 1.0f / vec3.z);
    }

    //enforces a minimum speed for use with moving start point lerps 
    public static Vector3 LerpMinSpeed(this Vector3 vec3, Vector3 from, Vector3 to, float percentage, float minSpeed)
    {
        var dif = to - from;
        var moveBy = dif * percentage;
        if (moveBy.sqrMagnitude < minSpeed * minSpeed)
            return Vector3.MoveTowards(from, to, minSpeed);

        return from + moveBy;
    }

    public static Vector3 LerpMaxSpeed(this Vector3 v, Vector3 from, Vector3 to, float percentage, float maxAmt)
    {
        var dif = to - from;
        var moveBy = dif * percentage;
        moveBy = moveBy.Capped(maxAmt);

        return from + moveBy;
    }
}

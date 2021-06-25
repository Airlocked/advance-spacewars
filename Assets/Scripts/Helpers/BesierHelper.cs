using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BesierHelper
{
    public static Vector3 QuadLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        var p0 = Vector3.Lerp(a, b, t);
        var p1 = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(p0, p1, t);
    }

    public static Quaternion QuadLerp(Quaternion a, Quaternion b, Quaternion c, float t)
    {
        var p0 = Quaternion.Lerp(a, b, t);
        var p1 = Quaternion.Lerp(b, c, t);
        return Quaternion.Lerp(p0, p1, t);
    }

    public static Vector3 CubeLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        var p0 = QuadLerp(a, b, c, t);
        var p1 = QuadLerp(b, c, d, t);
        return Vector3.Lerp(p0, p1, t);
    }

    public static Quaternion CubeLerp(Quaternion a, Quaternion b, Quaternion c, Quaternion d, float t)
    {
        var p0 = QuadLerp(a, b, c, t);
        var p1 = QuadLerp(b, c, d, t);
        return Quaternion.Lerp(p0, p1, t);
    }
}

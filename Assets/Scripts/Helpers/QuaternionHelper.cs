using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionHelper
{
    public static Quaternion GetValid(Quaternion q)
    {
        if (q.x == 0f && q.y == 0f && q.z == 0f && q.w == 0f) return Quaternion.identity;
        return q;
    }
}

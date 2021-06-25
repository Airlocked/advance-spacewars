using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHelper
{
    public static Vector3 GetMousePointOnPlane(Plane plane)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out var hit))
        {
            return ray.GetPoint(hit);
        }
        return Vector3.zero;
    }

    public static T GetComponentUnderMouse<T>(float distance = 500) where T : Component
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, distance))
        {
            return hitInfo.collider.GetComponent<T>();
        }
        return null;
    }
}

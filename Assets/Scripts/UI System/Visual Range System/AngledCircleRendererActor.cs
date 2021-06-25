using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class which can draw line circles and angles;
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public partial class AngledCircleRendererActor : MonoBehaviour
{
    protected LineRenderer lineRenderer;
}

/// <summary>
/// Lifecycle
/// </summary>
public partial class AngledCircleRendererActor
{
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
}

/// <summary>
/// Public actions
/// </summary>
public partial class AngledCircleRendererActor
{
    /// <summary>
    /// Draws line arc on a horizontal plane
    /// </summary>
    /// <param name="center"></param>
    /// <param name="startAngle"></param>
    /// <param name="endAngle"></param>
    /// <param name="radius"></param>
    /// <param name="step"></param>
    public void DrawArc(Vector3 center, float startAngle, float endAngle, float radius, float step)
    {
        var points = GetArcPoints(center, startAngle, endAngle, radius, step, false);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}

public partial class AngledCircleRendererActor
{
    protected List<Vector3> GetArcPoints(Vector3 center, float startAngle, float endAngle, float radius, float step = 2, bool reverse = false)
    {
        var points = new List<Vector3>();

        if (reverse)
        {
            var currentAngle = endAngle;
            while (currentAngle > startAngle)
            {
                var point = GetPointFor(currentAngle, radius, center);
                points.Add(point);

                currentAngle -= step;
            }

            var endPoint = GetPointFor(startAngle, radius, center);
            points.Add(endPoint);
        }
        else
        {
            var currentAngle = startAngle;
            while (currentAngle < endAngle)
            {
                var point = GetPointFor(currentAngle, radius, center);
                points.Add(point);

                currentAngle += step;
            }

            var endPoint = GetPointFor(endAngle, radius, center);
            points.Add(endPoint);
        }
        

        return points;
    }

    protected Vector3 GetPointFor(float angle, float radius, Vector3 center)
    {
        return new Vector3
        {
            x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius + center.x,
            y = center.y,
            z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius + center.z
        };
    }
}

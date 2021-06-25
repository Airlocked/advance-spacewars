using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Draws trajectory line for unit
/// </summary>
public partial class MoveTrajectoryRenderer : AngledCircleRendererActor
{
    [SerializeField] private UnitActor unit;
}

/// <summary>
/// Lifecycle
/// </summary>
public partial class MoveTrajectoryRenderer
{
    private void Start()
    {
        if (unit == null) unit = GetComponentInParent<UnitActor>();
    }

    private void Update()
    {
        DrawMoveLine();
    }
}

/// <summary>
/// Private actions
/// </summary>
public partial class MoveTrajectoryRenderer
{
    private void DrawMoveLine()
    {
        var points = GetTrajectoryPoints(0.03f);

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    private List<Vector3> GetTrajectoryPoints(float step)
    {
        var points = new List<Vector3>();

        var value = 0.001f;

        while (value < 1f)
        {
            points.Add(unit.EvaluateTrajectory(value).position);
            value += step;
        }
        points.Add(unit.EvaluateTrajectory(1f).position);

        return points;
    }
}

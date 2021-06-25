using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Draws movement border of a unit
/// </summary>
public partial class MoveArcRenderer : AngledCircleRendererActor
{
    [SerializeField] private UnitActor unit;
}

/// <summary>
/// Lifecycle
/// </summary>
public partial class MoveArcRenderer
{
    private void Start()
    {
        if (unit == null) unit = GetComponentInParent<UnitActor>();
    }

    private void Update()
    {
        DrawMoveArc();
    }
}

/// <summary>
/// Private actions
/// </summary>
public partial class MoveArcRenderer
{
    private void DrawMoveArc()
    {
        if (GameController.Shared == null) return;
        if (UnitManager.Shared.SelectedUnit == unit && unit.TeamID == GameController.Shared.CurrentTeamID)
        {
            var points = GetArcPoints(transform.localPosition, -unit.DataModel.MoveAngle / 2f + 90, unit.DataModel.MoveAngle / 2f + 90, unit.DataModel.MoveDistance);
            points.Add(transform.localPosition);

            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }
}

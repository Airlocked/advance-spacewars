using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Draws attack radius of a unit
/// </summary>
public partial class AttackArcRenderer : AngledCircleRendererActor
{
    [SerializeField] private UnitActor unit;
    [Space]
    [SerializeField] private Gradient selectedGradient;
    [SerializeField] private Gradient deselectedGradient;
}

/// <summary>
/// Lifecycle
/// </summary>
public partial class AttackArcRenderer
{
    private void Start()
    {
        if (unit == null) unit = GetComponentInParent<UnitActor>();
    }

    private void Update()
    {
        DrawAttackArc();
    }
}

/// <summary>
/// Private actions
/// </summary>
public partial class AttackArcRenderer
{
    private void DrawAttackArc()
    {
        var points = GetArcPoints(unit.TargetPose.position, 0, 359, unit.DataModel.AttackRadius);

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());

        if (UnitManager.Shared.SelectedUnit == unit || UnitManager.Shared.TargetUnit == unit)
            lineRenderer.colorGradient = selectedGradient;
        else
            lineRenderer.colorGradient = deselectedGradient;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays confirmation pointers for actions
/// </summary>
public partial class ActionConfirmationManager : MonoBehaviour
{
    [SerializeField] private ActionConfirmationIcon moveIcon;
    [SerializeField] private ActionConfirmationIcon attackIcon;

    [Space]
    [SerializeField] private GameController gameController;

}

/// <summary>
/// Lifecycle
/// </summary>
public partial class ActionConfirmationManager
{
    private void Awake()
    {
        moveIcon.SetButtonAction(OnMoveConfirmClick);
        attackIcon.SetButtonAction(OnAttackConfirmClick);
    }

    private void LateUpdate()
    {
        HandleMovePointer();
        HandleAttackPointer();
    }
}

/// <summary>
/// Private actions
/// </summary>
public partial class ActionConfirmationManager
{
    // Move
    private void HandleMovePointer()
    {
        var unit = UnitManager.Shared.SelectedUnit;
        if (gameController.GameState == GameState.MoveLocationSelected && unit != null && !unit.IsMoved)
        {
            var worldPosition = unit.TargetPose.position;
            moveIcon.Show(worldPosition);
        }
        else
        {
            moveIcon.Hide();
        }
    }

    private void OnMoveConfirmClick()
    {
        gameController.ConfirmMovementAction();
    }

    // Attack
    private void HandleAttackPointer()
    {
        var unit = UnitManager.Shared.SelectedUnit;
        var target = UnitManager.Shared.TargetUnit;
        if (gameController.GameState == GameState.AttackTargetSelected && target != null)
        {
            var worldPosition = target.CurrentPose.position;

            // Text handling
            if (unit != null)
            {
                var distance = (target.CurrentPose.position - unit.CurrentPose.position).magnitude;
                if (distance < unit.DataModel.AttackRadius)
                {
                    attackIcon.buttonText.text = "АТАКА";
                }
                else
                {
                    attackIcon.buttonText.text = "НЕДОСТУПНО";
                }
            }

            attackIcon.Show(worldPosition);
        }
        else
        {
            attackIcon.Hide();
        }
    }

    private void OnAttackConfirmClick()
    {
        gameController.ConfirmAttackCommand();
    }
}
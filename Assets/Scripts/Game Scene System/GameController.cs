using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    NoSelection,
    UnitSelected,
    MoveLocationSelected,
    AttackTargetSelected,
    UnitBusy,
}

/// <summary>
/// Controls the flow of the game. Executes the turn order.
/// </summary>
public partial class GameController : MonoBehaviour
{
    public static GameController Shared { get; private set; }
}

/// <summary>
/// Game State
/// </summary>
public partial class GameController
{
    [Header("State Machine")]
    private GameState state;
    public GameState GameState
    {
        get { return state; }
        set
        {
            if (state == value) return;
            state = value;
            Debug.Log($"Game Controller: State changed to {state}");
            onStateChanged.Invoke();
        }
    }
    public UnityEvent onStateChanged = new UnityEvent();
}

/// <summary>
/// Turn Order Management
/// </summary>
public partial class GameController
{
    [SerializeField] private int numberOfTeams;
    [SerializeField] private int currentTeamID;
    public int CurrentTeamID => currentTeamID;
}

/// <summary>
/// Lifecycle
/// </summary>
public partial class GameController
{
    private void Awake()
    {
        Shared = this;
    }

    private void Start()
    {
        GameState = GameState.NoSelection;
    }

    private void Update()
    {
        HandleTrajectoryInput();
        HandleSelectionInput();
        HandleDeselectionInput();
    }
}

/// <summary>
/// Public actions
/// </summary>
public partial class GameController
{
    public void ConfirmMovementAction()
    {
        var selectedUnit = UnitManager.Shared.SelectedUnit;
        if (selectedUnit == null) return;
        if (selectedUnit.TeamID != CurrentTeamID) return;
        if (selectedUnit.IsMoved) return;

        //selectedUnit.MoveToPosition();
        //GameState = GameState.UnitBusy;
        StartCoroutine(MoveRoutine(selectedUnit));
    }

    public void ConfirmAttackCommand()
    {
        if (GameState != GameState.AttackTargetSelected) return;
        var unit = UnitManager.Shared.SelectedUnit;
        var target = UnitManager.Shared.TargetUnit;

        if (unit == null || target == null) return;
        if ((target.CurrentPose.position - unit.CurrentPose.position).magnitude > unit.DataModel.AttackRadius) return;

        StartCoroutine(AttackRoutine(unit, target));

    }

    /// <summary>
    /// Ends current turn and passes it to next team
    /// </summary>
    public void EndTurn()
    {
        foreach (var unit in UnitManager.Shared.units)
            unit.EndTurn();

        if (currentTeamID < numberOfTeams - 1) currentTeamID++;
        else currentTeamID = 0;

        SelectUnit(null);
    }
}

/// <summary>
/// Private actions
/// </summary>
public partial class GameController
{
    /// <summary>
    /// If selected unit is in active team and didn't moved yet, player can choose trajectory by mouse pointer and set it with a click.
    /// </summary>
    private void HandleTrajectoryInput()
    {
        var selectedUnit = UnitManager.Shared.SelectedUnit;
        if (selectedUnit == null) return;
        if (selectedUnit.TeamID != CurrentTeamID) return;
        if (selectedUnit.IsMoved) return;
        if (GameState != GameState.UnitSelected) return;

        var mousePosition = MouseHelper.GetMousePointOnPlane(new Plane(Vector3.up, Vector3.zero));
        selectedUnit.SetTargetMovePosition(mousePosition);

        if (Input.GetMouseButtonDown(0)) GameState = GameState.MoveLocationSelected;
    }

    /// <summary>
    /// 
    /// </summary>
    private void HandleSelectionInput()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        var unit = MouseHelper.GetComponentUnderMouse<UnitActor>();
        if (unit != null) SelectUnit(unit);
    }

    /// <summary>
    /// If unit is selected and not in transit state, player can deselect it with ESC button
    /// </summary>
    private void HandleDeselectionInput()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        var selectedUnit = UnitManager.Shared.SelectedUnit;

        switch (GameState)
        {
            case GameState.NoSelection:
                break;

            case GameState.UnitSelected:
                // Drop selection
                if (selectedUnit == null) return;
                selectedUnit.ResetTargetPose();
                SelectUnit(null);
                break;

            case GameState.MoveLocationSelected:
                // Drop location selection
                if (selectedUnit == null) return;
                selectedUnit.ResetTargetPose();
                GameState = GameState.UnitSelected;
                break;

            case GameState.AttackTargetSelected:
                UnitManager.Shared.TargetUnit = null;
                GameState = GameState.UnitSelected;
                break;
        }
    }

    // Event handling
    private void SelectUnit(UnitActor unit)
    {
        if (unit == null)
        {
            UnitManager.Shared.SelectedUnit = null;
            UnitManager.Shared.TargetUnit = null;
            GameState = GameState.NoSelection;
            return;
        }
        // State dependent behaviour

        // Юнит на нашей стороне
        if (unit.TeamID == CurrentTeamID)
        {
            if (UnitManager.Shared.SelectedUnit == unit) return;

            switch (GameState)
            {
                case GameState.NoSelection:
                    UnitManager.Shared.SelectedUnit = unit;
                    GameState = GameState.UnitSelected;
                    break;

                case GameState.UnitSelected:
                    UnitManager.Shared.SelectedUnit = unit;
                    GameState = GameState.UnitSelected;
                    break;

                case GameState.MoveLocationSelected:
                    UnitManager.Shared.SelectedUnit.ResetTargetPose();
                    UnitManager.Shared.SelectedUnit = unit;
                    GameState = GameState.UnitSelected;
                    break;

                case GameState.AttackTargetSelected:
                    UnitManager.Shared.SelectedUnit.ResetTargetPose();
                    UnitManager.Shared.SelectedUnit = unit;
                    GameState = GameState.UnitSelected;
                    break;

                case GameState.UnitBusy:
                    break;
            }
        }
        // Юнит это цель
        else
        {
            switch (GameState)
            {
                case GameState.NoSelection:
                    UnitManager.Shared.TargetUnit = unit;
                    break;

                case GameState.UnitSelected:
                    UnitManager.Shared.TargetUnit = unit;
                    GameState = GameState.AttackTargetSelected;
                    break;

                case GameState.MoveLocationSelected:
                    UnitManager.Shared.SelectedUnit.ResetTargetPose();
                    UnitManager.Shared.TargetUnit = unit;
                    GameState = GameState.AttackTargetSelected;
                    break;

                case GameState.AttackTargetSelected:
                    UnitManager.Shared.TargetUnit = unit;
                    break;

                case GameState.UnitBusy:
                    break;
            }
        }
    }
}

/// <summary>
/// Routines
/// </summary>
public partial class GameController
{
    private IEnumerator MoveRoutine(UnitActor unit)
    {
        GameState = GameState.UnitBusy;
        unit.FlagIsMoved();
        var value = 0f;
        while (value < 1f)
        {
            var pose = unit.EvaluateTrajectory(value);
            unit.transform.position = pose.position;
            unit.transform.rotation = pose.rotation;

            yield return new WaitForEndOfFrame();
            value += Time.deltaTime / unit.DataModel.MoveSpeedSeconds;
        }
        unit.transform.position = unit.TargetPose.position;
        unit.transform.rotation = unit.TargetPose.rotation;
        unit.ResetCurrentPose();
        unit.ResetTargetPose();

        GameState = GameState.UnitSelected;
    }

    private IEnumerator AttackRoutine(UnitActor attacker, UnitActor defender)
    {
        GameState = GameState.UnitBusy;
        // Attacker ship attacks
        attacker.FlagIsAttacked();
        attacker.AttackOnPosition(defender.transform.position);
        yield return new WaitForSeconds(1);
        defender.SetDamage(3);

        if ((attacker.CurrentPose.position - defender.CurrentPose.position).magnitude < defender.DataModel.AttackRadius)
        {
            // Delay between attacks
            yield return new WaitForSeconds(1);

            // Defender ship fights back
            defender.AttackOnPosition(attacker.transform.position);
            yield return new WaitForSeconds(1);
            attacker.SetDamage(3);
        }

        GameState = GameState.UnitSelected;
        SelectUnit(attacker);
    }
}
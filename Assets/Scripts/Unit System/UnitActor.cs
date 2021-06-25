using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Battle Unit
/// </summary>
///
[RequireComponent(typeof(SphereCollider))]
public partial class UnitActor : MonoBehaviour
{
    [Header("Data Model")]
    [SerializeField] private UnitDataModel dataModel;
    public UnitDataModel DataModel => dataModel;

    [Header("Team Data")]
    [SerializeField] int teamID;
    public int TeamID => teamID;

    [Header("Unit Data")]
    [SerializeField] int currentHealth;
    public int CurrentHealth => currentHealth;

    [Header("Collider Data")]
    [SerializeField] SphereCollider selectionCollider;
    public SphereCollider SelectionCollider => selectionCollider;

    [Header("Trajectory Data")]
    [SerializeField] private Pose currentPose;
    public Pose CurrentPose => currentPose;

    [SerializeField] private Pose targetPose;
    public Pose TargetPose => targetPose;

    [Header("Action Data")]
    [SerializeField] private bool isBusy = false;
    public bool IsBusy => isBusy;

    [SerializeField] private bool isMoved;
    public bool IsMoved => isMoved;

    [SerializeField] private bool isAttacked;
    public bool IsAtttacked => isAttacked;


    [Header("Events")]
    public UnityEvent onDataModelChanged = new UnityEvent();

    private List<UnitModelActor> models = new List<UnitModelActor>();
}




/// <summary>
/// Lifecycle
/// </summary>
public partial class UnitActor
{
    // Awake
    private void Awake()
    {
        if (selectionCollider == null) selectionCollider = GetComponent<SphereCollider>();
        ResetAppearance();

        EndTurn();
    }

    // OnEnable
    private void OnEnable()
    {
        UnitManager.Shared.Add(this);
    }

    // OnDisable
    private void OnDisable()
    {
        UnitManager.Shared.Remove(this);
        onDataModelChanged.RemoveAllListeners();
    }
}




/// <summary>
/// Public Actions
/// </summary>
public partial class UnitActor
{
    /// <summary>
    /// Update actor with new data model
    /// </summary>
    /// <param name="dataModel">Data model</param>
    public void SetDataModel(UnitDataModel dataModel)
    {
        this.dataModel = dataModel;
        ResetAppearance();
    }

    /// <summary>
    /// Update unit's team ID. Units with a same team are allies and take turns simultaneously.
    /// </summary>
    /// <param name="teamID">Team ID</param>
    public void SetTeamID(int teamID)
    {
        this.teamID = teamID;
    }

    /// <summary>
    /// Sets target pose through desired position. Clamps if necessary.
    /// </summary>
    /// <param name="targetPosition"></param>
    public void SetTargetMovePosition(Vector3 desiredPosition)
    {
        var lookVector = desiredPosition - currentPose.position;
        var lookRotation = Quaternion.LookRotation(lookVector, Vector3.up);

        // Calculate angleDelta
        var angle = Quaternion.Angle(currentPose.rotation, lookRotation);
        var angleDelta = Mathf.Clamp(angle / dataModel.MoveAngle * 2f, 0f, 1f);
        var localDesiredPosition = transform.InverseTransformPoint(desiredPosition);
        if (localDesiredPosition.x < 0) angleDelta *= -1f;

        // Calculate distanceDelta
        var distance = lookVector.magnitude;
        var distanceDelta = Mathf.Clamp01(distance / dataModel.MoveDistance);
        if (localDesiredPosition.z < 0) distanceDelta *= 0f;

        // Calculate targetPosition on deltas
        var positionEuler = new Vector3
        {
            x = 0,
            y = currentPose.rotation.eulerAngles.y + dataModel.MoveAngle / 2f * angleDelta - 90,
            z = 0
        };

        var targetPosition = Quaternion.Euler(positionEuler) * Vector3.right * dataModel.MoveDistance * distanceDelta + currentPose.position;

        // Calculate targetRotation on deltas
        var targetEuler = new Vector3
        {
            x = 0,
            y = positionEuler.y + 90 + dataModel.TurnAngleOnMove * angleDelta,
            z = 0,
        };

        targetPose = new Pose(targetPosition, Quaternion.Euler(targetEuler));
    }

    /// <summary>
    /// Returns pose for any position on current move trajectory
    /// </summary>
    /// <param name="value">Value from 0 to 1</param>
    public Pose EvaluateTrajectory(float value)
    {
        // Points
        var a = currentPose.position;
        var d = targetPose.position;

        var distance = (d - a).magnitude;

        var b = a + currentPose.forward * distance / 3f;
        var c = d - targetPose.forward * distance / 3f;


        var evaluatedPosition = BesierHelper.CubeLerp(a, b, c, d, Mathf.Clamp01(value));

        var ra = currentPose.rotation;
        var rd = targetPose.rotation;

        var evaluatedRotation = Quaternion.Lerp(QuaternionHelper.GetValid(ra), QuaternionHelper.GetValid(rd), value) ;
        return new Pose(evaluatedPosition, evaluatedRotation);
    }

    /// <summary>
    /// Executes movement action
    /// </summary>
    public void FlagIsMoved()
    {
        isMoved = true;
    }

    public void FlagIsAttacked()
    {
        isAttacked = true;
    }

    /// <summary>
    /// Activates attack animation
    /// </summary>
    /// <param name="position"></param>
    public void AttackOnPosition(Vector3 position)
    {

    }

    /// <summary>
    /// Ends turn of a unit. Refreshes it's actions and poses
    /// </summary>
    public void EndTurn()
    {
        isMoved = false;
        isAttacked = false;
        isBusy = false;
        ResetCurrentPose();
        ResetTargetPose();
    }

    public void ResetCurrentPose()
    {
        currentPose = new Pose(transform.position, transform.rotation);
    }

    public void ResetTargetPose()
    {
        targetPose = currentPose;
    }

    /// <summary>
    /// Update actor with damage taken from enemy
    /// </summary>
    /// <param name="damage"></param>
    public void SetDamage(int damage)
    {
        var remainingDamage = damage;
        while (remainingDamage > 0)
        {
            var randomModel = models.FindAll(c => c.CurrentHealth > 0)[(int)(Random.value * models.Count)];
            randomModel.SetDamage(1);
            currentHealth -= 1;
            remainingDamage -= 1;
            Debug.Log($"{name} is taking 1 damage. {remainingDamage} reamins to take");
            if (currentHealth < 1) break;
        }
    }
}




/// <summary>
/// Private Actions
/// </summary>
public partial class UnitActor
{
    [ContextMenu("Reset Appearance")]
    private void ResetAppearance()
    {
        if (dataModel == null) return;

        // Set GameObject Name
        gameObject.name = $"{dataModel.UnitName}, Team: {TeamID}";

        // Set health
        currentHealth = dataModel.UnitHealth;

        // Set Collider
        selectionCollider.center = dataModel.SphereColliderOrigin;
        selectionCollider.radius = dataModel.SphereColliderRadius;

        // Clean models
        foreach (var model in models)
        {
            if (Application.isEditor) DestroyImmediate(model.gameObject);
            else Destroy(model.gameObject);
        }
        models.Clear();

        // Create new models
        var squadOffset = Vector3.zero;
        var isEven = false;
        for (int i = 0; i < dataModel.SquadSize; i++)
        {
            // Spawn model
            var model = Instantiate(dataModel.Prefab, transform, true);
            models.Add(model);

            // Health
            model.SetMaxHealth(dataModel.SquadModelHealth);

            // Position in formation
            model.transform.localPosition = squadOffset;
            model.transform.localRotation = Quaternion.identity;

            // Calculate next offset
            squadOffset = new Vector3(-squadOffset.x, squadOffset.y, squadOffset.z);
            if (!isEven)
            {
                squadOffset += dataModel.SquadFormationOffset;
            }
            isEven = !isEven;
        }
    }

    private IEnumerator MoveRoutine()
    {
        isBusy = true;
        var value = 0f;
        while (value < 1f)
        {
            var pose = EvaluateTrajectory(value);
            transform.position = pose.position;
            transform.rotation = pose.rotation;

            yield return new WaitForEndOfFrame();
            value += Time.deltaTime / dataModel.MoveSpeedSeconds;
        }
        transform.position = targetPose.position;
        transform.rotation = targetPose.rotation;
        currentPose = new Pose(transform.position, transform.rotation);
        targetPose = currentPose;

        isBusy = false;
    }
}




/// <summary>
/// Selection detection
/// </summary>
public partial class UnitActor
{
    //private void OnMouseUp()
    //{
    //    if (!isBusy)
    //        UnitManager.Shared.RegisterSelection(this);
    //}
}




/// <summary>
/// Gizmos
/// </summary>
public partial class UnitActor
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        var a = currentPose.position;
        var d = targetPose.position;

        var distance = (d - a).magnitude;

        var b = a + currentPose.forward * distance / 3f;
        var c = d - targetPose.forward * distance / 3f;

        Gizmos.DrawLine(a, b);
        Gizmos.DrawLine(b, c);
        Gizmos.DrawLine(c, d);
    }
}
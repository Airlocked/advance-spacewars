using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Fighter,
    Bomber,
    Interceptor,
    Capital
}

[CreateAssetMenu(fileName = "Unit Data Model", menuName = "Units/Unit Data Model", order = 0)]
public class UnitDataModel : ScriptableObject
{
    [Header("Title")]
    [SerializeField] private string unitName;
    public string UnitName => unitName;

    [Header("Health and size")]
    [SerializeField] private UnitType type;
    public UnitType Type => type;

    [SerializeField] private int unitHealth;
    public int UnitHealth => unitHealth;

    [SerializeField] private int squadModelHealth;
    public int SquadModelHealth => squadModelHealth;

    [SerializeField] private int squadSize;
    public int SquadSize => squadSize;

    [Header("Formation and model")]
    [SerializeField] private Vector3 squadFormationOffset;
    public Vector3 SquadFormationOffset => squadFormationOffset;

    [SerializeField] private UnitModelActor prefab;
    public UnitModelActor Prefab => prefab;

    [Header("Collider Data")]
    [SerializeField] Vector3 sphereColliderOrigin;
    public Vector3 SphereColliderOrigin => sphereColliderOrigin;

    [SerializeField] float sphereColliderRadius;
    public float SphereColliderRadius => sphereColliderRadius;

    [Header("Movement Data")]
    [SerializeField] private float moveDistance;
    public float MoveDistance => moveDistance;

    [SerializeField] private float moveAngle;
    public float MoveAngle => moveAngle;

    [SerializeField] private float turnAngleOnMove;
    public float TurnAngleOnMove => turnAngleOnMove;

    [SerializeField] private float moveSpeedSeconds = 1;
    public float MoveSpeedSeconds => moveSpeedSeconds;

    [Header("Attack Data")]
    [SerializeField] private float attackRadius;
    public float AttackRadius => attackRadius;

    [Header("Weapons")]
    [SerializeField] private List<WeaponDataModel> weapons = new List<WeaponDataModel>();
    public List<WeaponDataModel> Weapons => weapons;
}

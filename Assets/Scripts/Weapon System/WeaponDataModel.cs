using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponArcType
{
    Front,
    DoubleSided,
    Back
}

[CreateAssetMenu(fileName = "Weapon Data Model", menuName = "Units/Weapon Data Model", order = 1)]
public class WeaponDataModel : ScriptableObject
{
    [Header("Visual Data")]
    [SerializeField] private string weaponName;
    public string WeaponName => weaponName;

    [Header("Weapon Data")]
    [SerializeField] private int damage;
    public int Damage => damage;

    [SerializeField] private List<UnitType> targetTypes = new List<UnitType>();
    public List<UnitType> TargetTypes => targetTypes;

    [Space]
    [SerializeField] private float minDistance;
    public float MinDistance => minDistance;

    [SerializeField] private float maxDistance;
    public float MaxDistance => maxDistance;

    [SerializeField] private float arcAngle;
    public float ArcAngle => arcAngle;

    [SerializeField] private WeaponArcType arcType;
    public WeaponArcType ArcType => arcType;
}

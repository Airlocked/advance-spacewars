using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitSelectionEvent: UnityEvent<UnitActor>
{

}

/// <summary>
/// Global unit manager.
/// </summary>
public partial class UnitManager
{
    // Singleton
    private static UnitManager shared;
    public static UnitManager Shared
    {
        get
        {
            if (shared == null) shared = new UnitManager();
            return shared;
        }
    }

    // Unit list
    public List<UnitActor> units = new List<UnitActor>();

    // Unit selection
    public UnitActor SelectedUnit { get; set; }
    public UnitActor TargetUnit { get; set; }
}

/// <summary>
/// Public Actions
/// </summary>
public partial class UnitManager
{
    public void Add(UnitActor unit)
    {
        if (units.Contains(unit)) return;
        units.Add(unit);

        Debug.Log($"Unit Manager: Adding {unit.name}");
        Debug.Log($"Unit Manager: Unit Count {units.Count}");
    }

    public void Remove(UnitActor unit)
    {
        if (!units.Contains(unit)) return;
        units.Remove(unit);
        Debug.Log($"Unit Manager: Removing {unit.name}");
        Debug.Log($"Unit Manager: Unit Count {units.Count}");
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Renders unit overlay icons
/// </summary>
public partial class UnitIconManagerActor : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private UnitIconActor iconPrefab;

    private List<UnitIconActor> icons = new List<UnitIconActor>();
}

/// <summary>
/// Lifecycle
/// </summary>
public partial class UnitIconManagerActor
{
    private void Update()
    {
        UpdateIcons();
    }
}

/// <summary>
/// Private actions
/// </summary>
public partial class UnitIconManagerActor
{
    private void UpdateIcons()
    {
        var units = UnitManager.Shared.units;

        while (icons.Count > units.Count)
        {
            var icon = icons[0];
            Destroy(icon.gameObject);
            icons.Remove(icon);
        }

        while (icons.Count < units.Count)
        {
            var icon = Instantiate(iconPrefab, transform, true);
            icons.Add(icon);
        }

        for (int i = 0; i < units.Count; i++)
        {
            icons[i].SetUnit(units[i]);
        }
    }
}

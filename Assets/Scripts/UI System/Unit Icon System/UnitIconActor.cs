using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// HUD Unit Icon
/// </summary>
[RequireComponent(typeof(RectTransform))]
public partial class UnitIconActor : MonoBehaviour
{
    [Header("Icon Data")]
    [SerializeField] private UnitActor unit;
    public UnitActor Unit => unit;

    [Header("Elements")]
    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text nameText;

    [Header("Positioning")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
}

/// <summary>
/// Lifecycle
/// </summary>
public partial class UnitIconActor
{
    private void Awake()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        if (canvas == null) canvas = GetComponentInParent<Canvas>();
    }

    private void LateUpdate()
    {
        UpdatePosition();
        UpdateTexts();
    }
}

/// <summary>
/// Public actions
/// </summary>
public partial class UnitIconActor
{
    public void SetUnit(UnitActor unit)
    {
        this.unit = unit;
    }
}

/// <summary>
/// Private actions
/// </summary>
public partial class UnitIconActor
{
    private void UpdatePosition()
    {
        if (unit == null) return;
        var unitCenter = unit.transform.position + unit.transform.forward * unit.SelectionCollider.center.z;
        var screenPosition = Camera.main.WorldToScreenPoint(unitCenter);
        var position = new Vector2
        {
            x = screenPosition.x,
            y = screenPosition.y
        };
        rectTransform.anchoredPosition = position / canvas.scaleFactor;
    }

    private void UpdateTexts()
    {
        healthText.text = $"{unit.CurrentHealth}";
        nameText.text = unit.DataModel.UnitName;
    }
}
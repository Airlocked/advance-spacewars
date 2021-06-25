using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDetailsManager : MonoBehaviour
{
    [SerializeField] bool isSelected = true;

    [SerializeField] private TMPro.TMP_Text unitNameText;
    [SerializeField] private TMPro.TMP_Text unitHealthText;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        var unit = isSelected ? UnitManager.Shared.SelectedUnit : UnitManager.Shared.TargetUnit;
        if (unit == null)
        {
            rectTransform.anchoredPosition = Vector2.up * -300;
            return;
        }
        else
        {
            rectTransform.anchoredPosition = Vector2.zero;
            unitNameText.text = unit.DataModel.UnitName;
            unitHealthText.text = $"{unit.CurrentHealth}/{unit.DataModel.UnitHealth}";
        }
    }
}

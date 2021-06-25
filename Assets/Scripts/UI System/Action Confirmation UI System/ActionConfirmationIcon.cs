using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Displays single confirmation pointer
/// </summary>
[RequireComponent(typeof(RectTransform))]
public partial class ActionConfirmationIcon : MonoBehaviour
{
    [Header("Elements")]
    public Button ConfirmButton;
    public TMPro.TMP_Text buttonText;

    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector3 worldPosition;

    private UnityAction buttonAction;
}

/// <summary>
/// Lifecycle
/// </summary>
public partial class ActionConfirmationIcon
{
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void OnEnable()
    {
        ConfirmButton.onClick.AddListener(buttonAction);
    }

    private void Update()
    {
        var localPosition = Camera.main.WorldToScreenPoint(worldPosition);
        rectTransform.anchoredPosition = localPosition / canvas.scaleFactor;
    }

    private void OnDisable()
    {
        ConfirmButton.onClick.RemoveAllListeners();
    }
}

/// <summary>
/// Public actions
/// </summary>
public partial class ActionConfirmationIcon
{
    public void SetButtonAction(UnityAction action)
    {
        buttonAction = action;

    }

    public void Show(Vector3 worldPosition)
    {
        this.worldPosition = worldPosition;
        if (!isActiveAndEnabled)  gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (isActiveAndEnabled) gameObject.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays turn data
/// </summary>
public partial class TurnOrderManager : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private Button endTurnButton;
}

/// <summary>
/// Lifecycle
/// </summary>
public partial class TurnOrderManager
{
    private void OnEnable()
    {
        endTurnButton.onClick.AddListener(OnEndTurnClick);
    }

    private void OnDisable()
    {
        endTurnButton.onClick.RemoveListener(OnEndTurnClick);
    }
}

/// <summary>
/// Private actions
/// </summary>
public partial class TurnOrderManager
{
    private void OnEndTurnClick()
    {
        gameController.EndTurn();
    }
}

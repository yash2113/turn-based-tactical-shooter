using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Class responsible for managing the UI related to turns in the game
public class TurnSystemUI : MonoBehaviour
{
    [SerializeField]
    private Button endTurnBtn;
    [SerializeField]
    private TextMeshProUGUI turnNumberText;
    [SerializeField]
    private GameObject enemyTurnVisualGameObject;

    private void Start()
    {
        endTurnBtn.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;


        UpdateTurnNumber();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    // Event handler for the TurnSystem's OnTurnChanged event
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnNumber();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    // Method to update the text displaying the current turn number
    private void UpdateTurnNumber()
    {
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }

    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    // Method to update the visibility of the end turn button based on whether it's the player's turn
    private void UpdateEndTurnButtonVisibility()
    {
        endTurnBtn.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }

}

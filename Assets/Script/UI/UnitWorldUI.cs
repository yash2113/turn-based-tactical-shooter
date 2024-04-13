using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Class responsible for managing the UI elements of a unit in the world
public class UnitWorldUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI actionPointsText;
    [SerializeField]
    private Unit unit;
    [SerializeField]
    private Image healthBarImage;
    [SerializeField]
    private HealthSystem healthSystem;

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        UpdateActionPointsText();
        UpdateHealthBar();
    }

    // Event handler for the health system's OnDamaged event
    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, System.EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized(); 
    }


}

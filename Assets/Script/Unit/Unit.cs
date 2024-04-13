using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private const int ACTION_POINT_MAX = 100;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private BaseAction[] baseActionArray;
    private AudioSource unitAudioSource;
    private int actionPoint = ACTION_POINT_MAX;

    [SerializeField]
    private bool isEnemy;// Flag indicating if the unit is an enemy

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetComponents<BaseAction>();
        unitAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);

    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            //Unit Changed Grid Position
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);  
        }
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach(BaseAction baseAction in baseActionArray)
        {
            if(baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoint(baseAction.GetActionsPointCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    // Method to check if the unit can spend action points to take an action
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(actionPoint >= baseAction.GetActionsPointCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoint(int amount)
    {
        actionPoint -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return actionPoint;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if((IsEnemy() && (!TurnSystem.Instance.IsPlayerTurn())) || 
            (!IsEnemy() && (TurnSystem.Instance.IsPlayerTurn())))
        {
            actionPoint = ACTION_POINT_MAX;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }

    public void SetUnitActive(bool set)
    {
        gameObject.SetActive(set);
    }

    public void SetAudioClip( AudioClip clip)
    {
        unitAudioSource.clip = clip;
    }

    public void PlayAudioClip()
    {
        unitAudioSource.Play();
        
    }

    public void SetAudioLoop( bool loop)
    {
        unitAudioSource.loop = loop;
    }

    public void StopAudioClip()
    {
        unitAudioSource.Stop();
    }

}

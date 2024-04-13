using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract class representing a base action that can be performed by a unit
public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    // Abstract method to get the name of the action
    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    // Virtual method to check if a grid position is a valid target for the action
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionsPointCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;

        // Trigger event indicating the action has started
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();
        // Trigger event indicating the action has completed
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    // Method to get the unit performing the action
    public Unit GetUnit()
    {
        return unit;
    }

    // Method to get the best enemy AI action for a given grid position
    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();
        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

        // Iterate through valid action grid positions and get the enemy AI action for each
        foreach (GridPosition gridPosition in  validActionGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }

        // Sort the enemy AI action list by action value and return the best action
        if (enemyAIActionList.Count > 0)
        {
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyAIActionList[0];
        }
        else
        {
            return null;
        }

    }

    // Abstract method to get the enemy AI action for a given grid position
    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);

}

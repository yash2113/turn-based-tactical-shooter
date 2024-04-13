using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class representing a grenade action, derived from BaseAction
public class GrenadeAction : BaseAction
{
    [SerializeField]
    private Transform grenadeProjectilePrefab;
    [SerializeField]
    private AudioClip grenadeSound;

    private int maxThrowDistance = 7;

    private void Update()
    {
        if(!isActive)
        {
            return;
        }
    }

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
    // Method to get a list of valid target grid positions for the action
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxThrowDistance)
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }


        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        // Instantiate the grenade projectile and set up its behavior
        Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviourComplete);


        Debug.Log("Grenade Action");
        ActionStart(onActionComplete);
    }

    // Method called when the grenade projectile behavior is complete
    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
        unit.SetAudioClip(grenadeSound);
        unit.PlayAudioClip();
    }
}

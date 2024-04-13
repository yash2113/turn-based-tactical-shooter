using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class representing a move action, derived from BaseAction
public class MoveActions : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    //Pre defined private variables
    private string IsWalking = "IsWalking";

    [SerializeField]
    private int maxMoveDistance = 4;
    [SerializeField]
    private AudioClip moveSound;

    private List<Vector3> positionList;
    private int currentPositionIndex;

    private void Update()
    {
        if(!isActive)
        {
            return;
        }

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float rotationSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);

        //Check if the Unit should be walking or in idle state
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            //Pre defined values
            float moveSpeed = 4f;

            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            // Move to the next position if available, otherwise stop moving
            currentPositionIndex++;
            if(currentPositionIndex >= positionList.Count)
            {
                unit.SetAudioLoop(false); 
                unit.StopAudioClip();
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
            
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = Pathfinding.Instance.FindPath(unit.GetGridPosition(),gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        // Convert grid positions to world positions for movement
        foreach (GridPosition pathGridPosition  in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }
        unit.SetAudioClip(moveSound);
        unit.SetAudioLoop(true);
        unit.PlayAudioClip();
        OnStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    // Method to get a list of valid target grid positions for the action
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        // Iterate through a range of grid positions around the unit to determine valid move locations
        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if(unitGridPosition == testGridPosition)
                {
                    //unit is at same position
                    continue;
                }

                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    //Grid Already occupied by another unit
                    continue;
                }

                if(!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }

                if (!Pathfinding.Instance.HasPath(unitGridPosition, testGridPosition))
                {
                    continue;
                }

                int pathFindingDistanceMultiplier = 10;
                if(Pathfinding.Instance.GetPathLength(unitGridPosition,testGridPosition) > maxMoveDistance * pathFindingDistanceMultiplier)
                {
                    //Path length is too long
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }


        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

        // Return an enemy AI action with the grid position and its value based on the target count
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }


}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// Class representing a spinning action, derived from BaseAction
public class SpinAction : BaseAction
{
    //Could also define a delegate or use in build delegates like action or func<>
    //public delegate void SpinCompleteDelegate();

    private float totalSpinAmount;
    [SerializeField]
    private int SpinActionPointsCost = 2;
    [SerializeField]
    private AudioClip spinFunnyClip;

    private void Update()
    {

        if (!isActive)
        {
            return;
        }

        // Play spinning audio clip
        unit.SetAudioClip(spinFunnyClip);
        unit.PlayAudioClip();

        // Calculate spin amount and update rotation
        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        totalSpinAmount += spinAddAmount;
        if(totalSpinAmount >= 360f)
        {
            ActionComplete();
        }

    }
   
    public override void TakeAction(GridPosition gridPosition ,Action onActionComplete)
    {
        totalSpinAmount = 0;
        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    // Method to get a list of valid target grid positions for the action
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition// The spin action is valid at the unit's current grid position
        };
    }

    public override int GetActionsPointCost()
    {
        return SpinActionPointsCost;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

}

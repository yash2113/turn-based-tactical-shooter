using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    private const string IsOpen = "IsOpen";

    private Animator animator;
    private GridPosition gridPosition;
    [SerializeField]
    private InteractSphere interactable;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        interactable.OnSphereTurnedRed += Interactable_OnSphereTurnedRed;

        if(interactable.gameObject == null)
        {
            Debug.Log("No need");
        }
        
    }

    private void Interactable_OnSphereTurnedRed(object sender, System.EventArgs e)
    {
        PillarDown();
    }

    private void PillarUp()
    {
        animator.SetBool(IsOpen, false);
    }

    private void PillarDown()
    {
        animator.SetBool(IsOpen, true);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }

    private void ReturnMoveGrid()
    {
        
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class responsible for updating pathfinding when crates are destroyed
public class PathfindingUpdater : MonoBehaviour
{
    private void Start()
    {
        // Subscribe to the OnAnyDestroyed event of DestructableCrate
        DestructableCrate.OnAnyDestroyed += DestructableCrate_OnAnyDestroyed;
    }

    private void DestructableCrate_OnAnyDestroyed(object sender, System.EventArgs e)
    {
        DestructableCrate destructableCrate = sender as DestructableCrate;
        // Set the grid position of the destroyed crate to walkable in the pathfinding system
        Pathfinding.Instance.SetIsWalkableGridPosition(destructableCrate.GetGridPosition(), true);
    }
}

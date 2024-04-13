using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// This class represents a debug object within a grid, displaying information using TextMeshPro.
public class GridDebugObject : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro textMeshPro;

    private object gridObject;

    public virtual void SetGridObject(object gridObject)
    {
        this.gridObject = gridObject;
    }

    protected virtual void Update()
    {
       textMeshPro.text = gridObject.ToString();
    }

}

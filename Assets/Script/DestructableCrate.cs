using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCrate : MonoBehaviour
{
    public static event EventHandler OnAnyDestroyed;

    [SerializeField] private Transform crateDestroyedPrefab;

    private GridPosition gridPosition;

    private void Start()
    {
        // Get the grid position of the crate when it starts
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetGridPosition() 
    {
        return gridPosition; 
    }

    // Method called to inflict damage to the crate
    public void Damage()
    {
        Transform crateDestroyedTransform = Instantiate(crateDestroyedPrefab, transform.position, transform.rotation);
        // Apply explosion force to the children of the destroyed crate visual effect
        ApplyExplosionToChildren(crateDestroyedTransform, 150f, transform.position, 10f);
        Destroy(gameObject);

        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    // Method to apply explosion force to the children of a given root transform
    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }

            // Recursively apply explosion force to the children of this child
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRadius);
        }
    }
}

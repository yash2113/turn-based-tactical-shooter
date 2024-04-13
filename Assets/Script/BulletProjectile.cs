using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer trailRenderer;
    [SerializeField]
    private Transform bulletHitVFXPrefab;

    private Vector3 targetPosition;

    // Method to set up the bullet projectile with a target position
    public void SetUp(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        // Calculate the direction in which the bullet should move
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        float moveSpeed = 200f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        // If the bullet has passed the target position, stop it at the target position
        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;

            trailRenderer.transform.parent = null;
            Destroy(gameObject);
            // Instantiate the bullet hit visual effect at the target position
            Instantiate(bulletHitVFXPrefab, targetPosition, Quaternion.identity);
        }
    }


}

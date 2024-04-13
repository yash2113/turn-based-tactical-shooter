using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField]
    private Transform grenadeExplodeVFXPrefab;
    [SerializeField]
    private TrailRenderer trailRenderer;
    [SerializeField]
    private AnimationCurve arcYAnimationCurve;

    private const int GRENADE_DAMAGE = 30;
    
    private Vector3 targetPosition;
    private Action onGrenadeBehaviourComplete;
    private float totalDistance;
    private Vector3 positonXZ;

    private void Update()
    {
        // Calculate movement direction towards the target position
        Vector3 moveDir = (targetPosition - positonXZ).normalized;
        float moveSpeed = 15f;

        // Update the position in the XZ plane
        positonXZ += moveDir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positonXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        // Calculate the height of the grenade based on the animation curve
        float maxHeight = totalDistance / 4f;
        float postionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positonXZ.x, postionY, positonXZ.z);

        float reachedTargetDistance = 0.2f;
        if(Vector3.Distance(positonXZ, targetPosition) < reachedTargetDistance )
        {
            Explode();

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

            //Visual effects
            trailRenderer.transform.parent = null;
            Instantiate(grenadeExplodeVFXPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);
            Destroy(gameObject);

            onGrenadeBehaviourComplete();
        }
    }

    // Method to perform explosion logic
    private void Explode()
    {
        float damageRadius = 4f;
        Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent<Unit>(out Unit targetUnit))
            {
                //Sound

                targetUnit.Damage(GRENADE_DAMAGE);
            }
            if (collider.TryGetComponent<DestructableCrate>(out DestructableCrate destructableCrate))
            {
                destructableCrate.Damage();
            }
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positonXZ = transform.position;
        positonXZ.y = 0f;

        totalDistance = Vector3.Distance(positonXZ, targetPosition);
    }

}

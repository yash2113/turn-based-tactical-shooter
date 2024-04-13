using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField]
    private Transform ragdollRootBone;
    [SerializeField]
    private float explosionForce = 300f;
    [SerializeField]
    private float explosionRadius = 10f;

    // Set up the ragdoll based on the original root bone and apply explosion force
    public void SetUp(Transform originalRootBone)
    {
        MatchAllChildTransforms(originalRootBone, ragdollRootBone);

        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        ApplyExplosionToRagdoll(ragdollRootBone, explosionForce, transform.position + randomDir, explosionRadius);

    }

    // Recursively match positions and rotations of child transforms
    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if(cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildTransforms(child,cloneChild);
            }    
        }
    }

    // Recursively apply explosion force to all rigidbodies in the ragdoll
    private void ApplyExplosionToRagdoll(Transform root , float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        foreach(Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);   
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRadius);
        }
    }


}

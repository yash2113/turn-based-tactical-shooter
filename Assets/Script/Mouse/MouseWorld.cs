using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// A utility class for obtaining the world position of the mouse cursor.
public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;

    [SerializeField]
    private LayerMask mousePlaneLayerMask;

    private void Awake()
    {
        instance = this;
    }

    public static Vector3 GetPosition()
    {
        // Create a ray from the camera through the mouse cursor position.
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask);

        // Return the point of collision with the mouse plane (world position of the mouse cursor).
        return raycastHit.point;
    }


}

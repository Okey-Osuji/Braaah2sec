
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    public float pushForce = 8f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.rigidbody;

        if (rb == null || rb.isKinematic)
            return;

        // Get the horizontal direction of movement
        Vector3 pushDirection = hit.moveDirection;
        pushDirection.y = 0f;
        pushDirection.Normalize();

        // Apply force at the point of impact
        rb.AddForceAtPosition(
            pushDirection * pushForce,
            hit.point,
            ForceMode.Force
        );
    }
}

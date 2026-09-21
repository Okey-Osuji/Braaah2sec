using UnityEngine;
public class PickUpObject : MonoBehaviour
{
    private Rigidbody rb;
    public bool IsHeld { get; private set; } // Lets goals ignore an object while the player is holding it.

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void PickUp(Transform holdPoint)
    {
        // Disable physics while the object is controlled by the hold point.
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Attach the object and align it exactly with the hold point.
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        IsHeld = true;
    }
    public void Drop()
    {
        // Detach first, then return control of the object to the physics system.
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;
        IsHeld = false;
    }
    public void MoveToHoldPoint(Vector3 targetPosition)
    {
        rb.MovePosition(targetPosition);
    }
    public void Throw(Vector3 impulse)
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        IsHeld = false;
        rb.AddForce(impulse, ForceMode.Impulse);
    }
}

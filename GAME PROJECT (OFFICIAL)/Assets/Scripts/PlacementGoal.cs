using UnityEngine;

// Completes the level when the required pickup item is placed upright in this trigger zone.
[RequireComponent(typeof(BoxCollider))]
public class PlacementGoal : MonoBehaviour
{
    [Header("Required Item")]
    [SerializeField] private PickUpObject requiredItem; // The only item that can complete this goal.

    [Header("Placement Rules")]
    [Range(0f, 90f)]
    [SerializeField] private float uprightTolerance = 15f; // Maximum allowed tilt from vertical, in degrees.
    [SerializeField] private float maximumSpeed = 0.15f;    // Item must be nearly still to count as placed.

    [Header("Completion")]
    [SerializeField] private EndScreen endScreen; // Existing end-screen controller to show on success.

    private bool completed; // Prevents the end screen from being shown more than once.

    private void Reset()
    {
        // A floor area detects objects only when its collider is a trigger.
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void Awake()
    {
        // Keep the setup safe if this component is added to an existing object.
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (completed || requiredItem == null)
        {
            return;
        }

        // Support colliders placed on a child object of the pickup item.
        PickUpObject itemInZone = other.GetComponentInParent<PickUpObject>();
        if (itemInZone != requiredItem || requiredItem.IsHeld)
        {
            return;
        }

        Rigidbody itemBody = requiredItem.GetComponent<Rigidbody>();
        if (itemBody == null || itemBody.linearVelocity.magnitude > maximumSpeed)
        {
            return;
        }

        // The object's local up direction must point close enough to world up.
        float uprightDotProduct = Vector3.Dot(requiredItem.transform.up, Vector3.up);
        float minimumUprightDotProduct = Mathf.Cos(uprightTolerance * Mathf.Deg2Rad);
        if (uprightDotProduct < minimumUprightDotProduct)
        {
            return;
        }

        CompleteGoal();
    }

    private void CompleteGoal()
    {
        completed = true;

        if (endScreen != null)
        {
            endScreen.Show();
        }
        else
        {
            Debug.LogWarning("PlacementGoal: Assign an EndScreen component in the Inspector.", this);
        }
    }
}

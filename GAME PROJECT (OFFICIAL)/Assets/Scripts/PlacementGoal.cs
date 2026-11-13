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
    [SerializeField] private Animator leftDoorAnimator;
    [SerializeField] private string leftDoorOpenTriggerName = "OpenDoor";
    [SerializeField] private Animator rightDoorAnimator;
    [SerializeField] private string rightDoorOpenTriggerName = "OpenDoor";

    private bool completed; // Prevents the door animation from being triggered more than once.

    private void Reset()
    {
        // A floor area detects objects only when its collider is a trigger.
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void Awake()
    {
        // Keeps the setup safe if this component is added to an existing object.
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

        TriggerDoor(leftDoorAnimator, leftDoorOpenTriggerName, "left");
        TriggerDoor(rightDoorAnimator, rightDoorOpenTriggerName, "right");
    }

    private void TriggerDoor(Animator doorAnimator, string triggerName, string doorLabel)
    {
        if (doorAnimator == null || string.IsNullOrWhiteSpace(triggerName))
        {
            Debug.LogWarning($"PlacementGoal: Assign the {doorLabel} door Animator and its open trigger name in the Inspector.", this);
            return;
        }

        foreach (AnimatorControllerParameter parameter in doorAnimator.parameters)
        {
            if (parameter.name == triggerName && parameter.type == AnimatorControllerParameterType.Trigger)
            {
                doorAnimator.SetTrigger(triggerName);
                return;
            }
        }

        Debug.LogWarning($"PlacementGoal: The {doorLabel} door Animator has no Trigger parameter named '{triggerName}'. Check its Animator Controller.", this);
    }
}

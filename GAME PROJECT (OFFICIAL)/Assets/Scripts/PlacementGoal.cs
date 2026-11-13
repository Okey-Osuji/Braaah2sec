using UnityEngine;

// Completes the level when the required pickup item enters this trigger zone.
[RequireComponent(typeof(BoxCollider))]
public class PlacementGoal : MonoBehaviour
{
    [Header("Required Item")]
    [SerializeField] private PickUpObject requiredItem; // The only item that can complete this goal.

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
        // Any overlap by the required item counts, whether it is held or released.
        PickUpObject itemInZone = other.GetComponentInParent<PickUpObject>();
        if (itemInZone != requiredItem)
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

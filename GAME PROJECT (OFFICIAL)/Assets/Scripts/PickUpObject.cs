using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [Header("Proximity Particle Effect")]
    [SerializeField] private Transform player;
    [SerializeField] private float particleRange = 3f;
    [SerializeField] private ParticleSystem auraParticles;

    [Header("Drop Safety")]
    [SerializeField] private float dropSurfaceProbeHeight = 10f;
    [SerializeField] private float dropClearance = 0.05f;

    private Rigidbody rb;

    public bool IsHeld { get; private set; } // Lets goals ignore an object while the player is holding it

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Finds a Particle System attached to this item or one of its children
        if (auraParticles == null)
        {
            auraParticles = GetComponentInChildren<ParticleSystem>();
        }

        if (auraParticles != null)
        {
            auraParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    // Update method to track whenever player comes near mesh, which truggers particle effects on or off
    private void Update()
    {
        if (player == null || auraParticles == null)
        {
            return;
        }

        bool playerIsNear = !IsHeld && Vector3.Distance(transform.position, player.position) <= particleRange;

        if (playerIsNear && !auraParticles.isPlaying)
        {
            auraParticles.Play(true);
        }
        else if (!playerIsNear && auraParticles.isPlaying)
        {
            auraParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public void PickUp(Transform holdPoint)
    {
        // Disable physics while the object is controlled by the hold point
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Attaches the object and aligns it exactly with the hold point
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        IsHeld = true;
    }

    public void Drop()
    {
        // Remember the held object's horizontal drop location before detaching.
        Vector3 dropPosition = transform.position;
        transform.SetParent(null);
        Physics.SyncTransforms();

        // If the camera is pointed down, the HoldPoint can be below the floor.
        // Find the nearest upward-facing surface below that location and place
        // the item above it before making the Rigidbody dynamic again.
        Collider itemCollider = GetComponent<Collider>();
        if (itemCollider != null && TryFindDropSurface(dropPosition, out RaycastHit surfaceHit))
        {
            float colliderCenterOffset = itemCollider.bounds.center.y - rb.position.y;
            float centerHeightAboveSurface = itemCollider.bounds.extents.y - colliderCenterOffset + dropClearance;
            rb.position = surfaceHit.point + Vector3.up * centerHeightAboveSurface;
        }

        // Auto Sync Transforms is disabled in this project. Register the final
        // collider pose before switching this Rigidbody back to dynamic mode.
        Physics.SyncTransforms();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.WakeUp();
        IsHeld = false;
    }

    private bool TryFindDropSurface(Vector3 position, out RaycastHit surfaceHit)
    {
        Vector3 rayOrigin = position + Vector3.up * dropSurfaceProbeHeight;
        RaycastHit[] hits = Physics.RaycastAll(rayOrigin, Vector3.down, dropSurfaceProbeHeight * 2f,
            Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.transform == transform || hit.collider.transform.IsChildOf(transform))
            {
                continue;
            }

            // Ignore walls and undersides; the item needs a surface that can support it.
            if (hit.normal.y > 0.5f)
            {
                surfaceHit = hit;
                return true;
            }
        }

        surfaceHit = default;
        return false;
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

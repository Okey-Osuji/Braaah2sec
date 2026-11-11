using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [Header("Proximity Particle Effect")]
    [SerializeField] private Transform player;
    [SerializeField] private float particleRange = 3f;
    [SerializeField] private ParticleSystem auraParticles;

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
        // Detaches first, then return control of the object to the physics system
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

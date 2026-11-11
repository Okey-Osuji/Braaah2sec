
using UnityEngine;

public class PaperPhysics : MonoBehaviour
{
    public float pushForce = 0.8f;
    public float liftForce = 0.3f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerStay(Collider other)
    {
        CharacterController player =
            other.GetComponent<CharacterController>();

        if (player == null)
            player = other.GetComponentInParent<CharacterController>();

        if (player == null || rb == null)
            return;

        float speed = player.velocity.magnitude;

        if (speed < 0.2f)
            return;

        Vector3 direction = transform.position - other.transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        direction.Normalize();

        rb.AddForce(
            direction * pushForce * speed +
            Vector3.up * liftForce * speed,
            ForceMode.Force
        );

        rb.AddTorque(
            Random.insideUnitSphere * 0.05f * speed,
            ForceMode.Force
        );
    }
}

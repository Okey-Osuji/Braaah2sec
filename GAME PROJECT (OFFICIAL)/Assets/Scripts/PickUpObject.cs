using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [Header("Proximity Highlight")]
    [SerializeField] private Transform player;
    [SerializeField] private float highlightRange = 3f;
    [SerializeField] private Color highlightColor = Color.green;

    private Rigidbody rb;
    private Renderer[] renderers;
    private Color[] originalColors;

    public bool IsHeld { get; private set; } // Lets goals ignore an object while the player is holding it.

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Include renderer components on child objects, if the item uses a nested model.
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            Material material = renderers[i].material;
            originalColors[i] = material.HasProperty("_BaseColor")
                ? material.GetColor("_BaseColor") // URP Lit material.
                : material.color;                  // Built-in Standard material.
        }
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        bool playerIsNear = !IsHeld && Vector3.Distance(transform.position, player.position) <= highlightRange;

        for (int i = 0; i < renderers.Length; i++)
        {
            Material material = renderers[i].material;
            Color targetColor = playerIsNear ? highlightColor : originalColors[i];

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", targetColor);
            }
            else
            {
                material.color = targetColor;
            }
        }
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

using UnityEngine;
using UnityEngine.InputSystem; // Needed for Mouse.current.

public class KeyClicker : MonoBehaviour
{
    [SerializeField] private float reach = 3f; // How far the player can press from.

    private void Update()
    {
        // Check that a mouse exists, then detect a left click this frame.
        if (Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame) return;

        // Cast a ray from the camera straight ahead, where the crosshair is.
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, reach))
        {
            PasswordKey key = hit.collider.GetComponentInParent<PasswordKey>();
            if (key != null) key.Press();
        }
    }
}
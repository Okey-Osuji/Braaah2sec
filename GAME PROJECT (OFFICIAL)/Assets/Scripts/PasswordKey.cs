using UnityEngine;

public class PasswordKey : MonoBehaviour
{
    [SerializeField] private Password password;              // Password manager that receives this cube's digit
    [SerializeField] [Range(0, 9)] private int digit;        // Number represented by this keypad cube

    private void Awake()
    {
        // Use an assigned manager first; otherwise find the first Password object in the scene.
        if (password == null)
        {
            password = FindFirstObjectByType<Password>();
        }
    }

    // Called by PlayerControls after the new Input System raycast hits this cube.
    public void Press()
    {
        // The cube must know which Password manager to send its digit to.
        if (password == null)
        {
            Debug.LogWarning("PasswordKey: No Password component has been assigned.", this);
            return;
        }

        // Convert the configured number to text and send it when this cube is clicked.
        password.EnterDigit(digit.ToString());
    }
}

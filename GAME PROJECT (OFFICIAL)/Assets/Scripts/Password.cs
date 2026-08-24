using UnityEngine;
using UnityEngine.Events;

public class Password : MonoBehaviour
{
    // The fixed seven-digit passcode the player must enter in order.
    private const string CorrectPasscode = "7452619";

    // Sound settings assigned in the Unity Inspector.
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;       // Component used to play the keypad sounds
    [SerializeField] private AudioClip buttonClickSound;     // Sound played for every key press
    [SerializeField] private AudioClip wrongPasscodeSound;   // Buzzer played when the player enters a wrong digit

    // Events that can be connected to other actions in the Unity Inspector.
    [Header("Events")]
    [SerializeField] private UnityEvent onPasscodeAccepted;  // Called after correctly entering 7452619
    [SerializeField] private UnityEvent onPasscodeRejected;  // Called as soon as an incorrect digit is entered

    private string enteredPasscode = string.Empty;            // Digits entered during the current attempt
    private EndScreen endScreen;                              // Screen shown after the correct passcode is entered
    private bool puzzleCompleted;                             // Stops extra keypad input after success

    private void Awake()
    {
        // Find the end-screen controller attached to the password manager.
        endScreen = GetComponent<EndScreen>();
        if (endScreen == null)
        {
            Debug.LogWarning("Password: Add an EndScreen component to this object and assign its UI panel.", this);
        }
    }

    // Called by a PasswordKey cube, passing the digit assigned to that cube.
    public void EnterDigit(string digit)
    {
        // Do not accept more keypad input once the puzzle has been completed.
        if (puzzleCompleted)
        {
            return;
        }

        // Ignore invalid calls that do not provide exactly one numeric digit.
        if (string.IsNullOrEmpty(digit) || digit.Length != 1 || !char.IsDigit(digit[0]))
        {
            Debug.LogWarning("Password: EnterDigit requires exactly one numeric digit.", this);
            return;
        }

        // Play feedback and add the selected key to the player's current attempt.
        PlaySound(buttonClickSound);
        enteredPasscode += digit;
        Debug.Log($"Passcode entered: {enteredPasscode}");

        // Wait until all seven digits have been entered before checking the passcode.
        if (enteredPasscode.Length < CorrectPasscode.Length)
        {
            return;
        }

        // The full code is wrong, so play the buzzer and start a new attempt.
        if (enteredPasscode != CorrectPasscode)
        {
            Debug.Log("Incorrect passcode.");
            PlaySound(wrongPasscodeSound);
            onPasscodeRejected?.Invoke();
            enteredPasscode = string.Empty;
            return;
        }

        // The full code has been entered correctly.
        if (enteredPasscode == CorrectPasscode)
        {
            Debug.Log("Passcode accepted.");
            puzzleCompleted = true;
            if (endScreen != null)
            {
                endScreen.Show();
            }
            onPasscodeAccepted?.Invoke();
        }
    }

    private void PlaySound(AudioClip clip)
    {
        // Play the supplied clip only if both the audio player and clip are assigned.
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}

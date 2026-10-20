using UnityEngine;


public class HintPopup : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject cluePanel;       // Panel containing the clue text and return button

    [Header("Timing")]
    [SerializeField] private float delayInSeconds = 120f; // Two minutes by default

    private float elapsedTime;                            // Time spent trying to solve the password
    private bool timerRunning = true;                     // Whether the clue timer should keep counting
    private bool clueShown;                               // Ensures the clue appears only once

    private void Awake()
    {
        // Always hides the clue panel when the scene begins.
        if (cluePanel != null)
        {
            cluePanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Stops checking once the clue has appeared or the password has been solved.
        if (!timerRunning || clueShown)
        {
            return;
        }

        // Count only while the game is running.
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= delayInSeconds)
        {
            ShowClue();
        }
    }

    // Displays the clue panel after the timed delay.
    private void ShowClue()
    {
        clueShown = true;

        if (cluePanel == null)
        {
            Debug.LogWarning("HintPopup: Assign a clue panel in the Inspector.", this);
            return;
        }

        cluePanel.SetActive(true);
        Time.timeScale = 0f; // Pause gameplay until the player closes the clue.
    }

    // Assign this method to the clue button's On Click event to resume the game.
    public void ReturnToGame()
    {
        if (cluePanel != null)
        {
            cluePanel.SetActive(false);
        }

        Time.timeScale = 1f; // Resume normal gameplay after closing the clue.
    }

    // Called by Password when the player solves the passcode before the clue is needed.
    public void StopTimer()
    {
        timerRunning = false;
    }
}

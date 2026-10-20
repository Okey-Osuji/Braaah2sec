using UnityEngine;
using UnityEngine.SceneManagement;


public class EndScreen : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject endScreenPanel; // Panel containing the WELL DONE message and Exit button

    [Header("Scene")]
    [SerializeField] private string gameSceneName = "Main Menu Version 1( Stanley Parable Version)";

    // Called by Password after the player enters the correct passcode.
    public void Show()
    {
        if (endScreenPanel == null)
        {
            Debug.LogWarning("EndScreen: Assign the end-screen panel in the Inspector.", this);
            return;
        }

        endScreenPanel.SetActive(true);
    }

    // Assigns the method to the Exit button's On Click event in the Unity Inspector.
    public void ExitGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}

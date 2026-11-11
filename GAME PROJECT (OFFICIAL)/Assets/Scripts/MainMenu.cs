using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject controlsPanel;

    [Header("Scene")]
    [SerializeField] private string gameSceneName = "Game Prototype";

    private void Awake()
    {
        ShowMainMenu();
    }

    // Assigns this to the Play button's On Click event.
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Assigns this to the Controls button's On Click event.
    public void ShowControls()
    {
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    // Assigns this to the Controls panel's Return button On Click event.
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    // Assigns this to the Exit button's On Click event.
    public void QuitGame()
    {
        Application.Quit();
    }
}

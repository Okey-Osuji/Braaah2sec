using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject pausePanel;
    public GameObject controlsPanel;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
        controlsPanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

  void Update()
{
    if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
    {
        if (controlsPanel.activeSelf)
        {
            BackToPause();
        }
        else if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
}

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;

        pausePanel.SetActive(false);
        controlsPanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenControls()
    {
        pausePanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void BackToPause()
    {
        controlsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}

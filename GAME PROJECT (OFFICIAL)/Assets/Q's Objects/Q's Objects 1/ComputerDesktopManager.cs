
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ComputerDesktopManager : MonoBehaviour
{
    public GameObject[] computerDesktops;

    private PlayerController playerController;

    private void Start()
    {
        int selectedComputer =
            PlayerPrefs.GetInt("SelectedComputer", 1);

        OpenComputer(selectedComputer);

        playerController = FindObjectOfType<PlayerController>();

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseComputer();
        }
    }

    public void OpenComputer(int computerNumber)
    {
        for (int i = 0; i < computerDesktops.Length; i++)
        {
            computerDesktops[i].SetActive(i == computerNumber - 1);
        }
    }

    public void CloseComputer()
    {
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.UnloadSceneAsync("ComputerDesktop");
    }
}

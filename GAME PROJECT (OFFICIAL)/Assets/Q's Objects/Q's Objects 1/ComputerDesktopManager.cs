
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ComputerDesktopManager : MonoBehaviour
{
    public GameObject[] computerDesktops;

    private bool isClosing = false;

    private void Start()
    {
        int selectedComputer =
            PlayerPrefs.GetInt("SelectedComputer", 1);

        foreach (GameObject desktop in computerDesktops)
        {
            if (desktop != null)
            {
                desktop.SetActive(false);
            }
        }

        int index = selectedComputer - 1;

        if (index >= 0 && index < computerDesktops.Length)
        {
            computerDesktops[index].SetActive(true);

            Debug.Log("Displaying Desktop " + selectedComputer);
        }
        else
        {
            Debug.LogError("Invalid computer number: " + selectedComputer);
        }
    }

    private void Update()
    {
        if (isClosing)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Scene desktopScene = gameObject.scene;

            if (!desktopScene.isLoaded)
                return;

            isClosing = true;

            Debug.Log("ESC PRESSED - UNLOADING DESKTOP");

            SceneManager.UnloadSceneAsync(desktopScene);
        }
    }
}

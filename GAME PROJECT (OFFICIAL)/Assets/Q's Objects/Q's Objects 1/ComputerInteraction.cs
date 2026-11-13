
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ComputerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionDistance = 3f;
    public GameObject interactionPrompt;

    private ComputerInteractable currentComputer;
    private float lostTargetTimer = 0f;
    private float targetGracePeriod = 0.2f;

    private void Update()
    {
        Scene desktopScene =
            SceneManager.GetSceneByName("ComputerDesktop");

        if (desktopScene.isLoaded)
        {
            currentComputer = null;
            interactionPrompt.SetActive(false);
            return;
        }

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        ComputerInteractable detectedComputer = null;

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactionDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Collide))
        {
            detectedComputer =
                hit.collider.GetComponentInParent<ComputerInteractable>();
        }

        if (detectedComputer != null)
        {
            currentComputer = detectedComputer;
            lostTargetTimer = 0f;
        }
        else if (currentComputer != null)
        {
            lostTargetTimer += Time.deltaTime;

            if (lostTargetTimer >= targetGracePeriod)
            {
                currentComputer = null;
            }
        }

        interactionPrompt.SetActive(currentComputer != null);

        if (currentComputer != null &&
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            PlayerPrefs.SetInt(
                "SelectedComputer",
                currentComputer.computerNumber
            );

            SceneManager.LoadScene(
                "ComputerDesktop",
                LoadSceneMode.Additive
            );
        }
    }
}

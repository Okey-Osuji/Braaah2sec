using UnityEngine;
using UnityEngine.SceneManagement;

public class Unpause : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject controlsPanel;

    [Header("Scene")]
    [SerializeField] private string gameSceneName = "Game Prototype";   




    // Assigns this to the Play button's On Click event.
    public void PlayGame()
    {
        SceneManager.LoadScene( "Game Prototype");
        
    }
}
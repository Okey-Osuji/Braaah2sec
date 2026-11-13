
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class OpeningCutscene : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public string gameplaySceneName = "GameScene";

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene("Main Menu Version 1( Stanley Parable Version)");
    }
}

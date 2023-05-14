using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class exit_cutscene : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer.loopPointReached += OnEndReached;
    }

    private void Update()
    {
        handleExitClick();
    }
    private void OnEndReached(VideoPlayer vp)
    {
        videoPlayer.Play();
        StartCoroutine(loadNextSceneAsync());
    }

    private void handleExitClick()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            videoPlayer.Play();
            StartCoroutine(loadNextSceneAsync());
        }
    }

    private IEnumerator loadNextSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Act1");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

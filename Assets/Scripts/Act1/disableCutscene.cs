using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class disableCutscene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject mainPart;

    private void Start()
    {
        mainPart.SetActive(false);
        videoPlayer.loopPointReached += OnEndReached;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEndReached(videoPlayer);
        }
    }

    private void OnEndReached(VideoPlayer vp)
    {
        videoPlayer.enabled = false;
        mainPart.SetActive(true);
    }
}

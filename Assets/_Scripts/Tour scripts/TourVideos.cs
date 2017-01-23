using UnityEngine;
using System.Collections;

public class TourVideos : Photon.MonoBehaviour {
    public string FirstVideo;
    private MediaPlayerCtrl mediaPlayer;
    private bool playOnLoad = false;

    void Start() {
        mediaPlayer = GetComponent<MediaPlayerCtrl>();
        mediaPlayer.OnReady += OnLoadedVideo;
        mediaPlayer.Load(FirstVideo);
    }

    // Make a text file of video file names so I do not 
    // need to edit them here when I add more videos.
    public void LoadAndPlayVideo(string fileName) {
        playOnLoad = true;
        mediaPlayer.Load(fileName);
    }

    public void PlayVideo() {
        //mediaPlayer.Play();
    }

    private void OnLoadedVideo() {
        //if (playOnLoad)
        //    mediaPlayer.Play();
    }
}

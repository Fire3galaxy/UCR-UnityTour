using UnityEngine;
using System.Collections;

public class TourVideos : Photon.MonoBehaviour {
    public string FirstVideo;
    public MediaPlayerCtrl mediaPlayer;
    private bool playOnLoad = false;

    void Start() {
        mediaPlayer.OnReady += OnLoadedVideo;
        mediaPlayer.OnVideoError += OnVideoError;
        mediaPlayer.Load(FirstVideo);
        //mediaPlayer.Load("C:///Users/Daniel/AppData/LocalLow/Sky/MyVR/video.mp4");
    }

    // Make a text file of video file names so I do not 
    // need to edit them here when I add more videos.
    public void LoadAndPlayVideo(string fileName) {
        playOnLoad = true;
        mediaPlayer.Load(fileName);
    }

    public void PlayVideo() {
        mediaPlayer.Play();
    }

    private void OnLoadedVideo() {
        if (playOnLoad)
            mediaPlayer.Play();
        Debug.Log("Ready");
    }

    private void OnVideoError(MediaPlayerCtrl.MEDIAPLAYER_ERROR errorCode, 
            MediaPlayerCtrl.MEDIAPLAYER_ERROR errorCodeExtra) {
        Debug.Log("Error with loading video: " + errorCode + ", " + errorCodeExtra);
    }
}

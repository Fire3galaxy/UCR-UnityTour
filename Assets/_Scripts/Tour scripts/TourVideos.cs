using UnityEngine;
using System.Collections;
using System;
using System.Threading;

public class TourVideos : Photon.MonoBehaviour {
    public string FirstVideo;           // URL or StreamingAsset name of first video
    public MediaPlayerCtrl mediaPlayer;

    private bool playOnLoad;    // Used to stop movie from playing after loading during login menu
    private string[] manifest;
    private int currentVideo;

    // For the thread tests
    object thisLock = new object();
    string persistentPath;
    float timeStart;
    bool logThread;
    bool threadIsDone;
    bool ThreadIsDone
    {
        get
        {
            bool b;
            lock (thisLock) {
                b = threadIsDone;
            }
            return b;
        }
        set
        {
            lock (thisLock) {
                threadIsDone = value;
            }
        }
    }

    void Start() {
        ThreadIsDone = false;
        logThread = false;
        playOnLoad = false;
        currentVideo = 0;

        // Setup callback functions
        mediaPlayer.OnReady += OnLoadedVideo;
        mediaPlayer.OnEnd += OnEnd;
        mediaPlayer.OnVideoError += OnVideoError;
        mediaPlayer.OnVideoFirstFrameReady += OnVideoFirstFrameReady;

        // FIXME: Debug Testing. These functions are called immediately (So no UI should be visible)
        loadFirstVideoTest(FirstVideo);
        //mediaPlayer.Load("C:///Users/Daniel/AppData/LocalLow/Sky/MyVR/video.mp4");
        //StartCoroutine(loadManifestVideoOnlineTest());
    }

    void Update() {
        // When thread finishes, log how long it ran
        //if (!logThread && ThreadIsDone) {
        //    logThread = true;
        //    Debug.Log(Time.time - timeStart);
        //}
    }

    private void loadFirstVideoTest(string firstVideo) {
        mediaPlayer.Load(firstVideo);
    }

    private IEnumerator loadManifestVideoOnlineTest() {
        // Link to text "bourns1" manifest on dropbox
        WWW bourns1 = new WWW("https://www.dropbox.com/s/lprrlkl7xwjexa3/bourns1.txt?dl=1");
        yield return bourns1;

        char[] delimiters = { '\r', '\n' };
        manifest = System.Text.Encoding.ASCII.GetString(bourns1.bytes).Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        //mediaPlayer.Load(manifest[currentIndex]);
        
        //string videoFilename = manifest[0].Substring(6) + "-" + currentVideo.ToString() + ".mp4";
        mediaPlayer.Load(manifest[currentVideo + 2]);   // 1st video
        //yield return VideoDownloader.Save(videoFilename, manifest[currentVideo + 3]); // 2nd video
        //mediaPlayer.Load(VideoDownloader.getMediaCompatDatapath(videoFilename));
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

    // 1. Load video via mediaPlayer as soon as previous video finishes 
    // (slow probably because we could be downloading it sooner)
    // After testing: resolution might be TOO high, can't load 10 second video. 
    // Try downloading locally first?
    private void OnEnd() {
        Debug.Log("Video end: " + mediaPlayer.m_strFileName);
        //if (manifest != null)
        //    mediaPlayer.Load(manifest[++currentIndex]);
        mediaPlayer.SeekTo(0);
        mediaPlayer.Play();
    }

    private void OnVideoError(MediaPlayerCtrl.MEDIAPLAYER_ERROR errorCode, 
            MediaPlayerCtrl.MEDIAPLAYER_ERROR errorCodeExtra) {
        Debug.Log("Error with loading video: " + errorCode + ", " + errorCodeExtra);
    }

    private void OnVideoFirstFrameReady() {
        Debug.Log("OnVideoFirstFrameReady");

        // Download next video with coroutine while current video plays
        saveNextVideoTest();
    }
    
    private void saveNextVideoTest() {
        //string videoFilename = Application.persistentDataPath + "/" + manifest[0].Substring(6) + "-"
        //    + (currentVideo + 1).ToString() + ".mp4";
        //StartCoroutine(VideoDownloader.Save(videoFilename, manifest[currentVideo + 3]));
        //string videoFilename = Application.persistentDataPath + "/cafe-1.mp4";
        //StartCoroutine(VideoDownloader.Save(videoFilename, "http://jiasiproj.cs.ucr.edu/Windows/1st%20floor%20of%20bourns/Synced%20Videos/Video1/0.kava.mp4"));
    }

    private void downloadVideoThreadTest() {
        ThreadIsDone = false;
        string videoFilename = persistentPath + "/" + manifest[0].Substring(6) + "-" 
            + currentVideo.ToString() + ".mp4";
        VideoDownloader.Save(videoFilename, manifest[currentVideo + 3]);
        ThreadIsDone = true;
    }

    // Debug button
    public void playPause() {
        if (mediaPlayer.m_CurrentState == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
            mediaPlayer.Pause();
        else
            mediaPlayer.Play();
    }
}

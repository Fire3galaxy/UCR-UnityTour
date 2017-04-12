using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;

public class TourVideos : Photon.MonoBehaviour {
    public string FirstVideo;           // URL or StreamingAsset name of first video

    private MediaPlayerCtrl mediaPlayer;
    private bool playOnLoad;    // Used to stop movie from playing after loading during login menu
    private string[] manifest;
    private int currentVideo;

    //hold filenames for the downloaded videos
    private Queue<String> videoQueue;

    // For the thread tests
    object thisLock = new object();
    string persistentPath;
    float timeStart;
    bool logThread;
    // Lock isn't working. FIXME: Figure out why so we can update this in thread
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
        videoQueue = new Queue<String>();
        currentVideo = 0;

        // Setup callback functions
        mediaPlayer = GetComponent<MediaPlayerCtrl>();
        mediaPlayer.OnReady += OnLoadedVideo;
        mediaPlayer.OnEnd += OnEnd;
        mediaPlayer.OnVideoError += OnVideoError;
        mediaPlayer.OnVideoFirstFrameReady += OnVideoFirstFrameReady;

        //loadFirstVideoTest(FirstVideo);
        //mediaPlayer.Load("C:///Users/Daniel/AppData/LocalLow/Sky/MyVR/video.mp4");
        StartCoroutine(loadManifestVideoOnlineTest());
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
        if (manifest != null && videoQueue.Count != 0)
        {
            //load next video
            string nextVideo = videoQueue.Dequeue();
            Debug.Log("Loading next video " + nextVideo);
            mediaPlayer.Load(nextVideo);
        }
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
        String filename = getNextVideo();
        videoQueue.Enqueue(filename);
    }

    private String getNextVideo() {
        //wonky hackish if statement to loop the videos
        if (currentVideo >= manifest.Length - 3) {
            currentVideo = 0;
        }
        else {
            currentVideo++;
        }

        string filename = manifest[0].Substring(6) + "-" + currentVideo.ToString() + ".mp4";
        string filepath = Application.streamingAssetsPath + "/";
        string file = filepath + filename;

        Debug.Log("Next Video: " + filename);

        if (!System.IO.File.Exists(file)){
            saveNextVideo(file);
        }

        return filename;
    }

    //saves next video and returns the filename
    private void saveNextVideo(string savePath) {
        StartCoroutine(VideoDownloader.Save(savePath, manifest[currentVideo + 2]));
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

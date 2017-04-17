using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VR;
using UnityEngine.UI;
using System.Collections;

public class VideoLogic : Photon.PunBehaviour {
    public GameObject UICanvas;
    public GameObject EventSystem;
    public GameObject GvrViewerPrefab;
    public GameObject GvrReticlePrefab;
    public GameObject AnnotationPrefab;
    public TourLibrary videoLibrary;

    // FUNCTIONS TO SWITCH FROM UI MODE TO VR/TOUR MODE
    // Called by UI objects. Changes from UI to Tour objects
    public void SwitchToTourLogic() {
        UICanvas.SetActive(false); // Remove canvas
        SetupCardboardView();
        if (playOnLoad)
            mediaPlayer.Play();
        else
            playOnLoad = true;
    }

    // Google Cardboard Settings, phone orientation
    private void SetupCardboardView() {
        // Add VR View
        //VRSettings.LoadDeviceByName("cardboard");
        VRSettings.enabled = true;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Instantiate(GvrViewerPrefab, Camera.main.transform);
        Instantiate(GvrReticlePrefab, Camera.main.transform);
        Debug.Log("Should be in VR mode");

        // Change eventsystem input module
        EventSystem.GetComponent<StandaloneInputModule>().enabled = false;
        EventSystem.GetComponent<GvrPointerInputModule>().enabled = true;
    }
    
    // FUNCTIONS TO CONTROL VIDEO PLAYBACK IN TOUR MODE
    public MediaPlayerCtrl mediaPlayer;
    private bool playOnLoad;    // Used to stop movie from playing after loading during login menu

    void Start() {
        playOnLoad = false;
        
        // Setup callback functions
        mediaPlayer.OnReady += CheckAndPlayVideo;
        mediaPlayer.OnEnd += LoopVideo;
        mediaPlayer.OnVideoError += LogVideoError;
        
        // Load first video
        LoadVideo(0);
    }

    private void CheckAndPlayVideo() {
        if (playOnLoad)
            mediaPlayer.Play();
        else
            playOnLoad = true;  // False only during UI Screen

        Debug.Log("Video ready");
    }

    private void LoopVideo() {
        Debug.Log("Video end: " + mediaPlayer.m_strFileName);
        mediaPlayer.Stop();
        mediaPlayer.Play();
    }

    private void LogVideoError(MediaPlayerCtrl.MEDIAPLAYER_ERROR errorCode,
            MediaPlayerCtrl.MEDIAPLAYER_ERROR errorCodeExtra) {
        Debug.Log("Error with loading video: " + errorCode + ", " + errorCodeExtra);
    }

    // Debug button
    public void playPause() {
        if (mediaPlayer.m_CurrentState == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
            mediaPlayer.Pause();
        else
            mediaPlayer.Play();
    }

    // FUNCTIONS AND OBJECTS TO LOAD ANNOTATIONS IN VIDEO
    GameObject activeAnnotationPrefabs; // FIXME: in future, make this array for multiple annote per video

    private void LoadVideo(int vidNum) {
        // Video
        mediaPlayer.Load(videoLibrary.GetVideo(vidNum));

        // GameObject that will be parent to annotations
        GameObject allTourObjects = GameObject.Find("Tour Objects");    
        Transform allTourTransform = null;
        if (allTourObjects != null)
            allTourTransform = allTourObjects.GetComponent<Transform>();

        // Annotations
        if (activeAnnotationPrefabs != null)    // Destroy old annotations
            Destroy(activeAnnotationPrefabs);

        // FIXME: In future, replace inside of this with for loop that handles multiple annotations
        // Also, simplify design of annotations using mathematical formulas or something
        // Basically, point at place with camera where red dot should go, then calculate what the 
        // position and rotations should be.
        if (AnnotationPrefab != null) {         // Add new annotations
            Annotation annotation = videoLibrary.GetAnnotation(vidNum);

            // Annotation exists
            if (annotation != null) {
                // Instantiate new prefab
                if (allTourTransform != null)
                    activeAnnotationPrefabs = (GameObject)Instantiate(AnnotationPrefab, allTourTransform);
                else
                    activeAnnotationPrefabs = Instantiate(AnnotationPrefab);

                // Transform of new prefab
                activeAnnotationPrefabs.transform.position = annotation.Position;
                activeAnnotationPrefabs.transform.rotation = Quaternion.Euler(annotation.Rotation);

                // Text of new prefab
                Text[] texts = activeAnnotationPrefabs.GetComponentsInChildren<Text>();
                if (texts != null && texts.Length == 2) {
                    texts[0].text = annotation.Title;
                    texts[1].text = annotation.Description;
                }
            }
        }
    }
    
    // FUNCTION TO CHANGE VIDEO
    // For ECE Day Demo: Functions for buttons from Video Controller UI prefab
    public void NetworkChangeVideo(int vidNum) {
        LoadVideo(vidNum);
    }

    // CALLBACKS TO PHOTON
    override public void OnDisconnectedFromPhoton() {
        Debug.Log("Disconnected from photon");
    }
}

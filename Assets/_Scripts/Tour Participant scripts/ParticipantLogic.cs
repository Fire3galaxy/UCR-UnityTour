using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VR;

// A "Table of contents" for all the functions:
// 1. FUNCTIONS TO SWITCH FROM UI MODE TO VR/TOUR MODE & BACK
// 2. FUNCTIONS TO CONTROL VIDEO PLAYBACK IN TOUR (PARTICIPANT) MODE
// 3. FUNCTIONS TO LOAD VIDEOS AND ANNOTATIONS
// 4. CALLBACKS TO PHOTON
public class ParticipantLogic : Photon.PunBehaviour {
    public GameObject EventSystem;
    public GameObject GvrViewerPrefab;
    public GameObject GvrReticlePrefab;
    public GameObject AnnotationPrefab;
    public MediaPlayerCtrl mediaPlayer;

    // FUNCTIONS TO SWITCH FROM UI MODE TO VR/TOUR MODE & BACK
    // Called by UI objects. Changes from UI to Tour objects
    public void SwitchToTourLogic() {
        SetupCardboardView();   // Change settings for cardboard
    }

    private void SwitchToUILogic() {
        // Leave Room
        PhotonNetwork.LeaveRoom();

        // Stop tour
        mediaPlayer.Stop();
        RemoveCardboardView();

        // Reset UI Screen back to home page
        MainMenuLogic UILogic = GetComponent<MainMenuLogic>();
        UILogic.BackToHomeScreen();
    }

    // Google Cardboard Settings, phone orientation
    private void SetupCardboardView() {
        // Add VR View if needed
        if (GvrViewer.Instance != null) {
            GvrViewer.Instance.VRModeEnabled = true;
        } else {
            Instantiate(GvrViewerPrefab, Camera.main.transform);
            Instantiate(GvrReticlePrefab, Camera.main.transform);
        }

        // VR View is landscape
        VRSettings.enabled = true;
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Change eventsystem input module
        EventSystem.GetComponent<StandaloneInputModule>().enabled = false;
        EventSystem.GetComponent<GvrPointerInputModule>().enabled = true;

        Debug.Log("Should be in VR mode");
    }

    // Google Cardboard Settings, phone orientation
    private void RemoveCardboardView() {
        // Remove VR View
        VRSettings.enabled = false;
        GvrViewer.Instance.VRModeEnabled = false;
        Screen.orientation = ScreenOrientation.Portrait;;
        Debug.Log("Should be out of VR mode");

        // Change eventsystem input module
        EventSystem.GetComponent<StandaloneInputModule>().enabled = true;
        EventSystem.GetComponent<GvrPointerInputModule>().enabled = false;
    }

    // FUNCTIONS TO CONTROL VIDEO PLAYBACK IN TOUR MODE
    void Start() {
        // Setup callback functions
        mediaPlayer.OnEnd += LoopVideo;
        mediaPlayer.OnVideoError += LogVideoError;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            SwitchToUILogic();
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

    // FUNCTIONS TO LOAD VIDEOS AND ANNOTATIONS
    public void LoadVideo(int tourNum, int vidNum) {
        Debug.Log("Loading: " + TourLibrary.instance.GetVideoPath(tourNum, vidNum));
        // Video
        mediaPlayer.Load(TourLibrary.instance.GetVideoPath(tourNum, vidNum));

        // GameObject that will be parent to annotations
        GameObject allTourObjects = GameObject.Find("Tour Objects");    
        Transform allTourTransform = null;
        if (allTourObjects != null)
            allTourTransform = allTourObjects.GetComponent<Transform>();

        // Annotations
        //if (activeAnnotationPrefabs != null)    // Destroy old annotations
        //    Destroy(activeAnnotationPrefabs);

        // FIXME: In future, replace inside of this with for loop that handles multiple annotations
        // Also, simplify design of annotations using mathematical formulas or something
        // Basically, point at place with camera where red dot should go, then calculate what the 
        // position and rotations should be.
        //if (AnnotationPrefab != null) {         // Add new annotations
        //    Annotation annotation = TourLibrary.instance.GetAnnotation(vidNum);

        //    // Annotation exists
        //    if (annotation != null) {
        //        // Instantiate new prefab
        //        if (allTourTransform != null)
        //            activeAnnotationPrefabs = (GameObject)Instantiate(AnnotationPrefab, allTourTransform);
        //        else
        //            activeAnnotationPrefabs = Instantiate(AnnotationPrefab);

        //        // Transform of new prefab
        //        activeAnnotationPrefabs.transform.position = annotation.Position;
        //        activeAnnotationPrefabs.transform.rotation = Quaternion.Euler(annotation.Rotation);

        //        // Text of new prefab
        //        Text[] texts = activeAnnotationPrefabs.GetComponentsInChildren<Text>();
        //        if (texts != null && texts.Length == 2) {
        //            texts[0].text = annotation.Title;
        //            texts[1].text = annotation.Description;
        //        }
        //    }
        //}
    }

    // CALLBACKS TO PHOTON
    override public void OnDisconnectedFromPhoton() {
        Debug.Log("Disconnected from photon");
    }
}

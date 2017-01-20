using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VR;
using System.Collections;

public class TourLogic : Photon.MonoBehaviour {
    public GameObject UICanvas;
    public GameObject EventSystem;
    public GameObject GvrViewerPrefab;
    public GameObject GvrReticlePrefab;
    public MediaPlayerCtrl mediaPlayer;

    public void SwitchToTourLogic() {
        UICanvas.SetActive(false); // Remove canvas
        SetupCardboardView();
        //mediaPlayer.Play();
    }

    private void SetupCardboardView() {
        // Add VR View
        //VRSettings.LoadDeviceByName("cardboard");
        //VRSettings.enabled = true;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Instantiate(GvrViewerPrefab, Camera.main.transform);
        //Instantiate(GvrReticlePrefab, Camera.main.transform);
        Debug.Log("Should be in VR mode");

        // Change eventsystem input module
        EventSystem.GetComponent<StandaloneInputModule>().enabled = false;
        EventSystem.GetComponent<GvrPointerInputModule>().enabled = true;
    }
    
    //public virtual void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
    //    if (PhotonNetwork.room.playerCount == 2)
    //        mediaPlayer.Play();
    //}
}

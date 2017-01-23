using UnityEngine;
using System.Collections;

public class VideoControls : Photon.MonoBehaviour {
    TourVideos videoLibrary;

    void Start() {
        videoLibrary = GameObject.Find("Tour Objects/VideoManager")
            .GetComponent<TourVideos>();
    }

    public void MovePlayersToNextVideo() {
        Debug.Log("Called RPC function");
        photonView.RPC("ToNextVideo", PhotonTargets.All);
    }

    [PunRPC]
    private void ToNextVideo() {
        Debug.Log("Video change by master client");
        videoLibrary.LoadAndPlayVideo("racing360.mp4");
    }
}

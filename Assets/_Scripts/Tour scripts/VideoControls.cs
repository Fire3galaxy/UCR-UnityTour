using UnityEngine;
using System.Collections;

public class VideoControls : Photon.MonoBehaviour {
    TourVideos videoLibrary;
    VideoLogic videoLogic;

    void Start() {
        GameObject logicObject = GameObject.Find("Game and PUN Logic");
        videoLibrary = logicObject.GetComponent<TourVideos>();
        videoLogic = logicObject.GetComponent<VideoLogic>();
    }

    public void MovePlayersToNextVideo() {
        Debug.Log("Called RPC function");
        photonView.RPC("ToNextVideo", PhotonTargets.All);
    }

    [PunRPC]
    private void ToNextVideo() {
        Debug.Log("Video change by master client");
        videoLibrary.LoadAndPlayVideo("http://www.dropbox.com/s/id12n92mmpr4n7s/racing360.mp4?dl=1");
    }

    // All functions below used by Video Controller Buttons prefab
    public void MovePlayersToVideo(int vidNum) {
        Debug.Log("Called RPC function");
        photonView.RPC("ChooseVideo", PhotonTargets.All, vidNum);
    }

    [PunRPC]
    private void ChooseVideo(int vidNum) {
        videoLogic.NetworkChangeVideo(vidNum);
    }
}

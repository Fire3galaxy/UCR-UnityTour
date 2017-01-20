using UnityEngine;
using System.Collections;

public class VideoControls : Photon.MonoBehaviour {
    MediaPlayerCtrl mediaPlayer;

    void Start() {
        mediaPlayer = GameObject.Find("Tour Objects/VideoManager")
            .GetComponent<MediaPlayerCtrl>();
    }

    public void MovePlayersToNextVideo() {
        Debug.Log("Called RPC function");
        photonView.RPC("ToNextVideo", PhotonTargets.All);
    }

    [PunRPC]
    private void ToNextVideo() {
        Debug.Log("Video change by master client");
    }
}

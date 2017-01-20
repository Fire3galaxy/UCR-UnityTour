using UnityEngine;
using System.Collections;

public class MasterClientControls : Photon.MonoBehaviour {
    MediaPlayerCtrl mediaPlayer;

    void Start() {
        mediaPlayer = GameObject.Find("Tour Objects/VideoManager")
            .GetComponent<MediaPlayerCtrl>();
    }

    [PunRPC]
    public void MovePlayersToNextVideo() {
        Debug.Log("Video change by master client");
    }
}

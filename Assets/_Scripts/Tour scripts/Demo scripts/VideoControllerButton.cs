using UnityEngine;
using System.Collections;
using System;

public class VideoControllerButton : Photon.MonoBehaviour {
    public VideoControls videoControls;

    // Change video based on which button was clicked (1-5)
    public void OnClick() {
        int vidNum;
        if (videoControls != null && int.TryParse(tag, out vidNum))
            videoControls.MovePlayersToVideo(vidNum);
        else {
            Debug.LogError("Could not change video");
        }
    }
}

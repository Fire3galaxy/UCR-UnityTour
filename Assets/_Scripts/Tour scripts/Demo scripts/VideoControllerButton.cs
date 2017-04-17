using UnityEngine;
using System.Collections;
using System;

public class VideoControllerButton : Photon.MonoBehaviour {
    private VideoControls videoControls;
    private int index;

    public void setVideoControls(VideoControls ctrls) {
        videoControls = ctrls;
    }

    public void setIndex(int i) {
        index = i;
    }

    // Change video based on which button was clicked (1-5)
    public void OnClick() {
        //int vidNum;
        if (videoControls != null) //&& int.TryParse(tag, out vidNum))
            videoControls.MovePlayersToVideo(index);
        else
            Debug.LogError("Could not change video");
    }

    public void OnClickLeaveRoom() {
        Debug.Log("Left Photon room");

        // Leave Room
        PhotonNetwork.LeaveRoom();

        // Set up UI to be at home screen
        GameObject logicObject = GameObject.Find("Game and PUN Logic");
        MainMenuLogic UIlogic = logicObject.GetComponent<MainMenuLogic>();
        UIlogic.BackToHomeScreen();

        // Destroy this masterClient UI prefab
        if (InstantiatePlayerObjects.masterClientUI != null) {
            Destroy(InstantiatePlayerObjects.masterClientUI);
            InstantiatePlayerObjects.masterClientUI = null;
        }
    }
}

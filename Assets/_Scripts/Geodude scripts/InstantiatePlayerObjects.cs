using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InstantiatePlayerObjects : Photon.MonoBehaviour {
    public GameObject avatarPrefab;
    public GameObject masterClientUIPrefab; // For ECE Day Presentation. Lets masterclient determine video played

    static public GameObject masterClientUI = null;

    public void OnJoinedRoom()
    {
        // Instantiate avatar
        GameObject avatarGameObject = PhotonNetwork.Instantiate(avatarPrefab.name, 
            new Vector3(0, 0, 0), Quaternion.identity, 0);
        avatarGameObject.GetComponent<Renderer>().material.color = Color.cyan;

        // Instantiate Menu controls for master client &
        // set up menu with video controls
        if (PhotonNetwork.isMasterClient) {
            Debug.Log("You are master client");

            // Instantiate Video Controls
            masterClientUI = Instantiate(masterClientUIPrefab);
            Button[] buttons = masterClientUI.GetComponentsInChildren<Button>();

            // Give actions to corresponding buttons
            VideoControls controls = avatarGameObject.GetComponent<VideoControls>();
            for (int i = 0; i < buttons.Length; i++) {
                VideoControllerButton controllerButton = buttons[i].GetComponent<VideoControllerButton>();
                controllerButton.setVideoControls(controls);
                controllerButton.setIndex(i);
            }   
        }
    }
}

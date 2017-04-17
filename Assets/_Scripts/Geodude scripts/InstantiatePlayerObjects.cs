using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InstantiatePlayerObjects : Photon.MonoBehaviour {
    public GameObject avatarPrefab;
    public GameObject masterClientCanvas;
    public GameObject videoControllerUIPrefab; // For ECE Day Presentation. Lets masterclient determine video played

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

            VideoControls controls = avatarGameObject.GetComponent<VideoControls>();

            GameObject controllerObject = Instantiate(videoControllerUIPrefab);
            Button[] buttons = controllerObject.GetComponentsInChildren<Button>();

            foreach (Button b in buttons)
                b.GetComponent<VideoControllerButton>().videoControls = controls;

            //GameObject menuObject = Instantiate(masterClientCanvas);

            //Button button = menuObject.transform.GetComponentInChildren<Button>();
            //button.onClick.AddListener(controls.MovePlayersToNextVideo);
        }
    }
}

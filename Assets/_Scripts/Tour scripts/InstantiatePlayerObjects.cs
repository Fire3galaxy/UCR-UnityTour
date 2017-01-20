using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InstantiatePlayerObjects : Photon.MonoBehaviour {
    public GameObject avatarPrefab;
    public GameObject masterClientCanvas;
    private GameObject avatarGameObject;

    public void OnJoinedRoom()
    {
        // Instantiate avatar
        avatarGameObject = PhotonNetwork.Instantiate(avatarPrefab.name, 
            new Vector3(0, 0, 0), Quaternion.identity, 0);
        avatarGameObject.GetComponent<Renderer>().material.color = Color.cyan;

        // Instantiate Menu controls for master client
        if (PhotonNetwork.isMasterClient) {
            Debug.Log("You are master client");
            avatarGameObject.AddComponent<MasterClientControls>();

            GameObject menuObject = Instantiate(masterClientCanvas);
            MasterClientControls controls = 
                avatarGameObject.GetComponent<MasterClientControls>();
            
            menuObject.transform
                .FindChild("Button")
                .GetComponent<Button>()
                .onClick
                .AddListener(controls.MovePlayersToNextVideo);
        }
    }
}

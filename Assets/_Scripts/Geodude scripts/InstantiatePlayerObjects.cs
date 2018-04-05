using UnityEngine;

public class InstantiatePlayerObjects : Photon.MonoBehaviour {
    public GameObject avatarPrefab;
    public GameObject avatarGuidePrefab;

    public void OnJoinedRoom()
    {
        if (!PhotonNetwork.isMasterClient) {
            // Instantiate avatar
            GameObject avatarGameObject = PhotonNetwork.Instantiate(avatarPrefab.name,
                new Vector3(0, 0, 0), Quaternion.identity, 0);
            avatarGameObject.GetComponent<Renderer>().material.color = Color.cyan;
        } else {
            // Instantiate avatar
            GameObject avatarGameObject = PhotonNetwork.Instantiate(avatarGuidePrefab.name,
                new Vector3(0, 0, 0), Quaternion.identity, 0);

            // Attach controls to masterclient geodude and give it to GuideLogic
            GuideLogic TourGuideLogic = GameObject.Find("Game and PUN Logic").GetComponent<GuideLogic>();
            NetworkVideoControls controls = avatarGameObject.GetComponent<NetworkVideoControls>();
            TourGuideLogic.SetVideoControls(controls);
        }
    }
}

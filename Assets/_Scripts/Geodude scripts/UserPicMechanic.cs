using UnityEngine;
using System.Collections;

public class UserPicMechanic : Photon.MonoBehaviour {
    private const float DISTANCE = 1.4f;

    public GameObject userPicPrefab;
    private GameObject userPicObject = null;

    void Update() {
        if (!photonView.isMine && photonView.owner.ID != PhotonNetwork.masterClient.ID) {
            if (userPicObject == null)
                createUserPic();
            else
                updateUserPicPos();
        }
    }
    
    void createUserPic() {
        // Vector from object to camera
        Vector3 towardsCamera = DISTANCE * transform.forward;
        // Face of quad is on other side
        Quaternion rotation = Quaternion.LookRotation(-towardsCamera);

        // How to instantiate an object:
        // 1. Copy prefab "userPicPrefab"
        // 2. At position "transform.position + ... transform.forward)"
        // 3. With rotation "rotation"
        userPicObject = (GameObject)Instantiate(
            userPicPrefab,
            transform.position + (DISTANCE * transform.forward),
            rotation);
        userPicObject.GetComponent<Renderer>().material.color = Color.blue;
    }

    void updateUserPicPos() {
        // Vector from object to camera
        Vector3 towardsCamera = transform.position
            - userPicObject.transform.position;

        // This distance away
        userPicObject.transform.position = transform.position
            + (DISTANCE * transform.forward);
        // Face of quad is on other side
        userPicObject.transform.rotation =
            Quaternion.LookRotation(-towardsCamera);
    }

}
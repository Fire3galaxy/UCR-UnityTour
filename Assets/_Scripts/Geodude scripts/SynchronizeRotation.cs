using UnityEngine;
using System.Collections;

// Page 7 of the documentation, observing a monobehaviour
public class SynchronizeRotation : Photon.MonoBehaviour {
    private Quaternion rotation;

    // Update is called once per frame
    void Update() {
        if (photonView.isMine)
            rotation = transform.rotation;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting)
            stream.SendNext(rotation);
        else
            transform.rotation = (Quaternion)stream.ReceiveNext();
    }
}
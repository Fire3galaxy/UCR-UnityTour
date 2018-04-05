using UnityEngine;
using System.Collections;

// Page 7 of the documentation, observing a monobehaviour
public class SynchronizeRotation : Photon.MonoBehaviour {
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting)
            stream.SendNext(transform.rotation);
        else
            transform.rotation = (Quaternion)stream.ReceiveNext();
    }
}
using UnityEngine;
using System.Collections;

public class AttachToLocalCamera : Photon.MonoBehaviour
{
    void Update()
    {
        if (photonView.isMine)
            transform.rotation = Camera.main.GetComponent<Transform>().rotation;
    }
}

using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {	
	void Start() {
        GetComponent<Transform>().LookAt(Camera.main.GetComponent<Transform>());
    }
}

using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

// Attach this to object with text component
public class DebugShowVideoPlace : MonoBehaviour {
    public MediaPlayerCtrl mediaPlayer;
    Text videoText;
    
    void Start() {
        videoText = GetComponent<Text>();
    }
	
	void Update () {
        if (videoText != null && mediaPlayer != null)
            videoText.text = (mediaPlayer.GetSeekPosition() / 1000.0).ToString()
                + "/" + (mediaPlayer.GetDuration() / 1000.0).ToString();
	}
}

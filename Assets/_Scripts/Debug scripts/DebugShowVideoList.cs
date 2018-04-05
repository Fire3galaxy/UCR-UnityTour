using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class DebugShowVideoList : MonoBehaviour {
    bool hasLatestFiles;

	// Use this for initialization
	void Start () {
        hasLatestFiles = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (!hasLatestFiles) {
            Text text = GetComponent<Text>();
            foreach (string filename in Directory.GetFiles(Application.persistentDataPath))
                text.text += filename.Substring(filename.LastIndexOf('\\') + 1) + "\n";
            hasLatestFiles = true;
        }
	}

    public void DeleteFiles() {
        foreach (string filename in Directory.GetFiles(Application.persistentDataPath)) {
            Debug.Log("in persistent path: " + filename);
            if (filename.EndsWith(".mp4"))
                File.Delete(filename);
        }
        hasLatestFiles = false;
    }
}

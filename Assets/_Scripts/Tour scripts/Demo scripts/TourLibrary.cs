using UnityEngine;
using System.Collections;
using System;
using System.Threading;

public class TourLibrary : MonoBehaviour {
    public string[] videos; // URL or StreamingAsset name of first video
    private Annotation[] annotations;

    // Constructor to load up annotations associated with videos
    // For ECE day, these are hardcoded
    public TourLibrary() {
        annotations = new Annotation[6];
        annotations[0] = new Annotation(
            new Vector3(1.053f, .347f, .747f),
            new Vector3(-29.074f, 53.6f, .52f),
            "UCR Campus Store",
            "-Congrats, Class of 2017\n-We price match textbooks\n-Hours Today: 8am - 6pm");
    }
    
    public string GetVideo(int index) {
        string videoPath = Application.persistentDataPath + "/" + videos[index];
        int singleSlashIndex = videoPath.IndexOf('/');
        
        // MediaPlayerCtrl class requires this string to use a file path or url
        return "file://" + videoPath;
    }

    public Annotation GetAnnotation(int index) {
        if (index >= 0 && index < annotations.Length)
            return annotations[index];
        else
            return null;
    }
}

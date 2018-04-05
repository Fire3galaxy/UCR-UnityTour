using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class TourLibrary : MonoBehaviour {
    static public TourLibrary instance;

    public List<TourJsonObject> tourList;
    private Annotation[] annotationList;

    // Constructor to load up annotations associated with videos
    // For ECE day, these are hardcoded
    void Start() {
        tourList = new List<TourJsonObject>();
        instance = this; // For static access to video names

        // Annotations currently disabled
        //// Annotations (currently hardcoded and just one)
        //annotationList = new Annotation[6];
        //annotationList[0] = new Annotation(
        //    new Vector3(1.053f, .347f, .747f),
        //    new Vector3(-29.074f, 53.6f, .52f),
        //    "UCR Campus Store",
        //    "-Congrats, Class of 2017\n-We price match textbooks\n-Hours Today: 8am - 6pm");

        // Videos
        DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] tourFiles = info.GetFiles("*json");
        Debug.Log("length: " + tourFiles.Length);
        foreach (FileInfo file in tourFiles) {
            string tourJson = File.ReadAllText(file.FullName);
            TourJsonObject tour = JsonUtility.FromJson<TourJsonObject>(tourJson);
            tourList.Add(tour);
        }
    }

    // MediaPlayerCtrl class requires this string to use a file path or url
    public string GetVideoPath(int tourIndex, int videoIndex) {
        return "file://" + Application.persistentDataPath + "/" 
            + tourList[tourIndex].videos[videoIndex].name + ".mp4";
    }

    //public Annotation GetAnnotation(int index) {
    //    if (index >= 0 && index < annotationList.Length)
    //        return annotationList[index];
    //    else
    //        return null;
    //}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
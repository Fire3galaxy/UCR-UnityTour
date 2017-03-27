using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Net;

public class VideoDownloader : MonoBehaviour {
    [Serializable]
    class Test {
        public string name, fieryAbility;

        public Test(string name, string fieryAbility) {
            this.name = name;
            this.fieryAbility = fieryAbility;
        }
    }

    void Start() {
        Debug.Log(Application.persistentDataPath);
        TestAllFunctions();
    }

    void TestAllFunctions() {
        Debug.Log("Starting Save Function");
        StartCoroutine(Save());
    }

    // From laptop
    // 93 MB -> 24 seconds
    // 11 MB -> 3.5 seconds
    // 16.7 MB -> ~5 seconds
    static public IEnumerator Save() {
        Debug.Log("In Save");
        //yield return null;
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Open(
        //    Application.persistentDataPath + "/video.mp4", FileMode.OpenOrCreate);

        //bf.Serialize(file, new Test("Flora", "Fira Root"));
        //file.Close();

        float timeStart = Time.time;
        FileStream file = File.Open(
            Application.persistentDataPath + "/video.mp4", FileMode.Create);
        WWW videoWWW = new WWW("https://www.dropbox.com/s/9lps16modn1ye76/racing360-3.mp4?dl=1");
        yield return videoWWW;
        file.Write(videoWWW.bytes, 0, videoWWW.bytes.Length);
        file.Close();

        Debug.Log("Done downloading and writing. Time to download: " + (Time.time - timeStart).ToString());
    }
    
    // Run on main thread as coroutine
    static public IEnumerator Save(string filename, string url) {
        Debug.Log("In Save: " + filename + " from " + url);

        float timeStart = Time.time;
        FileStream file = File.Open(filename, FileMode.Create);
        WWW videoWWW = new WWW(url);
        yield return videoWWW;
        file.Write(videoWWW.bytes, 0, videoWWW.bytes.Length);
        file.Close();

        Debug.Log("Done downloading and writing. Time to download: " + (Time.time - timeStart).ToString());
    }

    public static string getMediaCompatDatapath(string filename) {
        string s = Application.persistentDataPath + "/" + filename;
        if (s.Contains("C:/"))
            return "C://" + s.Substring(3);
        return null;
    }

    void Load() {
        Debug.Log("In Save");
        if (File.Exists(Application.persistentDataPath + "/video.mp4")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(
                Application.persistentDataPath + "/video.mp4", FileMode.Open);
            Test t = (Test)bf.Deserialize(file);
            file.Close();

            Debug.Log(t.name + ", ability: " + t.fieryAbility);
        }
    }
}

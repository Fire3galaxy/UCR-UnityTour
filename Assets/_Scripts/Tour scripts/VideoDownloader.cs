using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

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

    IEnumerator Save() {
        Debug.Log("In Save");
        //yield return null;
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Open(
        //    Application.persistentDataPath + "/video.mp4", FileMode.OpenOrCreate);

        //bf.Serialize(file, new Test("Flora", "Fira Root"));
        //file.Close();

        FileStream file = File.Open(
            Application.persistentDataPath + "/video.mp4", FileMode.Create);
        WWW videoWWW = new WWW("https://www.dropbox.com/s/12vdxst6lta5ubg/stitched%20scene%20g.mp4?dl=1");
        yield return videoWWW;
        file.Write(videoWWW.bytes, 0, videoWWW.bytes.Length);
        file.Close();

        Debug.Log("Done downloading and writing");
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

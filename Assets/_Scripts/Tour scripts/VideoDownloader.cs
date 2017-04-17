using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Threading;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class VideoDownloader : MonoBehaviour {
    public Text text;
    string storagePath;

    [Serializable]
    class Test {
        public string name, fieryAbility;

        public Test(string name, string fieryAbility) {
            this.name = name;
            this.fieryAbility = fieryAbility;
        }
    }

    // Start function is for debug download tests
    //void Start() {
    //    storagePath = Application.persistentDataPath;
    //    Debug.Log(Application.persistentDataPath);
    //    //TestAllFunctions();
    //    //newThread();
    //}

    void newThread() {
        Thread thread = new Thread(new ThreadStart(downloadVideo));
        thread.Start();
    }

    // Code from http://stackoverflow.com/questions/4926676/mono-https-webrequest-fails-with-the-authentication-or-decryption-has-failed
    // Fixes why downloads failed from dropbox
    public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
        bool isOk = true;
        // If there are errors in the certificate chain, look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None) {
            for (int i = 0; i < chain.ChainStatus.Length; i++) {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown) {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid) {
                        isOk = false;
                    }
                }
            }
        }
        return isOk;
    }

    // Works. FIXME: Next time, switch this over to jiasi cs server
    void downloadVideo() {
        Debug.Log("HERE");
        string url = "http://jiasiproj.cs.ucr.edu/Windows/1st%20floor%20of%20bourns/Synced%20Videos/Video1/0.kava.mp4"; //"https://www.dropbox.com/s/f19oblkapdq9oa0/bourns1-0.mp4?dl=1";
        //string filename = @"C:\Users\Daniel\Documents\Visual Studio 2015\Projects"
        //    + @"\ConsoleApplication1\ConsoleApplication1\bourns1.mp4";
        string filename = storagePath + "/bourns1.mp4";
        //ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        WebClient client = new WebClient();
        client.Proxy = new WebProxy("127.0.0.1", 8888); // Check that this works once server is up
        try {
            client.DownloadFile(url, filename);
        } catch (WebException we) {
            Debug.Log("WebException: " + we.Message);
        }
        Debug.Log("Finished Download");
    }

    void TestAllFunctions() {
        //Debug.Log("VideoDownloader: Starting Save Function");
        text.text += "VideoDownloader: Starting Save Function";
        StartCoroutine(SaveFromProxy());
    }

    // FIXME: Try threading and using C# classes to use proxy
    public IEnumerator SaveFromProxy() {
        string filename = Application.persistentDataPath + "/cafe-1.mp4";
        string url = "http://jiasiproj.cs.ucr.edu/Windows/1st%20floor%20of%20bourns/Synced%20Videos/Video1/0.kava.mp4";
        //Debug.Log("Proxy, In Save: " + filename + " from " + url);
        text.text += "Proxy, In Save: " + filename + " from " + url;

        float timeStart = Time.time;
        Network.proxyIP = "127.0.0.1";
        Network.proxyPort = 8888;
        Network.useProxy = true;
        
        FileStream file = File.Open(filename, FileMode.Create);
        WWW videoWWW = new WWW(url);
        yield return videoWWW;
        file.Write(videoWWW.bytes, 0, videoWWW.bytes.Length);
        if (!string.IsNullOrEmpty(videoWWW.error) && text != null)
            text.text += videoWWW.error;
        file.Close();

        Debug.Log("Done downloading and writing. Time to download: " + (Time.time - timeStart).ToString());
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

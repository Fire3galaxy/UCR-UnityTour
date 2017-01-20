using UnityEngine;
using System.Collections;

public class QuitApplication : MonoBehaviour {
    public static void Quit() {
#if UNITY_EDITOR    
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

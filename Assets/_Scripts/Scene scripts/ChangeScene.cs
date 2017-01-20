using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChangeScene : MonoBehaviour {
    public string sceneName;

    public void Move()
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}

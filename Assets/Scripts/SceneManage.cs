using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public void loadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void loadScene(int sceneID) {
        SceneManager.LoadScene(sceneID);
    }
}

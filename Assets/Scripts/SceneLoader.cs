using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // 原有：跳转Main场景
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    // 新增：跳转Game1场景
    public void LoadGame1Scene()
    {
        SceneManager.LoadScene("Game1");
    }

    // 新增：跳转Game2场景
    public void LoadGame2Scene()
    {
        SceneManager.LoadScene("Game2");
    }
}
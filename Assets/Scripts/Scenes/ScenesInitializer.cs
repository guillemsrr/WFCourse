using UnityEngine;
using UnityEngine.SceneManagement;

namespace WFCourse
{
    public class ScenesInitializer : MonoBehaviour
    {
        private const string UI_SCENE_NAME = "UIScene";
        private const string LEVEL_SCENE_NAME = "LevelScene";
        private void Awake()
        {
            SceneManager.LoadScene(UI_SCENE_NAME, LoadSceneMode.Additive);
            SceneManager.LoadScene(LEVEL_SCENE_NAME, LoadSceneMode.Additive);

            SceneManager.UnloadSceneAsync(0);
        }
    }
}

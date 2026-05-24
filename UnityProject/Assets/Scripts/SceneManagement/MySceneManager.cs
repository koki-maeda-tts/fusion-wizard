using CombinationMagician.ScriptableObjects;
using CombinationMagician.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CombinationMagician.SceneManagement
{
    /// <summary>
    /// シーンの読み込みを行うクラス.
    /// シーンに一つ、必ず配置してください
    /// </summary>
    public class MySceneManager : SceneSingleton<MySceneManager>
    {
        [SerializeField]
        SceneNames _sceneNames;

        public void LoadTitle() => SceneManager.LoadScene(_sceneNames.TitleSceneName);

        public void LoadGame() => SceneManager.LoadScene(_sceneNames.GameSceneName);

        public void LoadTutorial() => SceneManager.LoadScene(_sceneNames.TutorialSceneName);
    }
}

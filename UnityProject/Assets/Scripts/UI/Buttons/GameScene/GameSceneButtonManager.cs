using CombinationMagician.SceneManagement;
using CombinationMagician.Score;
using CombinationMagician.StoppableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CombinationMagician.UI.Buttons.GameScene
{
    /// <summary>
    /// ゲームシーンに配置されたボタンに押したときの挙動を設定するクラス
    /// </summary>
    public class GameSceneButtonManager : MonoBehaviour
    {
        [SerializeField, Header("ポーズ画面のパネル")]
        GameObject _pausePanel;

        [SerializeField, Header("タイトルボタン")]
        Button[] _titleButtons;

        [SerializeField, Header("ポーズボタン")]
        Button _pauseButton;

        [SerializeField]
        ScoreManager _scoreManager;

        bool _paused = false;

        void Start()
        {
            _pausePanel.SetActive(_paused);
            foreach (var b in _titleButtons)
            {
                SetListener(b, MySceneManager.Instance.LoadTitle);
            }
            SetListener(_pauseButton, PauseSwitch);
        }

        void Update()
        {
            // ゲームが終了していなければ、タブを押したときにメニューを表示する
            if (Keyboard.current.tabKey.wasPressedThisFrame && !_scoreManager.IsEndGame)
            {
                PauseSwitch();
            }
        }

        /// <summary>
        /// ポーズメニューを表示する
        /// </summary>
        void PauseSwitch()
        {
            _paused ^= true;
            _pausePanel.SetActive(_paused);
            if (_paused)
            {
                StoppableObjectManager.Instance.Pause();
            }
            else
            {
                StoppableObjectManager.Instance.ResumeFromPause();
            }
        }

        void SetListener(Button button, UnityAction action) => button.onClick.AddListener(action);
    }
}

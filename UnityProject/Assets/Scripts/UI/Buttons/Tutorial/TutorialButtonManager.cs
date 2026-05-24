using System.Collections.Generic;
using System.Linq;
using CombinationMagician.SceneManagement;
using CombinationMagician.StoppableObjects;
using CombinationMagician.Tutorial;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CombinationMagician.UI.Buttons.Tutorial
{
    /// <summary>
    /// チュートリアルシーンに配置されたボタンに押したときの挙動を設定するクラス
    /// </summary>
    public class TutorialButtonManager : MonoBehaviour
    {
        [SerializeField, Header("ポーズ画面のパネル")]
        GameObject _pausePanel;

        [SerializeField, Header("タイトルボタン")]
        Button _titleButton;

        [SerializeField, Header("ポーズボタン")]
        Button _pauseButton;

        [SerializeField, Header("プレイヤーのコンポーネント")]
        StoppableObject[] _playerComponents;

        /// <summary>
        /// プレイヤーのコンポーネントのHashSet
        /// </summary>
        HashSet<StoppableObject> _playerComponentSet;

        [SerializeField]
        TutorialManager _tutorialManager;

        public bool Paused { get; private set; } = false;

        void Start()
        {
            _pausePanel.SetActive(Paused);
            SetListener(_titleButton, MySceneManager.Instance.LoadTitle);
            SetListener(_pauseButton, PauseSwitch);
            _playerComponentSet = _playerComponents.ToHashSet();
        }

        void Update()
        {
            if (Keyboard.current.tabKey.wasPressedThisFrame)
            {
                PauseSwitch();
            }
        }

        /// <summary>
        /// ポーズメニューを表示する
        /// </summary>
        void PauseSwitch()
        {
            Paused ^= true;
            Time.timeScale = Paused ? 0 : 1;
            _pausePanel.SetActive(Paused);
            SetOperable(Paused);
        }

        /// <summary>
        /// プレイヤーのコンポーネント以外を停止する.
        /// プレイヤーはチュートリアルマネージャーで止められていなければ止める
        /// </summary>
        /// <param name="isPause"></param>
        void SetOperable(bool isPause)
        {
            foreach (var o in StoppableObjectManager.Instance.Objects)
            {
                if (_playerComponentSet.Contains(o) && !_tutorialManager.IsPlayable)
                {
                    continue;
                }
                if (isPause)
                {
                    o.Stop();
                }
                else
                {
                    o.Resume();
                }
            }
        }

        void SetListener(Button button, UnityAction action) => button.onClick.AddListener(action);
    }
}

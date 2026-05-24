using System.Linq;
using CombinationMagician.Players.WeaponSwitch;
using CombinationMagician.SceneManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CombinationMagician.UI.Buttons.Title
{
    /// <summary>
    /// タイトルシーンに配置されたボタンに押したときの挙動を設定するクラス
    /// </summary>
    public class TitleButtonManager : MonoBehaviour
    {
        [SerializeField, Header("ゲームを開始するボタン")]
        Button _start;

        [SerializeField, Header("チュートリアルを開始するボタン")]
        Button _tutorial;

        [SerializeField, Header("ゲームを終了するボタン")]
        Button _endButton;

        [SerializeField, Header("設定を開くボタン")]
        Button _settingOpen;

        [SerializeField, Header("設定を閉じるボタン")]
        Button _settingClose;

        [SerializeField, Header("設定UIのパネル")]
        GameObject _settingsPanel;

        [SerializeField, Header("マウスホイールによる魔法の切り替え方向を設定するボタン")]
        Button _reverseMouseDir;

        [SerializeField, Header("チェックボックスのスプライト")]
        Sprite[] _reverseMouseDirSprites;

        [SerializeField, Header("クレジットを開くボタン")]
        Button _creditsOpen;

        [SerializeField, Header("クレジットを閉じるボタン")]
        Button _creditsClose;

        [SerializeField, Header("クレジットパネル")]
        GameObject _creditsPanel;

        [SerializeField, Header("ライセンスを開くボタン")]
        Button _licenseOpen;

        [SerializeField, Header("ライセンスを閉じるボタン")]
        Button _licenseClose;

        [SerializeField, Header("ライセンスパネル")]
        GameObject _licensePanel;

        [SerializeField, Header("ライセンスが記載されたオブジェクト")]
        GameObject[] _licenses;

        [SerializeField, Header("次のライセンスを表示するボタン")]
        Button _licenseNext;

        [SerializeField, Header("前のライセンスを表示するボタン")]
        Button _licenseBack;

        /// <summary>
        /// 現在表示しているライセンスのインデックス
        /// </summary>
        private int _licenseIndex = 0;

        void Start()
        {
            // 各ボタンの挙動を設定
            SetListener(_start, MySceneManager.Instance.LoadGame);
            SetListener(_tutorial, MySceneManager.Instance.LoadTutorial);
            SetListener(
                _endButton,
                () =>
                {
#if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
                }
            );
            SetListener(_settingOpen, () => _settingsPanel.SetActive(true));
            SetListener(_settingClose, () => _settingsPanel.SetActive(false));
            SetCheckBoxSprite();
            SetListener(
                _reverseMouseDir,
                () =>
                {
                    WeaponSwitcher.IsReverseMouseDir ^= true;
                    SetCheckBoxSprite();
                }
            );
            SetListener(_creditsOpen, () => _creditsPanel.SetActive(true));
            SetListener(_creditsClose, () => _creditsPanel.SetActive(false));
            SetListener(_licenseOpen, () => _licensePanel.SetActive(true));
            SetListener(_licenseClose, () => _licensePanel.SetActive(false));
            SetListener(
                _licenseNext,
                () =>
                {
                    _licenses[_licenseIndex].SetActive(false);
                    _licenses[++_licenseIndex].SetActive(true);
                    SetLicenseButtonState();
                }
            );
            SetListener(
                _licenseBack,
                () =>
                {
                    _licenses[_licenseIndex].SetActive(false);
                    _licenses[--_licenseIndex].SetActive(true);
                    SetLicenseButtonState();
                }
            );
            // 設定とクレジットを非表示にする
            _settingsPanel.SetActive(false);
            _creditsPanel.SetActive(false);
            // ライセンス表示に関わるUIの状態を初期化する
            _licensePanel.SetActive(false);
            SetLicenseButtonState();
            foreach (var license in _licenses)
            {
                license.SetActive(false);
            }
            if (_licenses.Length > 0)
            {
                _licenses[0].SetActive(true);
            }
        }

        /// <summary>
        /// 次/前のライセンスを表示するボタンの状態を設定する
        /// </summary>
        void SetLicenseButtonState()
        {
            _licenseNext.gameObject.SetActive(_licenseIndex < _licenses.Length - 1);
            _licenseBack.gameObject.SetActive(_licenseIndex > 0);
        }

        /// <summary>
        /// 武器を変更するマウスホイールの操作に関する設定のチェックボックスのスプライトを設定する
        /// </summary>
        void SetCheckBoxSprite() =>
            _reverseMouseDir.image.sprite = WeaponSwitcher.IsReverseMouseDir
                ? _reverseMouseDirSprites[0]
                : _reverseMouseDirSprites[1];

        void SetListener(Button button, UnityAction action) => button.onClick.AddListener(action);
    }
}

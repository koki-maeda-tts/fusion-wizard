using CombinationMagician.StoppableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CombinationMagician.Players.WeaponSwitch
{
    /// <summary>
    /// どの魔法を選択しているか管理する
    /// </summary>
    public class WeaponSwitcher : StoppableObject
    {
        [SerializeField]
        [Header(
            "使用魔法を選択するボタンのオブジェクト.\n"
                + "PlayerParameters.Magicsの個数と順番に対応させるようにしてください"
        )]
        GameObject[] _buttonObjs;
        Button[] _buttons;

        [Header("選択されていないときのスプライト")]
        [SerializeField]
        Sprite _normalSprite;

        [SerializeField]
        SpriteState _normalSpriteState;

        [Header("選択されたときのスプライト")]
        [SerializeField]
        Sprite _selectedSprite;

        [SerializeField]
        SpriteState _selectedSpriteState;

        /// <summary>
        /// _buttons[0.._allowIndex]の範囲のボタンが使える
        /// </summary>
        int _allowIndex = 1;

        /// <summary>
        /// 次の魔法が存在するときtrue
        /// </summary>
        public bool IsNextMagicExists => _allowIndex < _buttonObjs.Length;

        /// <summary>
        /// 選択しているボタン(魔法)のインデックス.
        /// </summary>
        public int SelectedIndex { get; private set; }

        /// <summary>
        /// マウスホイールによる魔法の切り替え方向を反転するときはtrue
        /// </summary>
        public static bool IsReverseMouseDir { get; set; } = true;

        void Start()
        {
            Init();
        }

        void Update()
        {
#if UNITY_EDITOR
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                AllowNext();
            }
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                IsReverseMouseDir ^= true;
            }
#endif
            UpdateIndex();
        }

        /// <summary>
        /// 初期化を行う.
        /// ボタンそれぞれに押したときの処理を追加し、
        /// 0番目のボタンを押した状態にする
        /// </summary>
        void Init()
        {
            _buttons = new Button[_buttonObjs.Length];
            for (int i = 0; i < _buttonObjs.Length; i++)
            {
                if (i >= _allowIndex)
                {
                    _buttonObjs[i].SetActive(false);
                }
                _buttons[i] = _buttonObjs[i].GetComponent<Button>();
                int localI = i;
                _buttons[i].onClick.AddListener(() => Select(localI));
            }
            Select(0);
        }

        /// <summary>
        /// Q, Eを押したときにインデックスを更新する
        /// </summary>
        void UpdateIndex()
        {
            int i = 0;
            var kb = Keyboard.current;
            var my = Mouse.current?.scroll.ReadValue().y;
            if (
                kb.eKey.wasPressedThisFrame
                || (my != null && my != 0 && (my > 0 ^ IsReverseMouseDir))
            )
            {
                i = 1;
            }
            else if (
                kb.qKey.wasPressedThisFrame
                || (my != null && my != 0 && (my < 0 ^ IsReverseMouseDir))
            )
            {
                i = -1;
            }
            if (i == 0)
            {
                return;
            }
            var selected = (SelectedIndex + i + _allowIndex) % _allowIndex;
            Select(selected);
        }

        /// <summary>
        /// i番目のボタンを選択し、選択していたボタンを解除する.
        /// </summary>
        void Select(int i)
        {
            var button = _buttons[SelectedIndex];
            button.image.sprite = _normalSprite;
            button.spriteState = _normalSpriteState;
            SelectedIndex = i;
            button = _buttons[SelectedIndex];
            button.image.sprite = _selectedSprite;
            button.spriteState = _selectedSpriteState;
        }

        /// <summary>
        /// 次のボタン(魔法)を使えるようにする
        /// </summary>
        public void AllowNext()
        {
            _buttonObjs[_allowIndex].SetActive(true);
            _buttons[_allowIndex].enabled = true;
            _allowIndex++;
        }

        public override void Stop()
        {
            enabled = false;
            foreach (var b in _buttons)
            {
                b.enabled = false;
            }
        }

        public override void Resume()
        {
            enabled = true;
            for (int i = 0; i < _allowIndex; i++)
            {
                _buttons[i].enabled = true;
            }
        }
    }
}

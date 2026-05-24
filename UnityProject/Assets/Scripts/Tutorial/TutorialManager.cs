using System;
using System.Linq;
using CombinationMagician.EditorExtensions;
using CombinationMagician.Players;
using CombinationMagician.Players.Weapons;
using CombinationMagician.Players.WeaponSwitch;
using CombinationMagician.ScriptableObjects;
using CombinationMagician.StoppableObjects;
using CombinationMagician.UI.Buttons.Tutorial;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static CombinationMagician.Players.Weapons.PlayerWeapon;
using static CombinationMagician.Tutorial.TutorialManager.TransitionConditions;

namespace CombinationMagician.Tutorial
{
    /// <summary>
    /// チュートリアルの進行を管理するクラス
    /// </summary>
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField]
        Player _player;

        /// <summary>
        /// プレイヤーが持つコンポーネントのうち、<see cref="StoppableObject"/>であるもの
        /// </summary>
        StoppableObject[] _players;
        Rigidbody2D _playerRb;
        LineRenderer _playerLine;

        [SerializeField]
        TutorialButtonManager _tutorialButtonManager;

        [SerializeField]
        WeaponSwitcher _weaponSwitcher;

        [SerializeField, Header("タイマーのUI")]
        TMP_Text _timerText;

        /// <summary>
        /// チュートリアルが始まった時間
        /// </summary>
        float _stdSeconds;

        [SerializeField]
        TutorialTexts _texts;

        [SerializeField]
        TMP_Text _textBox;

        [SerializeField, Header("テキスト送りの速さ(秒)")]
        float _textForwardingSecond;

        [SerializeField, Tag]
        string _combinationMagicTag;

        [SerializeField, Tag]
        string _bottunUITag;

        [SerializeField]
        Scarecrow[] _scarecrows;

        /// <summary>
        /// プレイヤーを操作できるときtrue
        /// </summary>
        public bool IsPlayable { get; private set; } = false;

        /// <summary>
        /// 移動した時間(秒)
        /// </summary>
        float _moveSeconds;

        /// <summary>
        /// 次のテキストに遷移する条件
        /// </summary>
        public enum TransitionConditions
        {
            NextButton, // 対応するキーを押す/クリックする
            Move, // 一定量以上移動する
            HitScarecrow, // かかしに攻撃する
            HitFire, // かかしに炎魔法をぶつける
            CombineMagic, // 合体魔法を生成する
            End, // テキスト終了
        }

        void Start()
        {
            _stdSeconds = Time.time;
            _players = _player.GetComponents<StoppableObject>();
            _playerRb = _player.GetComponent<Rigidbody2D>();
            _playerLine = _player.GetComponent<LineRenderer>();
            _playerLine.enabled = false;
            _player.IsAttackable = false;
            CombinationMagicGenerator.Instance.IsActive = false;
            Tutorial().Forget();
        }

        void Update()
        {
            _timerText.text = $"{Time.time - _stdSeconds:f2}s";
        }

        /// <summary>
        /// チュートリアルを進行する.
        /// テキストごとに、設定された条件を確認する
        /// </summary>
        async UniTask Tutorial()
        {
            await UniTask.Yield(cancellationToken: destroyCancellationToken);
            SetPlayerOperable(false);
            foreach (var i in _texts.Infos)
            {
                // テキストを表示
                await TextForwarding(_textBox, i.Text, _textForwardingSecond);
                // 遷移条件によってゲームの状態を変更する
                switch (i.Condition)
                {
                    case Move:
                        SetPlayerOperable(true);
                        _moveSeconds = 0;
                        break;
                    case HitScarecrow or HitFire:
                        SetPlayerOperable(true);
                        _playerLine.enabled = true;
                        _player.IsAttackable = true;
                        if (i.Condition == HitFire)
                        {
                            _weaponSwitcher.AllowNext();
                        }
                        foreach (var s in _scarecrows)
                        {
                            s.IsHit = false;
                        }
                        break;
                    case CombineMagic:
                        SetPlayerOperable(true);
                        CombinationMagicGenerator.Instance.IsActive = true;
                        break;
                }
                // 次のテキストに遷移する条件を確認する
                await UniTask.WaitUntil(
                    ConditionCheckFunc(i.Condition),
                    cancellationToken: destroyCancellationToken
                );
                // チュートリアルが終わったらプレイ可能にする
                SetPlayerOperable(i.Condition == End);
            }
        }

        /// <summary>
        /// それぞれの条件をチェックする関数を返す
        /// </summary>
        /// <exception cref="ArgumentException">存在しない遷移条件</exception>
        Func<bool> ConditionCheckFunc(TransitionConditions conditions) =>
            conditions switch
            {
                NextButton => () =>
                {
                    var selected = EventSystem.current.currentSelectedGameObject;
                    return (
                        !_tutorialButtonManager.Paused
                        && (
                            Mouse.current.leftButton.wasPressedThisFrame
                            || Keyboard.current.enterKey.wasPressedThisFrame
                        )
                        && (selected == null || !selected.CompareTag(_bottunUITag))
                    );
                },
                Move => () =>
                {
                    Debug.Log(_moveSeconds);
                    if (MathF.Abs(_playerRb.linearVelocityX) > 0)
                    {
                        _moveSeconds += Time.deltaTime;
                    }
                    return _moveSeconds >= 5; // 5秒以上動いたらtrue
                },
                HitScarecrow => () => _scarecrows.All(s => s.IsHit),
                HitFire => () =>
                    _scarecrows.All(s =>
                        s.IsHit && (((int)s.MagicAttribute) & (int)MagicAttribute.Fire) != 0
                    ),
                CombineMagic => () =>
                    GameObject.FindGameObjectWithTag(_combinationMagicTag) != null,
                End => () => true,
                _ => throw new ArgumentException("無効な値です"),
            };

        /// <summary>
        /// 文字送りする
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        /// <param name="textForwardingSecond">文字送りする間隔</param>
        async UniTask TextForwarding(TMP_Text tMP_Text, string text, float textForwardingSecond)
        {
            tMP_Text.text = text;
            for (int i = 0; i < text.Length; i++)
            {
                tMP_Text.maxVisibleCharacters = i + 1;
                await UniTask.WaitForSeconds(
                    textForwardingSecond,
                    cancellationToken: destroyCancellationToken
                );
            }
        }

        /// <summary>
        /// プレイヤーを操作不可能にする
        /// </summary>
        /// <param name="isActive">操作できるときtrue</param>
        void SetPlayerOperable(bool isActive)
        {
            IsPlayable = isActive;
            foreach (var o in _players)
            {
                if (isActive)
                {
                    o.Resume();
                }
                else
                {
                    o.Stop();
                }
            }
        }

        void OnDestroy()
        {
            _players = null;
            _scarecrows = null;
        }
    }
}

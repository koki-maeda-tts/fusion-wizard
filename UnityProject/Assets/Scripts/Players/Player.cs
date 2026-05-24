using CombinationMagician.Audio;
using CombinationMagician.Players.WeaponSwitch;
using CombinationMagician.Score;
using CombinationMagician.ScriptableObjects;
using CombinationMagician.StoppableObjects;
using Cysharp.Threading.Tasks;
using GetOnlyPropertyGenerator;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CombinationMagician.Players
{
    public partial class Player : StoppableObject
    {
        [SerializeField, GenerateGetOnlyProperty]
        PlayerParameters _para;

        [SerializeField]
        WeaponSwitcher _weaponSwitcher;

        /// <summary>
        /// プレイヤーからマウスまでのベクトル
        /// </summary>
        Vector2 _playerToMouse;

        /// <summary>
        /// プレイヤーからマウスまでの、制限角度を考慮したベクトル
        /// </summary>
        public Vector2 RestrictedPlayerToMouse { get; private set; }

        /// <summary>
        /// マウスをクリックしたときのプレイヤーからマウスまでのベクトル
        /// </summary>
        public Vector2 P2mWhenPressed { get; private set; }

        /// <summary>
        /// 投射速さ
        /// </summary>
        public float ProjectionSpd { get; private set; }

        /// <summary>
        /// 現在HP
        /// </summary>
        float _hp;

        /// <summary>
        /// 攻撃できるときtrue
        /// </summary>
        public bool IsAttackable { get; set; } = true;

        /// <summary>
        /// マウスを左クリックしたときtrue
        /// </summary>
        public bool IsPressMouse { get; private set; } = false;

#if UNITY_EDITOR
        [SerializeField, Header("法線")]
        LineRenderer _debugNormalVec;
#endif

        Rigidbody2D _rb;
        SpriteRenderer _spriteRenderer;

        [SerializeField]
        PlayerClothesInfo[] _playerClothesInfos;
        Animator _animator;

        [SerializeField, Header("HPスライダー")]
        Slider _hpSlider;

        [SerializeField, Header("クールダウンスライダー")]
        Slider _coolDownSlider;

        [SerializeField]
        ScoreManager _scoreManager;

        readonly int _walkAnimId = Animator.StringToHash("Walk");

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _hpSlider.value = _hpSlider.maxValue = _hp = _para.InitHp;

            _coolDownSlider.value = _coolDownSlider.maxValue = 1;
        }

        void Update()
        {
            UpdatePlayerToMouse();
            Move();
            Projection();

#if UNITY_EDITOR
            // スローモーション(デバッグ)
            if (Keyboard.current.tKey.wasPressedThisFrame)
            {
                Time.timeScale = 0.1f;
            }
            else if (Keyboard.current.tKey.wasReleasedThisFrame)
            {
                Time.timeScale = 1.0f;
            }
#endif
        }

        /// <summary>
        /// プレイヤーからマウスまでのベクトルを更新する
        /// </summary>
        void UpdatePlayerToMouse()
        {
            var mpos = UnityEngine.Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            mpos.z = 0;
            _playerToMouse = mpos - transform.position;
            var deg = Mathf.Atan2(_playerToMouse.y, _playerToMouse.x) * Mathf.Rad2Deg;
            if (_para.LimitAngle < deg && deg < 180 - _para.LimitAngle)
            {
                deg = deg <= 90 ? _para.LimitAngle : 180 - _para.LimitAngle;
            }
            else if (deg < 0)
            {
                deg = deg < -90 ? 180 : 0;
            }
            RestrictedPlayerToMouse =
                new Vector2(Mathf.Cos(deg * Mathf.Deg2Rad), Mathf.Sin(deg * Mathf.Deg2Rad))
                * _playerToMouse.magnitude;
        }

        /// <summary>
        /// 移動する
        /// </summary>
        void Move()
        {
            _animator.SetBool(_walkAnimId, true);
            var kb = Keyboard.current;
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed)
            {
                _rb.linearVelocityX = -_para.MoveSpeed;
                FlipSprites(false);
            }
            else if (kb.dKey.isPressed || kb.rightArrowKey.isPressed)
            {
                _rb.linearVelocityX = _para.MoveSpeed;
                FlipSprites(true);
            }
            else
            {
                _rb.linearVelocityX = 0;
                _animator.SetBool(_walkAnimId, false);
            }
        }

        /// <summary>
        /// 魔法を投射する
        /// </summary>
        void Projection()
        {
            // クールタイム中は攻撃できない
            if (!IsAttackable)
            {
                return;
            }
            // マウスが指しているオブジェクトを取得
            var selected = EventSystem.current.currentSelectedGameObject;
            // 左クリックの状態を取得
            var mouseLeft = Mouse.current.leftButton;
            // 魔法選択ボタン以外の場所を押したときは攻撃を始める
            if (
                mouseLeft.wasPressedThisFrame
                && (selected == null || !selected.CompareTag(_para.ButtonUiTag))
            )
            {
                IsPressMouse = true;
                P2mWhenPressed = RestrictedPlayerToMouse;
#if UNITY_EDITOR
                _debugNormalVec.SetPosition(
                    1,
                    new Vector2(-P2mWhenPressed.y, P2mWhenPressed.x).normalized * 10
                );
#endif
            }
            // マウスをクリックしながらドラッグしているとき、投射速度を変える
            // マウスがプレイヤーに近いほど速く投射するようにする
            if (mouseLeft.isPressed && IsPressMouse)
            {
                float dis = Mathf.Abs(Vector2.Dot(P2mWhenPressed.normalized, _playerToMouse));
                ProjectionSpd =
                    (1 - dis / P2mWhenPressed.magnitude) * _para.AddendProjectionSpd
                    + _para.InitProjectionSpd;
            }
            // 左クリックを離したとき、魔法を発射する
            if (mouseLeft.wasReleasedThisFrame && IsPressMouse)
            {
                IsPressMouse = false;
                var obj = Instantiate(
                    _para.Magics[_weaponSwitcher.SelectedIndex],
                    transform.position,
                    Quaternion.identity
                );
                var rb = obj.GetComponent<Rigidbody2D>();
                rb.linearVelocity = P2mWhenPressed.normalized * ProjectionSpd;
                Interval().Forget();
            }
        }

        /// <summary>
        /// 攻撃可能になるまで待つ
        /// </summary>
        async UniTask Interval()
        {
            IsAttackable = false;
            float t = Time.time;
            while (Time.time - t <= _para.CoolSeconds)
            {
                _coolDownSlider.value = (Time.time - t) / _para.CoolSeconds;
                await UniTask.Yield(cancellationToken: destroyCancellationToken);
            }
            _coolDownSlider.value = _coolDownSlider.maxValue;
            IsAttackable = true;
        }

        /// <summary>
        /// ダメージを受ける
        /// </summary>
        public void TakeDamage(float damage)
        {
            _hp -= damage;
            _hpSlider.value = _hp;
            HideSprites();
            SEManager.Instance.PlayOneShot(_para.DamageSE);
            if (_hp <= 0)
            {
                _scoreManager.EndGame();
            }
        }

        /// <summary>
        /// 段々と衣服が消えていくような演出を行うために、
        /// 残りHPに応じてスプライトを非表示にする
        /// </summary>
        void HideSprites()
        {
            foreach (var i in _playerClothesInfos)
            {
                if (_hp < i.DisplayThreshold)
                {
                    i.SpriteRenderer.enabled = false;
                }
            }
        }

        /// <summary>
        /// スプライトのフリップを設定する
        /// </summary>
        void FlipSprites(bool flip)
        {
            _spriteRenderer.flipX = flip;
            foreach (var i in _playerClothesInfos)
            {
                i.SpriteRenderer.flipX = flip;
            }
        }

        public override void Stop()
        {
            enabled = false;
            _animator.enabled = false;
        }

        public override void Resume()
        {
            enabled = true;
            _animator.enabled = true;
            IsPressMouse = IsPressMouse && Mouse.current.leftButton.isPressed;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _playerClothesInfos = null;
        }
    }
}

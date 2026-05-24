using CombinationMagician.Audio;
using CombinationMagician.Enemies.Interfaces;
using CombinationMagician.Players;
using CombinationMagician.ScriptableObjects.Enemies;
using CombinationMagician.StoppableObjects;
using CombinationMagician.UI.CriticalParticle;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static CombinationMagician.Players.Weapons.PlayerWeapon;

namespace CombinationMagician.Enemies
{
    /// <summary>
    /// 各敵用クラスの基底となるクラス
    /// </summary>
    /// <typeparam name="Parameters">敵に設定するパラメータ</typeparam>
    public abstract class Enemy<Parameters> : StoppableObject, IEnemy
        where Parameters : EnemyParameters
    {
        [SerializeField]
        protected Parameters _para;

        [SerializeField]
        protected Player _player;

        /// <summary>
        /// 現在HP
        /// </summary>
        protected float _hp;

        /// <summary>
        /// ダメージをプレイヤーに与えられるときtrue
        /// </summary>
        bool _canInflictDamage = true;
        bool _isDead = false;

        protected Rigidbody2D _rb;
        protected Collider2D _collider2D;
        protected SpriteRenderer _spriteRenderer;
        protected Animator _animator;

        /// <summary>
        /// オーバーライドする場合は最初にbase.Start()を呼び出してください
        /// </summary>
        protected virtual void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
#if UNITY_EDITOR
            if (_hp == 0)
            {
                _hp = _para.BaseHp;
            }
#endif
        }

        /// <summary>
        /// オーバーライドする場合は最初にbase.Update()を呼び出してください
        /// </summary>
        protected virtual void Update()
        {
            if (_hp <= 0)
            {
                DestroyedProcessing();
            }
            if (transform.position.y <= -20)
            {
                DestroyThis();
            }
        }

        public void Init(Player playerManager, float addendHp)
        {
            _player = playerManager;
            _hp = _para.BaseHp + addendHp;
        }

        public void TakeDamage(float damage, MagicAttribute magicAttribute)
        {
            if ((((int)magicAttribute) & (int)_para.WeaknessAttribute) != 0)
            {
                // 弱点属性だった時
                _hp -= damage * _para.WeaknessMultiplier;
                CriticalParticleManager.Instance.CreateParticle(_para.ParticlePos, transform);
            }
            else
            {
                _hp -= damage;
            }
        }

        /// <summary>
        /// 被撃破処理を行う.HPが0以下であるときに実行されます
        /// </summary>
        void DestroyedProcessing()
        {
            if (_isDead)
            {
                return;
            }
            _isDead = true;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _rb.linearVelocity = Vector2.zero;
            _collider2D.enabled = false;
            _animator.SetBool(_para.DestroyAnimParaName, true);
            SEManager.Instance.PlayOneShot(_para.DestroySE);
        }

        public void DestroyThis() => Destroy(gameObject);

        public override void Stop()
        {
            enabled = false;
            _animator.enabled = false;
        }

        public override void Resume()
        {
            enabled = true;
            _animator.enabled = true;
        }

        void OnCollisionStay2D(Collision2D collision) =>
            InflictDamageOnPlayer(collision.gameObject).Forget();

        void OnTriggerStay2D(Collider2D collision) =>
            InflictDamageOnPlayer(collision.gameObject).Forget();

        /// <summary>
        /// 引数のオブジェクトがプレイヤーであればダメージを与える
        /// </summary>
        async UniTask InflictDamageOnPlayer(GameObject gameObject)
        {
            if (_canInflictDamage && gameObject.CompareTag("Player"))
            {
                gameObject.GetComponent<Player>().TakeDamage(_para.CollisionDamage);
                // ダメージを受けた直後はダメージを受け付けないようにする
                _canInflictDamage = false;
                await UniTask.WaitForSeconds(0.5f, cancellationToken: destroyCancellationToken);
                _canInflictDamage = true;
            }
        }
    }
}

using CombinationMagician.Audio;
using CombinationMagician.EditorExtensions;
using CombinationMagician.Enemies.Interfaces;
using GetOnlyPropertyGenerator;
using UnityEngine;

namespace CombinationMagician.Players.Weapons
{
    /// <summary>
    /// プレイヤーが投射する魔法のクラス
    /// </summary>
    public partial class PlayerWeapon : MonoBehaviour
    {
        [SerializeField, Tag, Header("敵のタグ")]
        string _enemyTag;

        [SerializeField, Tag, Header("魔法のタグ")]
        string _magicTag;

        [SerializeField, Header("自分自身の魔法の属性"), GenerateGetOnlyProperty]
        MagicAttribute _selfMagicAttribute;

        [SerializeField, Header("基本ダメージ")]
        float _damage;

        [SerializeField, Header("ダメージ増加量/s")]
        float _increaseAmount;

        /// <summary>
        /// 現在のダメージ量
        /// </summary>
        public float CurrentDamage => _damage + _addendDamage;

        [SerializeField, Header("消滅アニメーションのパラメータ名")]
        string _animParaName;

        [SerializeField, Header("着弾時のSE")]
        AudioClip _audioClip;
        Rigidbody2D _rb;
        Animator _animator;
        float _addendDamage = 0;

        /// <summary>
        /// 魔法の属性
        /// </summary>
        public enum MagicAttribute
        {
            Stone = 1 << 1,
            Fire = 1 << 2,
            Water = 1 << 3,
        }

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (transform.position.y <= -20)
            {
                Destroy(gameObject);
            }
            _addendDamage += _increaseAmount * Time.deltaTime;
        }

        /// <summary>
        /// 敵に衝突したときはダメージを与える.
        /// 他の魔法と衝突したときは合体魔法を生成する.
        /// その後、自身を削除するためのアニメーションを再生する
        /// </summary>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var obj = collision.gameObject;
            InflictDamageOnEnemy(obj);
            if (obj.CompareTag(_magicTag))
            {
                var other = obj.GetComponent<PlayerWeapon>();
                var attribute = other.SelfMagicAttribute;
                var x = transform.position.x;
                var damage = CurrentDamage + other.CurrentDamage;
                if (_selfMagicAttribute == MagicAttribute.Stone && attribute == MagicAttribute.Fire)
                {
                    CombinationMagicGenerator.Instance.CreateMeteorite(x, damage);
                }
                else if (
                    _selfMagicAttribute == MagicAttribute.Stone
                    && attribute == MagicAttribute.Water
                )
                {
                    CombinationMagicGenerator.Instance.CreateIce(x, damage);
                }
                else if (
                    _selfMagicAttribute == MagicAttribute.Fire
                    && attribute == MagicAttribute.Water
                )
                {
                    CombinationMagicGenerator.Instance.CreateTornado(x, damage);
                }
            }
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _animator.SetTrigger(_animParaName);
            Debug.Log("damage: " + (CurrentDamage));
        }

        private void OnTriggerEnter2D(Collider2D collision) =>
            InflictDamageOnEnemy(collision.gameObject);

        void InflictDamageOnEnemy(GameObject gameObject)
        {
            if (gameObject.CompareTag(_enemyTag))
            {
                gameObject.GetComponent<IEnemy>().TakeDamage(CurrentDamage, _selfMagicAttribute);
            }
            SEManager.Instance.PlayOneShot(_audioClip);
        }

        /// <summary>
        /// ゲームオブジェクトを消去する.
        /// 消滅アニメーションが終了した後に呼び出してください
        /// </summary>
        public void DestroyThis() => Destroy(gameObject);
    }
}

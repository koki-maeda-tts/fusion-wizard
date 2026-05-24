using System.Collections.Generic;
using CombinationMagician.Audio;
using CombinationMagician.EditorExtensions;
using CombinationMagician.Enemies.Interfaces;
using UnityEngine;
using static CombinationMagician.Players.Weapons.PlayerWeapon;

namespace CombinationMagician.Players.Weapons.CombinationMagics
{
    /// <summary>
    /// 隕石の合体魔法のクラス
    /// </summary>
    public class MeteoriteMagic : MonoBehaviour, ICombinationMagic
    {
        [SerializeField, Tag, Header("地面のタグ")]
        string _groundTag;

        [SerializeField, Tag, Header("敵のタグ")]
        string _enemyTag;

        [SerializeField, Header("消滅アニメーションを再生するパラメータ名")]
        string _animParaName;

        /// <summary>
        /// 衝突したオブジェクト
        /// </summary>
        readonly List<GameObject> _collisionObjs = new();

        /// <summary>
        /// 敵に与えるダメージ
        /// </summary>
#if UNITY_EDITOR
        [SerializeField]
#endif
        float _damage;

        [SerializeField, Header("落下する高さ")]
        float _posY;

        [SerializeField, Header("生成時のSE")]
        AudioClip _audioClip;

        Rigidbody2D _rb;
        Animator _animator;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public void Init(float posX, float damage)
        {
            _damage = damage;
            transform.position = new Vector2(posX, _posY);
            SEManager.Instance.PlayOneShot(_audioClip);
        }

        /// <summary>
        /// ゲームオブジェクトを削除する.消滅アニメーションの終了後に呼び出してください
        /// </summary>
        public void DestroyThis() => Destroy(gameObject);

        void OnTriggerEnter2D(Collider2D collision)
        {
            var obj = collision.gameObject;
            if (obj.CompareTag(_groundTag))
            {
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                _animator.SetTrigger(_animParaName);
                return;
            }
            else if (obj.CompareTag(_enemyTag))
            {
                if (_collisionObjs.Contains(obj))
                {
                    return;
                }
                _collisionObjs.Add(obj);
                var enemy = obj.GetComponent<IEnemy>();
                enemy.TakeDamage(_damage, MagicAttribute.Fire | MagicAttribute.Stone);
            }
        }

        void OnDestroy()
        {
            _collisionObjs.Clear();
        }
    }
}

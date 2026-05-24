using System.Collections.Generic;
using CombinationMagician.Audio;
using CombinationMagician.EditorExtensions;
using CombinationMagician.Enemies.Interfaces;
using UnityEngine;
using static CombinationMagician.Players.Weapons.PlayerWeapon;

namespace CombinationMagician.Players.Weapons.CombinationMagics
{
    /// <summary>
    /// 氷の合体魔法のクラス
    /// </summary>
    public class IceMagic : MonoBehaviour, ICombinationMagic
    {
        [SerializeField, Tag, Header("敵のタグ")]
        string _enemyTag;

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

        [SerializeField, Header("生成する高さ")]
        float _posY;

        [SerializeField, Header("速さ調整係数(a, b)")]
        Vector2 _ab;

        [SerializeField, Header("速さを変更する時間(秒)")]
        float _t;

        [SerializeField, Header("生成時のSE")]
        AudioClip _audioClip;

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
            if (obj.CompareTag(_enemyTag))
            {
                if (_collisionObjs.Contains(obj))
                {
                    return;
                }
                _collisionObjs.Add(obj);
                var enemy = obj.GetComponent<IEnemy>();
                enemy.TakeDamage(_damage, MagicAttribute.Stone | MagicAttribute.Water);
                var adj = obj.GetComponent<IMoveSpeedAdjustableEnemy>();
                adj?.AdjustSpeed(_ab.x, _ab.y, _t);
            }
        }

        void OnDestroy()
        {
            _collisionObjs.Clear();
        }
    }
}

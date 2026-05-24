using CombinationMagician.Audio;
using CombinationMagician.EditorExtensions;
using CombinationMagician.Players;
using UnityEngine;

namespace CombinationMagician.Enemies.Individual
{
    /// <summary>
    /// ドラゴン(<see cref="Dragon"/>)が攻撃用に発射する火球のクラス
    /// </summary>
    public class DragonBreath : MonoBehaviour
    {
        [SerializeField, Tag, Header("プレイヤーのタグ")]
        string _playerTag;

        [SerializeField, Header("与えるダメージ")]
        float _damage;

        [SerializeField, Header("消滅アニメーションのパラメータ名")]
        string _animParaName;

        [SerializeField, Header("着弾時のSE")]
        AudioClip _audioClip;

        Rigidbody2D _rb;
        Collider2D _collider;
        Animator _animator;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// アニメーション終了後に呼び出す
        /// </summary>
        public void DestroyThis() => Destroy(gameObject);

        /// <summary>
        /// 衝突したときは自身を削除する.
        /// プレイヤーに衝突していればダメージを与える
        /// </summary>
        /// <param name="collision"></param>
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(_playerTag))
            {
                var player = collision.gameObject.GetComponent<Player>();
                player.TakeDamage(_damage);
            }
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _collider.enabled = false;
            _animator.SetTrigger(_animParaName);
            SEManager.Instance.PlayOneShot(_audioClip);
        }
    }
}

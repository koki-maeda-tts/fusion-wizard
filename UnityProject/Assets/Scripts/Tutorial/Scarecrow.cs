using CombinationMagician.EditorExtensions;
using CombinationMagician.Players.Weapons;
using UnityEngine;

namespace CombinationMagician.Tutorial
{
    /// <summary>
    /// チュートリアルで出てくるかかしのクラス
    /// </summary>
    public class Scarecrow : MonoBehaviour
    {
        [SerializeField, Tag]
        string _magicTag;

        /// <summary>
        /// プレイヤーの攻撃があたったらtrue
        /// </summary>
        public bool IsHit { get; set; } = false;

        /// <summary>
        /// 当たった魔法の属性(ビットフラグ)
        /// </summary>
        public PlayerWeapon.MagicAttribute MagicAttribute { get; private set; }

        /// <summary>
        /// 当たった魔法の属性を設定する
        /// </summary>
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(_magicTag))
            {
                IsHit = true;
                MagicAttribute |= collision
                    .gameObject.GetComponent<PlayerWeapon>()
                    .SelfMagicAttribute;
            }
        }
    }
}

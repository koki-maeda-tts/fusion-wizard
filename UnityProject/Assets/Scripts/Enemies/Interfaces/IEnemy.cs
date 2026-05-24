using CombinationMagician.Players;
using CombinationMagician.Players.Weapons;

namespace CombinationMagician.Enemies.Interfaces
{
    /// <summary>
    /// 敵が持つ共通インターフェース
    /// </summary>
    public interface IEnemy
    {
        /// <summary>
        /// 初期化を行う.Instantiateでの生成後は必ず実行してください
        /// </summary>
        /// <param name="addendHp">追加HP</param>
        void Init(Player playerManager, float addendHp);

        /// <summary>
        /// ダメージを受ける
        /// </summary>
        /// <param name="damage">受けるダメージ量</param>
        /// <param name="magicAttribute">魔法の属性</param>
        void TakeDamage(float damage, PlayerWeapon.MagicAttribute magicAttribute);

        /// <summary>
        /// ゲームオブジェクトを消去する.消滅アニメーションの後に呼び出してください
        /// </summary>
        void DestroyThis();
    }
}

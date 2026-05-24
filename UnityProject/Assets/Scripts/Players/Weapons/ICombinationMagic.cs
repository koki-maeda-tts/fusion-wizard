namespace CombinationMagician.Players.Weapons
{
    /// <summary>
    /// 生成する合体魔法のインターフェース
    /// </summary>
    public interface ICombinationMagic
    {
        /// <summary>
        /// 初期化を行う.Instantiateで生成した後は必ず呼び出してください
        /// </summary>
        /// <param name="posX">生成するx座標</param>
        /// <param name="damage">敵に与えるダメージ</param>
        void Init(float posX, float damage);
    }
}

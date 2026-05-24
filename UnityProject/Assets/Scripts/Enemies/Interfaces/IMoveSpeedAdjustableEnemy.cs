namespace CombinationMagician.Enemies.Interfaces
{
    /// <summary>
    /// 移動速度をプレイヤーから変更させることができる敵が持つインターフェース
    /// </summary>
    public interface IMoveSpeedAdjustableEnemy
    {
        /// <summary>
        /// 移動速度を変更する.
        /// t秒の間、元の移動速度をvとして、移動速度をav+bにします
        /// </summary>
        void AdjustSpeed(float a, float b, float t);
    }
}

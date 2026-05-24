using CombinationMagician.Utility;
using UnityEngine;

namespace CombinationMagician.UI.CriticalParticle
{
    /// <summary>
    /// プレイヤーの魔法が敵の弱点属性を付いたときに表示する
    /// パーティクルを生成するためのクラス
    /// </summary>
    public class CriticalParticleManager : SceneSingleton<CriticalParticleManager>
    {
        [SerializeField]
        ParticleSystem _particle;

        /// <summary>
        /// クリティカルパーティクルを生成する
        /// </summary>
        /// <param name="genPos">生成位置</param>
        /// <param name="parent">パーティクルの親にするトランスフォーム</param>
        public void CreateParticle(Vector2 genPos, Transform parent)
        {
            var p = Instantiate(_particle, parent);
            p.transform.localPosition = genPos;
        }
    }
}

using System;
using GetOnlyPropertyGenerator;
using UnityEngine;

namespace CombinationMagician.Players
{
    /// <summary>
    /// プレイヤーの衣服の情報
    /// </summary>
    [Serializable]
    public partial class PlayerClothesInfo
    {
        [SerializeField, Header("衣服のスプライトのオブジェクト"), GenerateGetOnlyProperty]
        SpriteRenderer _spriteRenderer;

        [
            SerializeField,
            Header("服が表示されるHPの閾値.閾値ちょうどでは表示される"),
            GenerateGetOnlyProperty
        ]
        float _displayThreshold;
    }
}

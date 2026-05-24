using GetOnlyPropertyGenerator;
using UnityEngine;

namespace CombinationMagician.ScriptableObjects.Enemies.Individual
{
    [CreateAssetMenu(
        fileName = "MandragoraParameters",
        menuName = "Scriptable Objects/Enemies/Individual/MandragoraParameters"
    )]
    public partial class MandragoraParameters : MovableEnemyParameters
    {
        [SerializeField, Range(0, 90), Header("ジャンプ時の角度"), GenerateGetOnlyProperty]
        float _deg;

        [SerializeField, Header("上昇中のスプライト"), GenerateGetOnlyProperty]
        Sprite _upSprite;

        [SerializeField, Header("下降中のスプライト"), GenerateGetOnlyProperty]
        Sprite _downSprite;

        [SerializeField, Header("スプライトを変更する速さの閾値(正の値)"), GenerateGetOnlyProperty]
        float _thresholdSpd;

        [SerializeField, Header("ジャンプの間隔(秒)"), GenerateGetOnlyProperty]
        float _waitSeconds;
    }
}

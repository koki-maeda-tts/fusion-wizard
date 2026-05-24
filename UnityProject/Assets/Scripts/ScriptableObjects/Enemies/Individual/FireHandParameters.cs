using GetOnlyPropertyGenerator;
using UnityEngine;

namespace CombinationMagician.ScriptableObjects.Enemies.Individual
{
    [CreateAssetMenu(
        fileName = "FireHandParameters",
        menuName = "Scriptable Objects/Enemies/Individual/FireHandParameters"
    )]
    public partial class FireHandParameters : MovableEnemyParameters
    {
        [SerializeField, Header("プレイヤーに衝突し始めるx軸距離の閾値"), GenerateGetOnlyProperty]
        float _thresholdDisX;
    }
}

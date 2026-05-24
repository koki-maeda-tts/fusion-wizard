using GetOnlyPropertyGenerator;
using UnityEngine;

namespace CombinationMagician.ScriptableObjects.Enemies.Individual
{
    [CreateAssetMenu(
        fileName = "BatParameters",
        menuName = "Scriptable Objects/Enemies/Individual/BatParameters"
    )]
    public partial class BatParameters : MovableEnemyParameters
    {
        [SerializeField, Header("プレイヤーに衝突し始めるx軸距離の閾値"), GenerateGetOnlyProperty]
        float _thresholdDisX;
    }
}

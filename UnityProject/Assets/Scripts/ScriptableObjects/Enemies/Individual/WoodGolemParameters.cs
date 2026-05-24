using GetOnlyPropertyGenerator;
using UnityEngine;

namespace CombinationMagician.ScriptableObjects.Enemies.Individual
{
    [CreateAssetMenu(
        fileName = "WoodGolemParameters",
        menuName = "Scriptable Objects/Enemies/Individual/WoodGolemParameters"
    )]
    public partial class WoodGolemParameters : MovableEnemyParameters
    {
        [
            SerializeField,
            Header("歩行アニメーションを再生するパラメータの名前"),
            GenerateGetOnlyProperty
        ]
        string _walkAnimParaName;

        [SerializeField, Header("歩行する秒数"), GenerateGetOnlyProperty]
        float _walkTime;

        [SerializeField, Header("立ち止まる秒数"), GenerateGetOnlyProperty]
        float _idleTime;
    }
}

using GetOnlyPropertyGenerator;
using UnityEngine;

namespace CombinationMagician.ScriptableObjects.Enemies.Individual
{
    [CreateAssetMenu(
        fileName = "DragonParameters",
        menuName = "Scriptable Objects/Enemies/Individual/DragonParameters"
    )]
    public partial class DragonParameters : MovableEnemyParameters
    {
        [SerializeField, Header("射出するオブジェクト"), GenerateGetOnlyProperty]
        GameObject _fireBall;

        [SerializeField, Header("攻撃の射出速さ"), GenerateGetOnlyProperty]
        float _projectionSpd;

        [SerializeField, Header("攻撃を生成する位置"), GenerateGetOnlyProperty]
        Vector2 _createPos;

        [
            SerializeField,
            Header("攻撃アニメーションを再生するパラメータの名前"),
            GenerateGetOnlyProperty
        ]
        string _fireAnimParaName;

        [SerializeField, Header("攻撃の間隔(秒)"), GenerateGetOnlyProperty]
        float _waitSeconds;
    }
}

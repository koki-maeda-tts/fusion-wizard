using GetOnlyPropertyGenerator;
using UnityEngine;

namespace CombinationMagician.ScriptableObjects.Enemies
{
    /// <summary>
    /// 動く敵に関するパラメータ
    /// </summary>
    [CreateAssetMenu(
        fileName = "MovableEnemyParameters",
        menuName = "Scriptable Objects/Enemies/MovableEnemyParameters"
    )]
    public partial class MovableEnemyParameters : EnemyParameters
    {
        [SerializeField, Header("移動速さ"), GenerateGetOnlyProperty]
        float _moveSpeed;
    }
}

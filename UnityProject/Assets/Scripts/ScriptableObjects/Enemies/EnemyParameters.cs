using CombinationMagician.Players.Weapons;
using GetOnlyPropertyGenerator;
using UnityEngine;

namespace CombinationMagician.ScriptableObjects.Enemies
{
    /// <summary>
    /// 敵に関するパラメータ
    /// </summary>
    [CreateAssetMenu(
        fileName = "EnemyParameters",
        menuName = "Scriptable Objects/Enemies/EnemyParameters"
    )]
    public partial class EnemyParameters : ScriptableObject
    {
        [SerializeField, Header("基礎HP"), GenerateGetOnlyProperty]
        float _baseHp;

        [
            SerializeField,
            Header("衝突したときにプレイヤーに与えるダメージ"),
            GenerateGetOnlyProperty
        ]
        float _collisionDamage;

        [SerializeField, Header("弱点属性"), GenerateGetOnlyProperty]
        PlayerWeapon.MagicAttribute _weaknessAttribute;

        [SerializeField, Header("弱点属性の倍率"), GenerateGetOnlyProperty]
        float _weaknessMultiplier;

        [SerializeField, Header("消滅アニメーションのパラメータ名"), GenerateGetOnlyProperty]
        string _destroyAnimParaName;

        [SerializeField, Header("パーティクルを生成する位置"), GenerateGetOnlyProperty]
        Vector2 _particlePos;

        [SerializeField, Header("被撃破時のSE"), GenerateGetOnlyProperty]
        AudioClip _destroySE;
    }
}

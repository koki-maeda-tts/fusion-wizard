using System;
using GetOnlyPropertyGenerator;
using UnityEngine;

namespace CombinationMagician.Enemies.Spawner
{
    /// <summary>
    /// 敵を生成するために必要な情報
    /// </summary>
    [Serializable]
    public partial class EnemyInfo
    {
        [SerializeField, Header("敵のゲームオブジェクト"), GenerateGetOnlyProperty]
        GameObject _obj;

        [SerializeField, Header("生成位置"), GenerateGetOnlyProperty]
        Vector2 _position;

        [SerializeField, Header("生成するたびに追加されていくHP"), GenerateGetOnlyProperty]
        float _addendHp;

        [SerializeField, Header("最大(初期)生成間隔(秒)"), GenerateGetOnlyProperty]
        float _maxSpawnIntervalSeconds;

        [SerializeField, Header("最小生成間隔(秒)"), GenerateGetOnlyProperty]
        float _minSpawnIntervalSeconds;

        [
            SerializeField,
            Header("生成間隔が最小になるまでにかかる時間(秒)"),
            GenerateGetOnlyProperty
        ]
        float _minimumSeconds;

        [SerializeField, Header("生成間隔のランダムなずれ幅(%)"), GenerateGetOnlyProperty]
        float _spawnRandomDeviationRatio;

        [SerializeField, Header("次の敵が現れるまでの時間"), GenerateGetOnlyProperty]
        float _nextEnemySeconds;
    }
}

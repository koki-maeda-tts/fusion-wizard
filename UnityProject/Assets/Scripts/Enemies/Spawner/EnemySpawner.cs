using CombinationMagician.Enemies.Interfaces;
using CombinationMagician.Players;
using CombinationMagician.Players.WeaponSwitch;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CombinationMagician.Enemies.Spawner
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        Player _player;

        [SerializeField]
        WeaponSwitcher _weaponSwitcher;

        [SerializeField, Header("生成する敵.登場順で追加してください")]
        EnemyInfo[] _enemyInfos;

        void Start()
        {
            SpawnLoopStarter().Forget();
        }

        /// <summary>
        /// 敵ごとの生成ループを開始するループ
        /// </summary>
        async UniTask SpawnLoopStarter()
        {
            for (int i = 0; i < _enemyInfos.Length; i++)
            {
                SpawnLoop(_enemyInfos[i]).Forget();
                if (i != 0 && i % 2 == 0 && _weaponSwitcher.IsNextMagicExists)
                {
                    _weaponSwitcher.AllowNext();
                }
                await UniTask.WaitForSeconds(
                    _enemyInfos[i].NextEnemySeconds,
                    cancellationToken: destroyCancellationToken
                );
            }
        }

        /// <summary>
        /// 敵を生成するループ
        /// </summary>
        async UniTask SpawnLoop(EnemyInfo enemyInfo)
        {
            // 追加するHP、生成間隔、生成間隔の減少率を初期化
            float addendHp = 0;
            float spawnSeconds = enemyInfo.MaxSpawnIntervalSeconds;
            float decreaseRate =
                (enemyInfo.MinSpawnIntervalSeconds - enemyInfo.MaxSpawnIntervalSeconds)
                / enemyInfo.MinimumSeconds;

            // 敵を生成する
            while (true)
            {
                var obj = Instantiate(enemyInfo.Obj, enemyInfo.Position, Quaternion.identity);
                var enemy = obj.GetComponent<IEnemy>();
                enemy.Init(_player, addendHp);
                var waitSeconds =
                    spawnSeconds
                    * (1 + enemyInfo.SpawnRandomDeviationRatio * 0.01f * Random.Range(-1f, 1f));
                Debug.Log($"wait s, add hp: {waitSeconds}, {addendHp}");
                await UniTask.WaitForSeconds(
                    waitSeconds,
                    cancellationToken: destroyCancellationToken
                );
                addendHp += enemyInfo.AddendHp;
                spawnSeconds = Mathf.Max(
                    spawnSeconds + decreaseRate * waitSeconds,
                    enemyInfo.MinSpawnIntervalSeconds
                );
            }
        }
    }
}

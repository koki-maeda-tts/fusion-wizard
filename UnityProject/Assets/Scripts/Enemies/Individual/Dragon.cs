using CombinationMagician.ScriptableObjects.Enemies.Individual;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CombinationMagician.Enemies.Individual
{
    /// <summary>
    /// ドラゴンの敵のクラス
    /// </summary>
    public class Dragon : MoveSpeedAdjustableEnemy<DragonParameters>
    {
        protected override void Start()
        {
            base.Start();
            AttackLoop().Forget();
        }

        protected override void Update()
        {
            base.Update();
            _rb.linearVelocityX = -_moveSpdOffset; // 左向きに進むため符号反転
        }

        /// <summary>
        /// 指定値ごとにプレイヤーに攻撃を行う
        /// </summary>
        async UniTask AttackLoop()
        {
            while (true)
            {
                await UniTask.WaitForSeconds(
                    _para.WaitSeconds,
                    cancellationToken: destroyCancellationToken
                );
                StartFire();
            }
        }

        /// <summary>
        /// プレイヤーへの攻撃を開始する
        /// </summary>
        void StartFire() => _animator.SetTrigger(_para.FireAnimParaName);

        /// <summary>
        /// プレイヤーに攻撃する
        /// </summary>
        /// <remarks>
        /// 攻撃をするタイミングで、アニメーションから呼び出してください
        /// </remarks>
        public void Fire()
        {
            var obj = Instantiate(
                _para.FireBall,
                (Vector2)transform.position + _para.CreatePos,
                Quaternion.identity
            );
            obj.GetComponent<Rigidbody2D>().linearVelocity =
                (_player.transform.position - transform.position).normalized * _para.ProjectionSpd;
        }
    }
}

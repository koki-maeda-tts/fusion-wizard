using CombinationMagician.ScriptableObjects.Enemies.Individual;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CombinationMagician.Enemies.Individual
{
    /// <summary>
    /// ゴブリンの敵のクラス
    /// </summary>
    public class Goblin : MoveSpeedAdjustableEnemy<GoblinParameters>
    {
        protected override void Start()
        {
            base.Start();
            MoveLoop().Forget();
        }

        /// <summary>
        /// プレイヤーに方に移動する.
        /// 指定時間ごとに、移動と停止を繰り返す
        /// </summary>
        async UniTask MoveLoop()
        {
            while (true)
            {
                float t = Time.time;
                while (Time.time - t < _para.WalkTime)
                {
                    MoveToPlayer();
                    await UniTask.Yield(cancellationToken: destroyCancellationToken);
                }

                Idle();
                await UniTask.WaitForSeconds(
                    _para.IdleTime,
                    cancellationToken: destroyCancellationToken
                );
            }
        }

        /// <summary>
        /// アニメーターの歩行パラメーターを false に設定して待機状態にする
        /// </summary>
        void Idle()
        {
            _animator.SetBool(_para.WalkAnimParaName, false);
        }

        /// <summary>
        /// プレイヤーの方に移動する.
        /// 移動速度は
        /// <see cref="MoveSpeedAdjustableEnemy{T}._moveSpdMultiplier"/>と
        /// <see cref="MoveSpeedAdjustableEnemy{T}._moveSpdOffset"/>を
        /// 考慮する
        /// </summary>
        void MoveToPlayer()
        {
            bool isLeft = _player.transform.position.x < transform.position.x;
            float spd = _para.MoveSpeed * _moveSpdMultiplier + _moveSpdOffset;
            if (isLeft)
            {
                spd *= -1;
            }
            _rb.linearVelocityX = spd;
            _spriteRenderer.flipX = !isLeft;
            _animator.SetBool(_para.WalkAnimParaName, true);
        }
    }
}

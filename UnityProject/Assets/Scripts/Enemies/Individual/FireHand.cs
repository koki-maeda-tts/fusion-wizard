using CombinationMagician.ScriptableObjects.Enemies.Individual;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CombinationMagician.Enemies.Individual
{
    /// <summary>
    /// 火の手の敵のクラス
    /// </summary>
    public class FireHand : Enemy<FireHandParameters>
    {
        protected override void Start()
        {
            base.Start();
            _rb.constraints =
                RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            MoveLoop().Forget();
        }

        /// <summary>
        /// プレイヤーに近づく.
        /// まずプレイヤーとのx軸距離が指定値を下回るまで真横に進み、
        /// その後プレイヤーの位置に向かって進む
        /// </summary>
        /// <returns></returns>
        async UniTask MoveLoop()
        {
            while (
                Mathf.Abs(_player.transform.position.x - transform.position.x) > _para.ThresholdDisX
            )
            {
                MoveToPlayerX();
                await UniTask.Yield(cancellationToken: destroyCancellationToken);
            }
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            while (true)
            {
                MoveToPlayerStraight();
                await UniTask.Yield(cancellationToken: destroyCancellationToken);
            }
        }

        /// <summary>
        /// プレイヤーに向かって真横に進む
        /// </summary>
        void MoveToPlayerX()
        {
            bool isLeft = _player.transform.position.x < transform.position.x;
            float spd = _para.MoveSpeed;
            if (isLeft)
            {
                spd *= -1;
            }
            _rb.linearVelocityX = spd;
            _spriteRenderer.flipX = !isLeft;
        }

        /// <summary>
        /// プレイヤーに向かって一直線に進む
        /// </summary>
        void MoveToPlayerStraight()
        {
            _spriteRenderer.flipX = _player.transform.position.x >= transform.position.x;
            _rb.linearVelocity =
                (_player.transform.position - transform.position).normalized * _para.MoveSpeed;
        }
    }
}

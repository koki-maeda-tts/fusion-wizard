using CombinationMagician.ScriptableObjects.Enemies.Individual;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CombinationMagician.Enemies.Individual
{
    /// <summary>
    /// マンドラゴラの敵のクラス
    /// </summary>
    public class Mandragora : MoveSpeedAdjustableEnemy<MandragoraParameters>
    {
        Sprite _defaultSprite;

        protected override void Start()
        {
            base.Start();
            _defaultSprite = _spriteRenderer.sprite;
            MoveLoop().Forget();
        }

        protected override void Update()
        {
            base.Update();
            UpdateSprite();
        }

        /// <summary>
        /// プレイヤーの方に移動する.
        /// 指定時間ごとにジャンプと停止を繰り返す
        /// </summary>
        /// <returns></returns>
        async UniTask MoveLoop()
        {
            while (true)
            {
                JumpToPlayer();
                await UniTask.WaitForSeconds(
                    _para.WaitSeconds,
                    cancellationToken: destroyCancellationToken
                );
            }
        }

        /// <summary>
        /// プレイヤーの方に向かってジャンプする.
        /// ジャンプの大きさは
        /// <see cref="MoveSpeedAdjustableEnemy{T}._moveSpdMultiplier"/>と
        /// <see cref="MoveSpeedAdjustableEnemy{T}._moveSpdOffset"/>を
        /// 考慮する
        /// </summary>
        void JumpToPlayer()
        {
            bool isLeft = _player.transform.position.x < transform.position.x;
            var spd = _para.MoveSpeed * _moveSpdMultiplier + _moveSpdOffset;
            _rb.linearVelocity =
                new Vector2(
                    Mathf.Cos(_para.Deg * Mathf.Deg2Rad),
                    Mathf.Sin(_para.Deg * Mathf.Deg2Rad)
                ) * spd;
            if (isLeft)
            {
                _rb.linearVelocityX *= -1;
            }
            _spriteRenderer.flipX = !isLeft;
        }

        /// <summary>
        /// 速度ベクトルに合わせてスプライトを変更する
        /// </summary>
        void UpdateSprite()
        {
            if (_rb.linearVelocityY > _para.ThresholdSpd)
            {
                _spriteRenderer.sprite = _para.UpSprite;
            }
            else if (_rb.linearVelocityY < -_para.ThresholdSpd)
            {
                _spriteRenderer.sprite = _para.DownSprite;
            }
            else
            {
                _spriteRenderer.sprite = _defaultSprite;
            }
        }
    }
}

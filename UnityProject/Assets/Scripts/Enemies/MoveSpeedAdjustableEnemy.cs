using CombinationMagician.Enemies.Interfaces;
using CombinationMagician.ScriptableObjects.Enemies;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CombinationMagician.Enemies
{
    /// <summary>
    /// 移動速度をプレイヤーから変更させることができる敵の基底クラス
    /// </summary>
    /// <typeparam name="Parameters"></typeparam>
    public abstract class MoveSpeedAdjustableEnemy<Parameters>
        : Enemy<Parameters>,
            IMoveSpeedAdjustableEnemy
        where Parameters : MovableEnemyParameters
    {
        /// <summary>
        /// 元の速さに掛ける係数
        /// </summary>
        protected float _moveSpdMultiplier = 1;

        /// <summary>
        /// 元の速さに加える定数
        /// </summary>
        protected float _moveSpdOffset = 0;

        /// <summary>
        /// AdjustSpeedによって速さを変更された回数
        /// </summary>
        int _numberOfSpdChanges = 0;

        /// <summary>
        /// 加速したときの色
        /// </summary>
        readonly Color _accelerationColor = new(0.5882353f, 1, 0.4156863f);

        /// <summary>
        /// 減速したときの色
        /// </summary>
        readonly Color _decelerationColor = new(0.4156863f, 0.7333333f, 1);
        Material _material;

        protected override void Start()
        {
            base.Start();
            _material = _spriteRenderer.material;
        }

        public void AdjustSpeed(float a, float b, float t) => AdjustSpeedInner(a, b, t).Forget();

        /// <summary>
        /// t秒の間、元の移動速度をvとして、移動速度をav+bにします
        /// </summary>
        async UniTask AdjustSpeedInner(float a, float b, float t)
        {
            int i = ++_numberOfSpdChanges;
            _moveSpdMultiplier = a;
            _moveSpdOffset = b;
            var spd = _para.MoveSpeed * _moveSpdMultiplier + _moveSpdOffset;
            if (spd > _para.MoveSpeed)
            {
                _material.color = _accelerationColor;
            }
            else if (spd < _para.MoveSpeed)
            {
                _material.color = _decelerationColor;
            }
            await UniTask.WaitForSeconds(t, cancellationToken: destroyCancellationToken);
            if (i == _numberOfSpdChanges)
            {
                _moveSpdMultiplier = 1;
                _moveSpdOffset = 0;
                _material.color = Color.white;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Destroy(_material);
            _material = null;
        }
    }
}

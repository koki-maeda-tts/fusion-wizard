using CombinationMagician.StoppableObjects;
using UnityEngine;

namespace CombinationMagician.Players.PredictionLines
{
    /// <summary>
    /// 魔法の飛翔予測線を描画するためのクラス
    /// プレイヤーにアタッチする前提です.
    /// </summary>
    public class PredictionLine : StoppableObject
    {
        Player _player;
        LineRenderer _line;
        const float _GRAVITY = 9.80665f;
        readonly float _step = 0.1f;

        void Start()
        {
            _player = GetComponent<Player>();
            _line = GetComponent<LineRenderer>();
            InitLine();
        }

        /// <summary>
        /// line rendererを初期化する
        /// </summary>
        void InitLine()
        {
            _line.positionCount = 0;
            _line.startWidth = _line.endWidth = 0.2f;
            _line.useWorldSpace = false;
        }

        void Update()
        {
            UpdateLine();
        }

        /// <summary>
        /// 予測線を更新する
        /// </summary>
        void UpdateLine()
        {
            if (_player.IsPressMouse)
            {
                Predict(_player.P2mWhenPressed.normalized * _player.ProjectionSpd);
            }
            else
            {
                Predict(
                    _player.RestrictedPlayerToMouse.normalized * _player.Para.InitProjectionSpd
                );
            }
        }

        /// <summary>
        /// 飛翔軌跡の予測線を表示する
        /// </summary>
        void Predict(Vector2 velocity)
        {
            InitLine();
            var vec = Vector2.zero;
            float t = 0;
            while (t <= 1)
            {
                vec.x = velocity.x * t;
                vec.y = velocity.y * t - 0.5f * _GRAVITY * (t * t + t * Time.fixedDeltaTime);
                _line.positionCount++;
                _line.SetPosition(_line.positionCount - 1, vec);
                t += _step;
            }
        }

        public override void Stop() => enabled = false;

        public override void Resume() => enabled = true;
    }
}

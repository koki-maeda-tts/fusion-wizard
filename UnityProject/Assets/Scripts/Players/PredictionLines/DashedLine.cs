using CombinationMagician.StoppableObjects;
using UnityEngine;

namespace CombinationMagician.Players.PredictionLines
{
    /// <summary>
    /// 点線を表示するためのクラス.
    /// プレイヤーにアタッチする前提です
    /// </summary>
    public class DashedLine : StoppableObject
    {
        LineRenderer _lineRenderer;
        Material _material;

        [SerializeField, Header("表示される線の長さ")]
        float _length;

        [SerializeField, Header("非表示になる線の長さ")]
        float _space;
        static readonly int _lengthProperty = Shader.PropertyToID("_Length");
        static readonly int _spaceProperty = Shader.PropertyToID("_Space");

        void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _material = _lineRenderer.material;
        }

        void Update()
        {
            Refresh();
        }

        /// <summary>
        /// ラインの長さに応じて点線の間隔を調整する
        /// </summary>
        void Refresh()
        {
            var positions = new Vector3[_lineRenderer.positionCount];
            _lineRenderer.GetPositions(positions);
            float totalLength = 0;
            for (int i = 1; i < positions.Length; i++)
            {
                totalLength += Vector3.Distance(positions[i], positions[i - 1]);
            }

            var ratio = 1 / totalLength;
            var lengthRatio = _length * ratio;
            var spaceRatio = _space * ratio;

            _material.SetFloat(_lengthProperty, lengthRatio);
            _material.SetFloat(_spaceProperty, spaceRatio);
        }

        public override void Stop() => enabled = false;

        public override void Resume() => enabled = true;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Destroy(_material);
            _material = null;
        }
    }
}

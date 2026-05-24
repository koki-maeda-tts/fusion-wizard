using System.Collections.Generic;
using CombinationMagician.Utility;
using UnityEngine;

namespace CombinationMagician.StoppableObjects
{
    /// <summary>
    /// <see cref="StoppableObject"/>を管理するためのクラス
    /// </summary>
    /// <remarks>
    /// <see cref="SceneSingleton{StoppableObjectManager}.Awake"/>よりも<see cref="StoppableObject.Awake"/>が
    /// 先に呼び出されてしまうことを防ぐため、
    /// プロジェクト設定のスクリプト実行順序を設定する必要があります
    /// </remarks>
    public class StoppableObjectManager : SceneSingleton<StoppableObjectManager>
    {
        public List<StoppableObject> Objects { get; set; } = new();

        /// <summary>
        /// 全ての<see cref="StoppableObject"/>を停止させ、<see cref="Time.timeScale"/>を0にする
        /// </summary>
        public void Pause()
        {
            foreach (var o in Objects)
            {
                o.Stop();
            }
            Time.timeScale = 0;
        }

        /// <summary>
        /// 全ての<see cref="StoppableObject"/>の動作を再開させ、<see cref="Time.timeScale"/>を1にする
        /// </summary>
        public void ResumeFromPause()
        {
            foreach (var o in Objects)
            {
                o.Resume();
            }
            Time.timeScale = 1;
        }

        void OnDestroy()
        {
            Objects.Clear();
            // ポーズ中にシーンが破棄された場合にタイムスケールを戻す
            Time.timeScale = 1;
        }
    }
}

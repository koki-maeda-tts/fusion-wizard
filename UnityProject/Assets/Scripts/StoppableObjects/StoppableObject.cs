using UnityEngine;

namespace CombinationMagician.StoppableObjects
{
    /// <summary>
    /// 動作を停止可能なオブジェクトはこれを継承する
    /// </summary>
    public abstract class StoppableObject : MonoBehaviour
    {
        /// <summary>
        /// 動作を再開する
        /// </summary>
        public abstract void Resume();

        /// <summary>
        /// 動作を停止する
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// オーバーライドする場合は最初にbase.Awake()を呼び出してください
        /// </summary>
        protected virtual void Awake()
        {
            StoppableObjectManager.Instance.Objects.Add(this);
        }

        /// <summary>
        /// オーバーライドする場合は最初にbase.OnDestroy()を呼び出してください
        /// </summary>
        protected virtual void OnDestroy()
        {
            StoppableObjectManager.Instance.Objects.Remove(this);
        }
    }
}

#nullable enable
using System;
using UnityEngine;

namespace CombinationMagician.Utility
{
    /// <summary>
    /// シングルトンを作成する場合はこのクラスを継承する.
    /// ただし、シーンに渡って存在するシングルトンではなく、
    /// シーンが破棄されればこのクラスも破棄されることに注意
    /// </summary>
    public abstract class SceneSingleton<T> : MonoBehaviour
        where T : Component
    {
        private static T? _instance = null;
        public static T Instance =>
            _instance ?? throw new InvalidOperationException("インスタンスが初期化されていません");

        /// <summary>
        /// オーバーライドする場合は最初にbase.Awake()を呼び出してください
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = gameObject.GetComponent<T>();
                if (_instance == null)
                {
                    throw new InvalidOperationException("コンポーネントが存在しません");
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}

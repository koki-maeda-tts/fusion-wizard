using UnityEngine;

namespace CombinationMagician.Camera
{
    /// <summary>
    /// 画面のアスペクトを調整します
    /// </summary>
    [ExecuteAlways]
    class AspectKeeper : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.Camera _targetCamera; //対象とするカメラ

        [SerializeField]
        Vector2 _aspectVec; //目的解像度

        void Update()
        {
            var screenAspect = Screen.width / (float)Screen.height; //画面のアスペクト比
            var targetAspect = _aspectVec.x / _aspectVec.y; //目的のアスペクト比

            var magRate = targetAspect / screenAspect; //目的アスペクト比にするための倍率

            var viewportRect = new Rect(0, 0, 1, 1); //Viewport初期値でRectを作成

            if (magRate < 1)
            {
                viewportRect.width = magRate; //使用する横幅を変更
                viewportRect.x = 0.5f - viewportRect.width * 0.5f; //中央寄せ
            }
            else
            {
                viewportRect.height = 1 / magRate; //使用する縦幅を変更
                viewportRect.y = 0.5f - viewportRect.height * 0.5f; //中央寄せ
            }

            _targetCamera.rect = viewportRect; //カメラのViewportに適用
        }
    }
}

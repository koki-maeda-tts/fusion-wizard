using CombinationMagician.Audio;
using CombinationMagician.StoppableObjects;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombinationMagician.Score
{
    /// <summary>
    /// ゲームのスコアの記録と、その表示を行うクラス
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField, Header("タイムを表示するテキスト")]
        TMP_Text _timeText;

        /// <summary>
        /// ゲームを始めた時間
        /// </summary>
        float _startingTime;

        /// <summary>
        /// ゲームが始まってからの時間
        /// </summary>
        float CurrentScore => Time.time - _startingTime;
        public float ResultScore { get; private set; }

        /// <summary>
        /// ゲームが終了したらtrue
        /// </summary>
        public bool IsEndGame { get; private set; } = false;

        [SerializeField, Header("ゲーム中のキャンバス")]
        Canvas _gameCanvas;

        [SerializeField, Header("リザルト画面のキャンバス")]
        Canvas _resultCanvas;

        [SerializeField, Header("カメラのトランスフォーム")]
        Transform _camera;

        [SerializeField, Header("カメラが移動するY座標")]
        float _cameraDownYPos;

        [SerializeField, Header("カメラの移動にかける時間")]
        float _cameraAnimSeconds;

        [SerializeField, Header("BGMのオーディオソース")]
        AudioSource _bgmAudioSource;

        [SerializeField, Header("BGMを段々と小さくするのにかける時間")]
        float _bgmAnimSeconds;

        [SerializeField, Header("リザルト画面のテキスト")]
        TMP_Text _resultText;

        [SerializeField, Header("スコアを表示するテキスト")]
        TMP_Text _scoreText;

        [SerializeField, Header("結果を表示したあとに表示するボタン(タイトルに戻るボタンなど)")]
        Button[] _resultButtons;

        [SerializeField, Header("文字送りの間隔(秒)")]
        float[] _textForwardingSeconds;

        [SerializeField, Header("スコアを表示するときのSE")]
        AudioClip _scoreDisplaySE;

        void Start()
        {
            _startingTime = Time.time;
            _gameCanvas.enabled = true;
            _resultCanvas.enabled = false;
            foreach (var button in _resultButtons)
            {
                button.gameObject.SetActive(false);
            }
            _resultText.maxVisibleCharacters = 0;
            _scoreText.text = "";

#if UNITY_EDITOR
            if (_textForwardingSeconds.Length < 3)
            {
                Debug.LogError("配列の要素数が3を満たしていません");
            }
#endif
        }

        void Update()
        {
            UpdateTimeDisplay();
        }

        void UpdateTimeDisplay()
        {
            _timeText.text = $"{CurrentScore:f2}s";
        }

        public void EndGame() => EndGameAsync().Forget();

        /// <summary>
        /// プレイヤーのHPが0になったあと、スコアを表示する
        /// </summary>
        async UniTask EndGameAsync()
        {
            var dct = destroyCancellationToken;
            // ゲームの本処理を止める
            ResultScore = CurrentScore;
            IsEndGame = true;
            StoppableObjectManager.Instance.Pause();
            _gameCanvas.enabled = false;

            // リザルトUIを表示し、各アニメーションを再生
            _resultCanvas.enabled = true;
            var move = _camera
                .DOMoveY(_cameraDownYPos, _cameraAnimSeconds)
                .SetEase(Ease.InOutQuad)
                .SetUpdate(true)
                .ToUniTask(cancellationToken: destroyCancellationToken);
            var bgm = _bgmAudioSource
                .DOFade(0, _bgmAnimSeconds)
                .SetUpdate(true)
                .ToUniTask(cancellationToken: destroyCancellationToken);

            await UniTask.WhenAll(move, bgm);

            // 結果をテキストで表示
            await TextForwarding();
            await UniTask.WaitForSeconds(_textForwardingSeconds[1], true, cancellationToken: dct);

            _scoreText.text = $"{ResultScore:f2}s!";
            SEManager.Instance.PlayOneShot(_scoreDisplaySE);
            await UniTask.WaitForSeconds(_textForwardingSeconds[2], true, cancellationToken: dct);

            // 各ボタンを有効化
            foreach (var button in _resultButtons)
            {
                button.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 一定間隔で文字送りする
        /// </summary>
        async UniTask TextForwarding()
        {
            for (int i = 0; i < _resultText.text.Length; i++)
            {
                _resultText.maxVisibleCharacters = i + 1;
                await UniTask.WaitForSeconds(
                    _textForwardingSeconds[0],
                    true,
                    cancellationToken: destroyCancellationToken
                );
            }
        }
    }
}

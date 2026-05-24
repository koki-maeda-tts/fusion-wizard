using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace CombinationMagician.Audio
{
    public class SoundVolumeChanger : MonoBehaviour
    {
        [SerializeField]
        AudioMixer _audioMixer;

        [SerializeField]
        Slider _bgmVolume;

        [SerializeField, Header("BGMの音量のパラメータ名")]
        string _bgmVolumeParaName;

        [SerializeField]
        Slider _seVolume;

        [SerializeField, Header("SEの音量のパラメータ名")]
        string _seVolumeParaName;

        private void Start()
        {
            SetLister(_bgmVolume, _bgmVolumeParaName);
            SetLister(_seVolume, _seVolumeParaName);

            // Audio Mixerが設定されているか確認
            if (_audioMixer.GetFloat(_bgmVolumeParaName, out float bgmVolume))
            {
                _bgmVolume.value = Mathf.Pow(10, bgmVolume / 20);
            }
            else
            {
                Debug.LogError("BGMの音量を取得できませんでした");
            }
            if (_audioMixer.GetFloat(_seVolumeParaName, out float seVolume))
            {
                _seVolume.value = Mathf.Pow(10, seVolume / 20);
            }
            else
            {
                Debug.LogError("SEの音量を取得できませんでした");
            }
        }

        /// <summary>
        /// スライダーのonValueChangedを設定します。
        /// スライダーの値を対数で変換し、Audio Mixerに設定するようにします。
        /// </summary>
        /// <param name="paraName">設定したいAudio Mixerのパラメータ名</param>
        void SetLister(Slider slider, string paraName)
        {
            slider.onValueChanged.AddListener(value =>
            {
                value = Mathf.Clamp01(value); // スライダーの値の範囲は[0,1]
                float decibel = 20f * Mathf.Log10(value);
                decibel = Mathf.Clamp(decibel, -80f, 0f);
                _audioMixer.SetFloat(paraName, decibel);
            });
        }
    }
}

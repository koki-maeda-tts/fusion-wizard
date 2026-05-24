using CombinationMagician.Utility;
using UnityEngine;

namespace CombinationMagician.Audio
{
    public class SEManager : SceneSingleton<SEManager>
    {
        AudioSource _audioSource;

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayOneShot(AudioClip audioClip)
        {
            Debug.Log(audioClip);
            _audioSource.PlayOneShot(audioClip);
        }
    }
}

using UnityEngine;

namespace CombinationMagician.Audio
{
    public class ButtonSE : MonoBehaviour
    {
        public void PlayOneShot(AudioClip audioClip) => SEManager.Instance.PlayOneShot(audioClip);
    }
}

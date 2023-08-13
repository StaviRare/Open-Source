using UnityEngine;

namespace GLHF.AudioSound
{
    public class Example : MonoBehaviour
    {
        public AudioLibrary AudioLibrary;

        private float _timer = 0.0f;
        private float _interval = 1.0f;

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _interval)
            {
                PlaySFX();
                _timer = 0.0f;
            }
        }

        private void PlaySFX()
        {
            var audioClip = AudioLibrary.sfx_ui_button;

            if(audioClip == null)
            {
                Debug.LogWarning("Loaded audio clip \"sfx_ui_button\" is null!");
            }
            else
            {
                AudioSound.SFXFromScreen(AudioLibrary.sfx_ui_button);
            }
        }
    }
}
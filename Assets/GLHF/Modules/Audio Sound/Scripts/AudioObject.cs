using UnityEngine;

namespace GLHF.AudioSound
{
    public class AudioObject : MonoBehaviour
    {
        public AudioSource AudioSource;
        private bool _isPaused; // For game pause implementation

        private void Update()
        {
            var canDestroy = AudioSource.isPlaying == false && _isPaused == false;

            if (canDestroy)
            {
                Destroy(gameObject);
            }
        }
    }
}
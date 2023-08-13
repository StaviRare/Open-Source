using UnityEngine;

namespace GLHF.AudioSound
{
    [CreateAssetMenu(fileName = "AudioLibrary", menuName = "GLHF/Audio Library", order = 1)]
    public class AudioLibrary : ScriptableObject
    {
        [Header("SFX")]
        public AudioClip sfx_ui_button;
        public AudioClip sfx_ui_denied;

        [Header("Music")]
        public AudioClip music_gameplay_loop;
    }
}
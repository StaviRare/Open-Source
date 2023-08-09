using UnityEngine.Audio;

namespace GLHF.AudioSound
{
    public static class AudioMixerManager
    {
        public static AudioMixer MasterMixer { get; private set; }
        public static AudioMixerGroup SFXMixerGroup { get; private set; }
        public static AudioMixerGroup MusicMixerGroup { get; private set; }


        public static void SetMasterMixer(AudioMixer audioMixer)
        {
            MasterMixer = audioMixer;
        }

        public static void SetSFXMixerGroup(AudioMixerGroup mixerGroup)
        {
            SFXMixerGroup = mixerGroup;
        }

        public static void SetMusicMixerGroup(AudioMixerGroup mixerGroup)
        {
            MusicMixerGroup = mixerGroup;
        }
    }
}
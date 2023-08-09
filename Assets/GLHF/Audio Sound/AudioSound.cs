using UnityEngine;

namespace GLHF.AudioSound
{
    public static class AudioSound
    {
        // With audio mixer usage
        public static AudioSource SFXFromScreen(AudioClip sfx)
        {
            var audioSource = FromScreen(sfx);
            audioSource.outputAudioMixerGroup = AudioMixerManager.SFXMixerGroup;
            return audioSource;
        }

        public static AudioSource SFXFromWorld(AudioClip sfx, Vector3 pos)
        {
            var audioSource = FromWorld(sfx, pos);
            audioSource.outputAudioMixerGroup = AudioMixerManager.SFXMixerGroup;
            return audioSource;
        }

        public static AudioSource MusicFromScreen(AudioClip music)
        {
            var audioSource = FromScreen(music);
            audioSource.outputAudioMixerGroup = AudioMixerManager.MusicMixerGroup;
            return audioSource;
        }


        // Simple usage
        public static AudioSource FromScreen(AudioClip sfx)
        {
            var audioSource = CreateAudioObject(sfx.name);
            audioSource.spatialBlend = 0;

            PlayAudio(audioSource, sfx);

            return audioSource;
        }

        public static AudioSource FromObject(AudioClip sfx, Transform obj)
        {
            var audioSource = CreateAudioObject(sfx.name);
            audioSource.transform.position = obj.position;
            audioSource.spatialBlend = 1;

            PlayAudio(audioSource, sfx);

            return audioSource;
        }

        public static AudioSource FromWorld(AudioClip sfx, Vector3 pos)
        {
            var audioSource = CreateAudioObject(sfx.name);
            audioSource.transform.position = pos;
            audioSource.spatialBlend = 1;
            audioSource.minDistance = 35;
            audioSource.maxDistance = 35;
            audioSource.dopplerLevel = 0;
            PlayAudio(audioSource, sfx);

            return audioSource;
        }


        private static AudioSource CreateAudioObject(string sfxName)
        {
            var audioGameObject = new GameObject("Audio: " + sfxName);
            var audioSource = audioGameObject.AddComponent<AudioSource>();
            var audioObject = audioGameObject.AddComponent<AudioObject>();
            audioObject.AudioSource = audioSource;

            return audioSource;
        }

        private static void PlayAudio(AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
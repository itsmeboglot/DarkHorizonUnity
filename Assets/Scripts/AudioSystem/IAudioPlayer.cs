using UnityEngine;

namespace Game.Audio
{
    public interface IAudioPlayer
    {
        void PlaySound(SoundType type);
        void PlaySound(AudioSource audioSource, SoundType type);
        void StopSound();
        void PlayMusic(MusicType type);
        void PlayMusic(AudioSource audioSource, MusicType type);
        void StopMusic();
        void SetSoundVolume(float value);
        void SetMusicVolume(float value);
    }
}
using System;
using UnityEngine;
using Zenject;

namespace Game.Audio
{
  [Serializable]
  public class VolumeData
  {
    public float music = 1f;
    public float sound = 1f;
  }

  public class AudioPlayerProvider : IAudioPlayer, IInitializable
  {
    private readonly AudioManager _audioManager;
    private VolumeData _volumeData;

    public AudioPlayerProvider(AudioManager audioManager)
    {
      _audioManager = audioManager;
    }

    public void Initialize()
    {
      LoadVolumeData();
    }

    private void LoadVolumeData()
    {
      _volumeData = new VolumeData();

      _audioManager.SetMusicVolume(_volumeData.music);
      _audioManager.SetSoundVolume(_volumeData.sound);
    }

    public void PlaySound(SoundType type)
    {
      _audioManager.PlaySound(type);
    }

    public void PlaySound(AudioSource audioSource, SoundType type)
    {
      _audioManager.PlaySound(audioSource, type);
    }

    public void StopSound()
    {
      _audioManager.StopSound();
    }

    public void PlayMusic(MusicType type)
    {
      _audioManager.PlayMusic(type);
    }

    public void PlayMusic(AudioSource audioSource, MusicType type)
    {
      _audioManager.PlayMusic(audioSource, type);
    }

    public void StopMusic()
    {
      _audioManager.StopMusic();
    }

    public void SetSoundVolume(float value)
    {
      _audioManager.SetSoundVolume(value);
    }

    public void SetMusicVolume(float value)
    {
      _audioManager.SetMusicVolume(value);
    }
  }
}
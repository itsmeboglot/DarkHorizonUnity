using System;
using Game.Audio;
using UnityEngine;

namespace AudioSystem
{
  public class AudioCustomerTest : MonoBehaviour
  {
    [SerializeField] private AudioManager audioManager;
    [Range(0,1f)] [SerializeField] private float soundVolume;
    [Range(0,1f)] [SerializeField] private float musicVolume;
    
    [ContextMenu("Play Sound")]
    private void PlaySound()
    {
      audioManager.PlaySound(SoundType.GameOver);
    }
    
    [ContextMenu("Stop Sound")]
    private void StopSound()
    {
      audioManager.StopSound();
    }
    
    [ContextMenu("Play Music")]
    private void PlayMusic()
    {
      audioManager.PlayMusic(MusicType.BattleTheme);
    }
    
    [ContextMenu("Stop Music")]
    private void StopMusic()
    {
      audioManager.StopMusic();
    }
    
    [ContextMenu("Change Volume")]
    private void ChangeVolume()
    {
      audioManager.SetMusicVolume(musicVolume);
      audioManager.SetSoundVolume(soundVolume);
    }
  }
}

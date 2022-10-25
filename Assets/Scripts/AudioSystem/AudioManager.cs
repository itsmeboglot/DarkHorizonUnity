using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource soundSource;
        [SerializeField] private AudioMixer masterMixer;
        [SerializeField] private List<AudioClip> musicClips;
        [SerializeField] private List<AudioClip> soundClips;

        public void PlaySound(SoundType type)
        {
            soundSource.PlayOneShot(GetClip(type));
        }

        public void PlaySound(AudioSource audioSource, SoundType type)
        {
            audioSource.PlayOneShot(GetClip(type));
        }

        public void StopSound()
        {
            soundSource.Stop();
        }

        public void PlayMusic(AudioSource audioSource, MusicType type)
        {
            audioSource.clip = GetClip(type);
            audioSource.Play();
        }

        public async void PlayMusic(MusicType type)
        {
            if(musicSource.isPlaying)
                await StopMusic();

            musicSource.volume = 0f;
            musicSource.DOFade(1f, 1.5f);
            musicSource.clip = GetClip(type);
            musicSource.Play();
        }

        public async UniTask StopMusic()
        {
            bool isStopped = false;
            musicSource.DOFade(0f, 0.7f).OnComplete(() =>
            {
                musicSource.Stop();
                isStopped = true;
            });

            await UniTask.WaitUntil(() => isStopped);
        }

        public void SetSoundVolume(float value)
        {
            var outputValue = Mathf.Clamp(value, 0.0001f, 1f);
            masterMixer.SetFloat("SoundVolume", Mathf.Log10(outputValue) * 20f);
        }

        public void SetMusicVolume(float value)
        {
            var outputValue = Mathf.Clamp(value, 0.0001f, 1f);
            masterMixer.SetFloat("MusicVolume", Mathf.Log10(outputValue) * 20f);
        }

        private AudioClip GetClip(SoundType type)
        {
            return soundClips.FirstOrDefault(x => x.name == type.ToString());
        }

        private AudioClip GetClip(MusicType type)
        {
            return musicClips.FirstOrDefault(x => x.name == type.ToString());
        }
    }
}
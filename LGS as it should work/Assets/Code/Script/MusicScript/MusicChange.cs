using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Script.Music
{
    public class MusicChange : MonoBehaviour
    {
        private AudioSource _musicBox;
        private float _fadeOutDuration = 1f; 
        private float _fadeInDuration = 1f;  
        private float _startVolume;
        private Dictionary<AudioClip, float> _clipPositions = new Dictionary<AudioClip, float>();

        private void Start()
        { 
            GameObject musicBoxObject = GameObject.FindGameObjectWithTag("MusicBox");
            _musicBox = musicBoxObject.GetComponent<AudioSource>();
            _startVolume = _musicBox.volume;
        }

        public void SetMusicClip(AudioClip newClip)
        {
           
            if (_musicBox.clip != newClip)
            {
                StartCoroutine(FadeOutMusicAndChange(newClip));
            }
            else if (!_musicBox.isPlaying) // Если музыка уже загружена, но не играет
            {
                StartCoroutine(FadeInMusic()); // Продолжим воспроизведение с текущей позиции
            }

           
        }

        private IEnumerator FadeOutMusicAndChange(AudioClip newClip)
        {
            while (_musicBox.volume > 0)
            {
                _musicBox.volume -= _startVolume * Time.deltaTime / _fadeOutDuration;
                yield return null;
            }
            SaveCurrentMusicPosition();
            _musicBox.Stop();
            _musicBox.clip = newClip;
            RestoreMusicPosition(newClip);

            _musicBox.Play();
            StartCoroutine(FadeInMusic());
        }

        private IEnumerator FadeInMusic()
        {
            while (_musicBox.volume < _startVolume)
            {
                _musicBox.volume += _startVolume * Time.deltaTime / _fadeInDuration;
                yield return null;
            }
        }

        private void SaveCurrentMusicPosition()
        {
            if (_musicBox.clip != null)
            {
                _clipPositions[_musicBox.clip] = _musicBox.time;
            }
        }

        private void RestoreMusicPosition(AudioClip clipToPlay)
        {
            if (_clipPositions.TryGetValue(clipToPlay, out var savedTime))
            {
                _musicBox.time = savedTime;
            }
            else
            {
                _musicBox.time = 0; // Начать с начала, если позиция не была сохранена
            }
        }
    }
}

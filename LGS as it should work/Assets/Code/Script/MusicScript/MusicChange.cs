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
            else if (!_musicBox.isPlaying) // If the music is already loaded but not playing
            {
                StartCoroutine(FadeInMusic()); // Continue playing from the current position
            }
        }

        private IEnumerator FadeOutMusicAndChange(AudioClip newClip)
        {
            float fadeOutStep = _startVolume * Time.deltaTime / _fadeOutDuration;

            while (_musicBox.volume > 0)
            {
                _musicBox.volume -= fadeOutStep;
                yield return null;
            }

            _musicBox.volume = 0; // Ensure volume is set to 0

            SaveCurrentMusicPosition();
            _musicBox.Stop();
            _musicBox.clip = newClip;
            RestoreMusicPosition(newClip);

            yield return new WaitForSeconds(0.1f); // Slight delay before starting the new clip

            _musicBox.Play();
            StartCoroutine(FadeInMusic());
        }

        private IEnumerator FadeInMusic()
        {
            float fadeInStep = _startVolume * Time.deltaTime / _fadeInDuration;

            while (_musicBox.volume < _startVolume)
            {
                _musicBox.volume += fadeInStep;
                yield return null;
            }

            _musicBox.volume = _startVolume; // Ensure volume is set to the starting volume
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
                _musicBox.time = 0; // Start from the beginning if the position was not saved
            }
        }
        
        public void PauseMusic()
        {
            if (_musicBox.isPlaying)
            {
                _musicBox.Pause();
            }
        }

        public void ResumeMusic()
        {
            if (!_musicBox.isPlaying)
            {
                _musicBox.UnPause();
            }
        }
    }
}

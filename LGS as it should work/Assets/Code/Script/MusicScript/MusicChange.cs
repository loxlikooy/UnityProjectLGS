using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Script.Music
{
    public class MusicChange : MonoBehaviour
    {
        private AudioSource _musicBox;
        public AudioClip musicOnChange;
        
        
 
        private void Start()
        { 
            GameObject musicBoxObject = GameObject.FindGameObjectWithTag("MusicBox");
            _musicBox = musicBoxObject.GetComponent<AudioSource>();
        }

        public void MusicChangeOnStateChase()
        {
           _musicBox.Stop();
           _musicBox.clip = musicOnChange;
           _musicBox.Play();
        }
    }
}
﻿using Code.Script.Music;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; private set; }

    private MusicChange _musicChange;
    [SerializeField] private AudioClip chaseMusicClip; 
    [SerializeField] private AudioClip patrolMusicClip; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _musicChange = FindObjectOfType<MusicChange>();
    }

    public void PlayChaseMusic()
    {
        _musicChange.SetMusicClip(chaseMusicClip);
    }

    public void PlayPatrolMusic()
    {
        _musicChange.SetMusicClip(patrolMusicClip);
    }
    
    public void PauseMusic()
    {
        if (_musicChange != null)
        {
            _musicChange.PauseMusic();
        }
    }

    public void ResumeMusic()
    {
        if (_musicChange != null)
        {
            _musicChange.ResumeMusic();
        }
    }
}
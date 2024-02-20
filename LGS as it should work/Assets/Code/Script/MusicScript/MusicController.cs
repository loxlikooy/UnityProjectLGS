using Code.Script.Music;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; private set; }

    private MusicChange _musicChange;
    [SerializeField] private AudioClip chaseMusicClip; // Assign in the Inspector
    [SerializeField] private AudioClip patrolMusicClip; // Assign in the Inspector

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
}
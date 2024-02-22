using System;
using UnityEngine;

namespace Code.Script.MusicScript
{
    public class MusicFollow : MonoBehaviour
    {
        private Transform _player;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            transform.position = _player.transform.position;
        }
    }
}
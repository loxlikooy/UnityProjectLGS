using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Code.Script.Wolf
{
    public class Wolf : Enemy
    {
        private static bool shouldAttack = true;
        
        private void OnEnable()
        {
            Artifacts.OnArtifactWolfPickedUp += HandleArtifactPickedUp;
        }

        private void OnDisable()
        {
            Artifacts.OnArtifactWolfPickedUp -= HandleArtifactPickedUp;
        }
        
        private void HandleArtifactPickedUp()
        {
            shouldAttack = false;
            _currentState = EnemyState.Patrolling;
            PlayerPrefs.SetInt("ShouldAttack", shouldAttack ? 1 : 0);
        }

        protected override void Update()
        {
            if (!shouldAttack)
            {
                // When the artifact is picked up, wolves stop attacking
                if (_currentState == EnemyState.Attacking || _currentState == EnemyState.Chasing)
                {
                    _currentState = EnemyState.Patrolling;
                    return;
                }
            }
            base.Update();
        }

        protected override void Start()
        { 
            if (PlayerPrefs.HasKey("ShouldAttack"))
            {
                shouldAttack = PlayerPrefs.GetInt("ShouldAttack") == 1;
            }
            base.Start();
        }
    }
}

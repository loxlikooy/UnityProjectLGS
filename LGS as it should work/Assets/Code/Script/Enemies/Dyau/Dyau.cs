using System.Collections;
using UnityEngine;

namespace Code.Script.Dyau
{
    public class Dyau : Enemy
    {
        [SerializeField] private GameObject shockwavePrefab;
        [SerializeField] private float shockwaveDamage;
        [SerializeField] private float shockwaveRadius;
        [SerializeField] private float shockWaveCooldown = 8f;
        [SerializeField] private Vector2 runAwayPosition = new (500, 500);
        [SerializeField] private float runAwaySpeed = 5f; 

        private float _shockwaveHandelCooldown;
        private bool hasRunAway; 

        protected override void Update()
        {
            if (hasRunAway)
            {
                return;
            }
            base.Update();
            _shockwaveHandelCooldown += Time.deltaTime;
        }

        protected override void Attack()
        {
            base.Attack();
            if (_shockwaveHandelCooldown <= shockWaveCooldown) return;
                _animator.SetBool("ShockWaving", true);
                _shockwaveHandelCooldown = 0f;
        }

        private void CreateShockwave()
        {
            GameObject shockwave = Instantiate(shockwavePrefab, transform.position, Quaternion.identity);
            Shockwave shockwaveScript = shockwave.GetComponent<Shockwave>();
            if (shockwaveScript != null)
            {
                shockwaveScript.damage = shockwaveDamage;
                shockwaveScript.radius = shockwaveRadius;
            }
        }

        private void IsNotShockWaving()
        {
            _animator.SetBool("ShockWaving", false);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || hasRunAway)
                return;

            if (Input.GetKey(KeyCode.E) && PlayerPrefs.GetInt("HasLetterArtifact", 0) == 1)
            {
                StartCoroutine(RunAway());
            }
        }

        private IEnumerator RunAway()
        {
            hasRunAway = true;
            _animator.SetTrigger("Patrolling");
            PlayerPrefs.SetInt("HasLetterArtifact", 0);
            
            while ((Vector2)transform.position != runAwayPosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, runAwayPosition, runAwaySpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}

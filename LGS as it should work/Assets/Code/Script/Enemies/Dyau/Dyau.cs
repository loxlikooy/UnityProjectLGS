using UnityEngine;

namespace Code.Script.Dyau
{
    public class Dyau : Enemy
    {
        [SerializeField] private GameObject shockwavePrefab;
        [SerializeField] private float shockwaveDamage;
        [SerializeField] private float shockwaveRadius;
        [SerializeField] private float shockWaveCooldown = 8f;

        private float _shockwaveHandelCooldown;

        protected override void Update()
        {
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
    }
}
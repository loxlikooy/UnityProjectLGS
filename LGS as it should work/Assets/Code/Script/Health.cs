using UnityEngine;

namespace Code.Script
{
    public class Health : MonoBehaviour, IDamagable
    {
        public float maxHealth = 100f;
        [SerializeField]
        private float currentHealth;

        public delegate void HealthChangedDelegate(float currentHealth);
        public event HealthChangedDelegate OnHealthChanged;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            OnHealthChanged?.Invoke(currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            Destroy(gameObject);
        }
    }
}

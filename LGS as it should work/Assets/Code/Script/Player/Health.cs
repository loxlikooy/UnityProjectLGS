using UnityEngine;

namespace Code.Script
{
    public class Health : MonoBehaviour, IDamagable
    {
        public float maxHealth = 100f; 
        [SerializeField]
        private float currentHealth; //displayable health
        [SerializeField] 
        public float CurrentHealth => currentHealth; //для будущего пригодится, чтоб другие части скрипта могли смотерть на хп персонажа

        
        public delegate void HealthChangedDelegate(float currentHealth);
        public event HealthChangedDelegate OnHealthChanged;

        private void Start()
        {
            currentHealth = maxHealth;
            PlayerHUDManager.Instance.SetHealth(currentHealth, maxHealth);
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            PlayerHUDManager.Instance.SetHealth(currentHealth, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            Destroy(gameObject);
            GameManager.Instance.ShowRestartScreen();
            
           
        }
    }
}

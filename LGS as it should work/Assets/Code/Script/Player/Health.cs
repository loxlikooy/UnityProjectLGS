using System.Linq.Expressions;
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
            LoadHealth();
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
        
        public void IncreaseHealth()
        {
            maxHealth += 10;
            PlayerHUDManager.Instance.SetHealth(currentHealth, maxHealth);
        }

        public void HealthRegen()
        {
            currentHealth +=  10;
            PlayerHUDManager.Instance.SetHealth(currentHealth, maxHealth);
            //регенить % от макс хп + при переходе на новый лвл
        }

        protected virtual void Die()
        {
            Destroy(gameObject);
            Destroy(GameObject.FindGameObjectWithTag("MusicBox"));
            GameManager.Instance.ShowRestartScreen();
            PlayerPrefs.DeleteAll();
        }
        
        public void SaveHealth()
        {
            PlayerPrefs.SetFloat("MaxHealth", maxHealth);
            PlayerPrefs.SetFloat("CurrentHealth", currentHealth);
            PlayerPrefs.Save();
        }

        public void LoadHealth()
        {
            if (PlayerPrefs.HasKey("MaxHealth"))
            {
                maxHealth = PlayerPrefs.GetFloat("MaxHealth");
            }
            if (PlayerPrefs.HasKey("CurrentHealth"))
            {
                currentHealth = PlayerPrefs.GetFloat("CurrentHealth");
            }
        }

    }
}

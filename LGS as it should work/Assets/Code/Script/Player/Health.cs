using System;
using UnityEngine;

namespace Code.Script
{
    public class Health : MonoBehaviour, IDamagable
    {
        [SerializeField] private PlayerStatsSO playerStatsSO;
        private ComponentGetter _componentGetter;

        private float _maxHealth;
        private float _currentHealth;  

        public delegate void HealthChangedDelegate(float currentHealth);
        public event HealthChangedDelegate OnHealthChanged;
        
        private void Start()
        {
            // Загружаем постоянные и временные параметры
            LoadPermanentStats();
            LoadTemporaryStats();
            PlayerHUDManager.Instance.SetHealth(_currentHealth, _maxHealth);
            
            _componentGetter = GetComponent<ComponentGetter>();

        }

        public void TakeDamage(float damage)
        {
            // Применяем урон и обновляем HUD
            _currentHealth -= damage;
            PlayerHUDManager.Instance.SetHealth(_currentHealth, _maxHealth);
            OnHealthChanged?.Invoke(_currentHealth);

            _componentGetter.PlayerAnimator.PlayDamageTakingAnimation();

            // Проверяем, умер ли игрок
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        public void IncreaseHealth(float amount)
        {
            // Увеличиваем максимальное здоровье
            _maxHealth += (int)(_maxHealth * (amount / 100));
            PlayerHUDManager.Instance.SetHealth(_currentHealth, _maxHealth);
            SaveTemporaryStats(); // Сохраняем временные изменения
        }

        public void HealthRegen(float amount)
        {
            // Восстанавливаем здоровье, если это возможно
            if (_currentHealth >= _maxHealth)
            {
                return;
            }
            _currentHealth += amount;
            PlayerHUDManager.Instance.SetHealth(_currentHealth, _maxHealth);
            SaveTemporaryStats(); // Сохраняем временные изменения
        }

        protected virtual void Die()
        {
            // Уничтожаем игровые объекты и очищаем временные данные
            Destroy(GameObject.FindGameObjectWithTag("MusicBox"));
            PlayerPrefs.DeleteAll();
            GameManager.Instance.ShowRestartScreen();
            Destroy(gameObject);
        }

        public void SaveTemporaryStats()
        {
            // Сохраняем временные параметры здоровья
            PlayerPrefs.SetFloat("TemporaryMaxHealth", _maxHealth);
            PlayerPrefs.SetFloat("TemporaryCurrentHealth", _currentHealth);
            PlayerPrefs.Save();
        }

        private void LoadPermanentStats()
        {
            // Загружаем постоянные параметры здоровья из ScriptableObject
            _maxHealth = playerStatsSO.maxHealth;
            _currentHealth = playerStatsSO.currentHealth;
        }

        private void LoadTemporaryStats()
        {
            // Загружаем временные параметры здоровья из PlayerPrefs
            if (PlayerPrefs.HasKey("TemporaryMaxHealth"))
            {
                _maxHealth = PlayerPrefs.GetFloat("TemporaryMaxHealth");
            }
            if (PlayerPrefs.HasKey("TemporaryCurrentHealth"))
            {
                _currentHealth = PlayerPrefs.GetFloat("TemporaryCurrentHealth");
            }
        }
    }
}

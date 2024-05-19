using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Script

{
    public class PlayerHUDManager : MonoBehaviour
    {
        public static PlayerHUDManager Instance { get; private set; }
        [Header("Health")]
        public Image healthBar;
        public Text healthText;
        [Header("EXP")]
        public Image expBar;
        public Text expText;
        [Header("Dash")]
        public Image dashOnColdown;
        [Header(("Attack"))]
        public Image attackOnCooldown;

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

        public void SetHealth(float health,float maxHealth)
        {
            
            healthBar.fillAmount = health / maxHealth;
            healthText.text = health.ToString("0") + "/" + maxHealth.ToString("0");
        }

        public void SetExp(float exp, float maxExp)
        {
            expBar.fillAmount = exp / maxExp;
            expText.text = exp.ToString("0") + "/" + maxExp.ToString("0");
        }

        public void DashColdown(float dashCooldown, float timeSinceLastDash)
        {
            float cooldownLeft = timeSinceLastDash / dashCooldown;
            dashOnColdown.fillAmount = Mathf.Clamp(cooldownLeft, 0, 1);
        }
        
        public void AttackCooldown(float attackCooldown, float timeSinceLastAttack)
        {
            float cooldownLeft = timeSinceLastAttack / attackCooldown;
            attackOnCooldown.fillAmount = Mathf.Clamp(cooldownLeft, 0, 1);
        }

        
    }
}
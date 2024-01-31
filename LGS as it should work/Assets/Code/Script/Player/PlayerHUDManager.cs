using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Script

{
    public class PlayerHUDManager : MonoBehaviour
    {
        public static PlayerHUDManager Instance { get; private set; }

        public Image healthBar;
        //public Image expBar;
        public Text healthText;
        public Image dashOnColdown;
        

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
            //expBar.fillAmount = exp / maxExp;
            return;
        }

        public void DashColdown(float dashCooldown, float timeSinceLastDash)
        {
            float cooldownLeft = timeSinceLastDash / dashCooldown;
            dashOnColdown.fillAmount = Mathf.Clamp(cooldownLeft, 0, 1);
        }
    }
}
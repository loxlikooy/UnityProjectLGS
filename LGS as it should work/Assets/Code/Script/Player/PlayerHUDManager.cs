using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Script

{
    public class PlayerHUDManager : MonoBehaviour
    {
        public static PlayerHUDManager Instance { get; private set; }

        public Image HealthBar;

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
            HealthBar.fillAmount = health / maxHealth ;
        }
    }
}
﻿using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Script
{
    public class EXP : MonoBehaviour
    {
        private float _expForNextLevel = 10f;
        private float _currentExp = 0f;

        private void Start()
        {
            PlayerHUDManager.Instance.SetExp(_currentExp, _expForNextLevel);
        }

        public void AddExp(float amountExp)
        {
            _currentExp += amountExp;
    
            // Используем цикл while для повторной проверки после каждого повышения уровня
            while (_currentExp >= _expForNextLevel)
            {
                _currentExp -= _expForNextLevel; 
                _expForNextLevel += 20; 
                GameManager.Instance.ShowRandomUpgrades(); 
            }
    
            PlayerHUDManager.Instance.SetExp(_currentExp, _expForNextLevel); // Обновляем HUD игрока
        }
        
    }
}
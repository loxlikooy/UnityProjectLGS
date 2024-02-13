using System;
using System.Collections;
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
            Quest killEnemyQuest = new Quest("Убить врага");
            QuestManager.Instance.AddQuest(killEnemyQuest);
            QuestManager.Instance.EnableQuestText(true);
        }

        public void AddExp(float amountExp) {
            if (GameManager.Instance.IsUpgradeScreenShown()) {
                Debug.Log("жду");
                StartCoroutine(WaitAndAddExp(amountExp));
            } else {
                _currentExp += amountExp;
                CheckLevelUp();
            }
        }

        private void CheckLevelUp() 
        {
            if (_currentExp >= _expForNextLevel) {
                _currentExp -= _expForNextLevel;
                _expForNextLevel += 20;
                if (!GameManager.Instance.IsUpgradeScreenShown()) {
                    GameManager.Instance.ShowRandomUpgrades();
                }
            }
            PlayerHUDManager.Instance.SetExp(_currentExp, _expForNextLevel);
        }

        IEnumerator WaitAndAddExp(float amountExp) 
        {
            // Ожидание пока экран выбора улучшений будет скрыт
            yield return new WaitUntil(() => !GameManager.Instance.IsUpgradeScreenShown());
            AddExp(amountExp); // Повторное добавление опыта после закрытия экрана
        }
        
    }
}
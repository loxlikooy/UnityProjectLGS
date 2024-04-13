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
            Quest killEnemyQuest = new Quest("Убить врага");
            QuestManager.Instance.AddQuest(killEnemyQuest);
            QuestManager.Instance.EnableQuestText(true);
            LoadEXP();
            PlayerHUDManager.Instance.SetExp(_currentExp, _expForNextLevel);
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
        
        public void SaveEXP()
        {
            PlayerPrefs.SetFloat("CurrentExp", _currentExp);
            PlayerPrefs.SetFloat("ExpForNextLevel", _expForNextLevel);
            PlayerPrefs.Save();
        }

        public void LoadEXP()
        {
            if (PlayerPrefs.HasKey("CurrentExp"))
            {
                _currentExp = PlayerPrefs.GetFloat("CurrentExp");
            }
            if (PlayerPrefs.HasKey("ExpForNextLevel"))
            {
                _expForNextLevel = PlayerPrefs.GetFloat("ExpForNextLevel");
            }
        }

        
    }
}
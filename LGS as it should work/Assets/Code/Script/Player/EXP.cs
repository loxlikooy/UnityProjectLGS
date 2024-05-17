using System.Collections;
using UnityEngine;

namespace Code.Script
{
    public class EXP : MonoBehaviour
    {
        private float _expForNextLevel = 10f;
        private float _currentExp;


        private void Start()
        {
            LoadEXP();
            PlayerHUDManager.Instance.SetExp(_currentExp, _expForNextLevel);
        }

        public void AddExp(float amountExp) 
        {
            if (GameManager.Instance.IsUpgradeScreenShown()) 
            {
                StartCoroutine(WaitAndAddExp(amountExp));
            }
            else 
            {
                while (amountExp>0f)
                {
                    _currentExp++;
                    amountExp--;
                    float expHolder = amountExp;
                    if (GameManager.Instance.IsUpgradeScreenShown()) 
                    {
                        StartCoroutine(WaitAndAddExp(expHolder));
                        return;
                    }
                    CheckLevelUp();
                    SaveEXP();
                }
                
            }
        }

        private void CheckLevelUp() 
        {
            if (_currentExp >= _expForNextLevel) 
            {
                _currentExp -= _expForNextLevel;
                _expForNextLevel += 20;
                if (!GameManager.Instance.IsUpgradeScreenShown()) 
                {
                    GameManager.Instance.ShowRandomUpgrades();
                }
            }
            PlayerHUDManager.Instance.SetExp(_currentExp, _expForNextLevel);
        } //пофиксить получения большого количества экспы

        private IEnumerator WaitAndAddExp(float amountExp) 
        {
            // Ожидание пока экран выбора улучшений будет скрыт
            yield return new WaitUntil(() => !GameManager.Instance.IsUpgradeScreenShown());
            AddExp(amountExp); // Повторное добавление опыта после закрытия экрана
        }
        
        private void SaveEXP()
        {
            PlayerPrefs.SetFloat("CurrentExp", _currentExp);
            PlayerPrefs.SetFloat("ExpForNextLevel", _expForNextLevel);
            PlayerPrefs.Save();
        }

        private void LoadEXP()
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
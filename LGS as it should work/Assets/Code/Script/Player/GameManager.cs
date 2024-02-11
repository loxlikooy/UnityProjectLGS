using System.Collections.Generic;
using Code.Script;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public GameObject restartScreen;
    public GameObject HUD;
    
    public Upgrade[] allUpgrades; // Массив со всеми доступными улучшениями
    public Button[] choiceButtons; // Массив кнопок для выбора улучшений
    public GameObject abilityChoiceScreen; // Экран выбора улучшений

    public Health playerHealth;
    public PlayerAttack playerAttack;
    public Dash playerDash;
    public MoveVelocity playerMoveVelocity;
    
    public void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }

        
    }

    private void Start()
    {
        InitializeUpgrades();
        
    }

    private void InitializeUpgrades()
    {
        allUpgrades = new Upgrade[]
        {
            new Upgrade { name = "Health Increase", effect = playerHealth.IncreaseHealth },
            new Upgrade { name = "Health Regen", effect = playerHealth.HealthRegen },
            new Upgrade { name = "Damage Increase", effect = playerAttack.IncreeseDamage },
            new Upgrade { name = "Decrease Dash Cooldown", effect = playerDash.DecreaseDashCooldown },
            new Upgrade { name = "Speed Increase", effect = playerMoveVelocity.IncreaseSpeed }
        };
    }




    public void ShowRestartScreen() {
        restartScreen.SetActive(true);
        HUD.SetActive(false);
        // Optionally, pause the game here if needed
    }

    public void ShowRandomUpgrades()
    {
        List<int> chosenIndices = new List<int>();
        while (chosenIndices.Count < 3)
        {
            int randomIndex = Random.Range(0, allUpgrades.Length);
            if (!chosenIndices.Contains(randomIndex))
            {
                chosenIndices.Add(randomIndex);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            int index = chosenIndices[i];
            choiceButtons[i].GetComponent<Image>().sprite = allUpgrades[index].icon;

            // Найти компонент TextMeshProUGUI внутри кнопки и установить его текст
            var buttonText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = allUpgrades[index].name; // Установить имя улучшения
        
            choiceButtons[i].onClick.RemoveAllListeners();
            choiceButtons[i].onClick.AddListener(() => ApplyUpgrade(allUpgrades[index]));
        }

        abilityChoiceScreen.SetActive(true);
        HUD.SetActive(false);
        Time.timeScale = 0;
    }



    void ApplyUpgrade(Upgrade upgrade)
    {
        upgrade.effect?.Invoke();
        HideAbilityChoiceScreen();
    }

    void HideAbilityChoiceScreen()
    {
        abilityChoiceScreen.SetActive(false);
        HUD.SetActive(true);
        Time.timeScale = 1;
    }

    public void RestartGame() {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }
}

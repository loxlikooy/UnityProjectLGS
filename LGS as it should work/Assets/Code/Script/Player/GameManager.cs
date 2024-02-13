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
    
    private Upgrade[] allUpgrades; // Массив со всеми доступными улучшениями
    public Button[] choiceButtons; // Массив кнопок для выбора улучшений
    public GameObject abilityChoiceScreen; // Экран выбора улучшений

    [SerializeField] private Health playerHealth;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private Dash playerDash;
    [SerializeField] private MoveVelocity playerMoveVelocity;
    
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
            new Upgrade
            {
                name = "Health Increase",
                effect = playerHealth.IncreaseHealth,
                icon = Resources.Load<Sprite>("OurIcons/survivalrateup")
            },
            new Upgrade
            {
                name = "Health Regen",
                effect = playerHealth.HealthRegen,
                icon = Resources.Load<Sprite>("OurIcons/survivalrateup")
            },
            new Upgrade
            {
                name = "Damage Increase",
                effect = playerAttack.IncreaseDamage, // Исправьте опечатку в названии метода здесь
                icon = Resources.Load<Sprite>("OurIcons/damageup")
            },
            new Upgrade
            {
                name = "Decrease Dash Cooldown",
                effect = playerDash.DecreaseDashCooldown,
                icon = Resources.Load<Sprite>("OurIcons/agilityup")
            },
            new Upgrade
            {
                name = "Speed Increase",
                effect = playerMoveVelocity.IncreaseSpeed,
                icon = Resources.Load<Sprite>("OurIcons/agilityup")
            },
            new Upgrade
            {
                name = "Upgrade Attack Speed",
                effect = playerAttack.DecreaseAttackCooldown,
                icon = Resources.Load<Sprite>("OurIcons/damageup")
            }
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
            Button button = choiceButtons[i];
            button.GetComponent<Image>().sprite = allUpgrades[index].icon;
            var buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = allUpgrades[index].name;

            button.onClick.RemoveAllListeners();
            int finalIndex = index; // Локальная переменная для использования в лямбда-выражении
            button.onClick.AddListener(() => ApplyUpgrade(allUpgrades[finalIndex]));
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

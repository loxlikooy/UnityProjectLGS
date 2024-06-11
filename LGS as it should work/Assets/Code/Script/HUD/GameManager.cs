using System.Collections.Generic;
using Code.Script;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject restartScreen;
    public GameObject HUD;
    private bool _isUpgradeScreenShown = false;

    public GameObject NPCUpgradeScreen;

    [SerializeField] private UpgradeData[] allUpgrades; // Array with all available upgrades
    [SerializeField] private Button[] choiceButtons; // Array of buttons for choosing upgrades
    public GameObject abilityChoiceScreen; // Screen for choosing upgrades

    private Health playerHealth;
    private PlayerAttack playerAttack;
    private Dash playerDash;
    private MoveVelocity playerMoveVelocity;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerHealth = FindObjectOfType<Health>();
        playerAttack = FindObjectOfType<PlayerAttack>();
        playerDash = FindObjectOfType<Dash>();
        playerMoveVelocity = FindObjectOfType<MoveVelocity>();

        if (playerHealth == null || playerAttack == null || playerDash == null || playerMoveVelocity == null)
        {
            Debug.LogError("One or more player components are not found on the scene.");
        }
    }

    public void ShowRestartScreen()
    {
        restartScreen.SetActive(true);
        HUD.SetActive(false);
    }

    public void ShowRandomUpgrades()
    {
        if (_isUpgradeScreenShown) return;
        _isUpgradeScreenShown = true;
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
            buttonText.text = allUpgrades[index].upgradeName;

            button.onClick.RemoveAllListeners();
            int finalIndex = index; // Local variable for use in the lambda expression
            button.onClick.AddListener(() => ApplyUpgrade(allUpgrades[finalIndex]));
        }

        abilityChoiceScreen.SetActive(true);
        HUD.SetActive(false);
        Time.timeScale = 0;
    }

    private void ApplyUpgrade(UpgradeData upgrade)
    {
        switch (upgrade.effect)
        {
            case UpgradeEffect.HealthIncrease:
                playerHealth.IncreaseHealth(10);
                break;
            case UpgradeEffect.HealthRegen:
                playerHealth.HealthRegen(10);
                break;
            case UpgradeEffect.DamageIncrease:
                playerAttack.IncreaseDamage(30);
                break;
            case UpgradeEffect.DecreaseDashCooldown:
                playerDash.DecreaseDashCooldown(0.5f);
                break;
            case UpgradeEffect.SpeedIncrease:
                playerMoveVelocity.IncreaseSpeed(10);
                break;
            case UpgradeEffect.UpgradeAttackSpeed:
                playerAttack.DecreaseAttackCooldown(10);
                break;
        }
        HideAbilityChoiceScreen();
    }

    private void HideAbilityChoiceScreen()
    {
        abilityChoiceScreen.SetActive(false);
        HUD.SetActive(true);
        _isUpgradeScreenShown = false;
        Time.timeScale = 1;
    }

    public bool IsUpgradeScreenShown()
    {
        return _isUpgradeScreenShown;
    }

    public void RestartGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == 6)
        {
            SceneManager.LoadScene(8);
        }
        else if (currentSceneIndex > 2)
        {
            SceneManager.LoadScene(7);
        }
       
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowNPCUpgradeMenu()
    {
        if (_isUpgradeScreenShown) return;
        _isUpgradeScreenShown = true;
        NPCUpgradeScreen.SetActive(true);
        HUD.SetActive(false);
        Time.timeScale = 0;
    }

    public void HideNPCUpgradeMenu()
    {
        NPCUpgradeScreen.SetActive(false);
        HUD.SetActive(true);
        _isUpgradeScreenShown = false;
        Time.timeScale = 1;
    }
}

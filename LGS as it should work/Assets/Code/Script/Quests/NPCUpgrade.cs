using Code.Script;
using UnityEngine;
using UnityEngine.UI;

public class NPCUpgrade : MonoBehaviour
{
    [SerializeField] private Text expText;
    [SerializeField] private Text healthUpgradeCountText;
    [SerializeField] private Text damageUpgradeCountText;
    [SerializeField] private Text playerDamageText;
    [SerializeField] private Text playerMaxHealthText;
    [SerializeField] private PlayerStatsSO playerStatsSO;
    [SerializeField] private PermamentUpgradeSO permamentUpgradeSO;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
            GameManager.Instance.ShowNPCUpgradeMenu();
            UpdateUI();
        }
    }

    public void UpgradeHealth()
    {
        if (permamentUpgradeSO.exp >= 1 && permamentUpgradeSO.timesUpgradedHealth < 3)
        {
            playerStatsSO.maxHealth += 10;
            permamentUpgradeSO.exp -= 1;
            permamentUpgradeSO.timesUpgradedHealth += 1;
            PlayerHUDManager.Instance.SetHealth(playerStatsSO.currentHealth, playerStatsSO.maxHealth);
        }
    }

    public void UpgradeDamage()
    {
        if (permamentUpgradeSO.exp >= 1 && permamentUpgradeSO.timesUpgradedStrength < 3)
        {
            playerStatsSO.playerDamage += 2;
            permamentUpgradeSO.exp -= 1;
            permamentUpgradeSO.timesUpgradedStrength += 1;
            UpdateUI();
        }
    }
    
    private void UpdateUI()
    {
        expText.text = "EXP: " + permamentUpgradeSO.exp;
        healthUpgradeCountText.text = "Health Upgrades: " + permamentUpgradeSO.timesUpgradedHealth;
        damageUpgradeCountText.text = "Damage Upgrades: " + permamentUpgradeSO.timesUpgradedStrength;
        playerDamageText.text = "Damage: " + playerStatsSO.playerDamage;
        playerMaxHealthText.text = "Max Health: " + playerStatsSO.maxHealth;
    }
    
}
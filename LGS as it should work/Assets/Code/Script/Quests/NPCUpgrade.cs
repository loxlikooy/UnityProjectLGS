using Code.Script;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCUpgrade : MonoBehaviour
{
    [SerializeField] private Text expText;
    [SerializeField] private Text healthUpgradeCountText;
    [SerializeField] private Text damageUpgradeCountText;
    [SerializeField] private Text playerDamageText;
    [SerializeField] private Text playerMaxHealthText;
    [SerializeField] private PlayerStatsSO playerStatsSO;
    [SerializeField] private PermamentUpgradeSO permamentUpgradeSO;
    [SerializeField] private DialogSO dialogSO;
    [SerializeField] private Text dialogText;
    [SerializeField] private Image characterImage; // Image component to display character sprite
    [SerializeField] private GameObject dialogUI;
    [SerializeField] private float typingSpeed = 0.05f; // Speed of the typing animation

    private int currentLineIndex = 0;
    private bool isDialogActive = false;
    private Coroutine typingCoroutine;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
            if (!isDialogActive && !dialogUI.activeInHierarchy)
            {
                ShowDialog();
            }
        }
    }

    private void ShowDialog()
    {
        if (dialogSO.TimesDialogueOpened == 0)
        {
            currentLineIndex = 0;
            characterImage.sprite = dialogSO.dialogEntries[currentLineIndex].characterSprite;
            typingCoroutine = StartCoroutine(TypeText(dialogSO.dialogEntries[currentLineIndex].dialogLine));
        }
        else
        {
            characterImage.sprite = dialogSO.repeatDialogSprite; // Use a sprite for repeat dialog
            typingCoroutine = StartCoroutine(TypeText(dialogSO.repeatDialog));
        }
        dialogUI.SetActive(true);
        isDialogActive = true;
        Time.timeScale = 0;
    }

    public void ContinueDialog()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            if (dialogSO.TimesDialogueOpened == 0)
            {
                dialogText.text = dialogSO.dialogEntries[currentLineIndex].dialogLine;
            }
            else
            {
                dialogText.text = dialogSO.repeatDialog;
            }
            typingCoroutine = null;
        }
        else
        {
            if (dialogSO.TimesDialogueOpened == 0)
            {
                currentLineIndex++;
                if (currentLineIndex < dialogSO.dialogEntries.Length)
                {
                    characterImage.sprite = dialogSO.dialogEntries[currentLineIndex].characterSprite;
                    typingCoroutine = StartCoroutine(TypeText(dialogSO.dialogEntries[currentLineIndex].dialogLine));
                }
                else
                {
                    dialogSO.TimesDialogueOpened++; // Increment the flag after the first dialog is completed
                    EndDialog();
                }
            }
            else
            {
                EndDialog();
            }
        }
    }

    private IEnumerator TypeText(string line)
    {
        dialogText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed); // Use WaitForSecondsRealtime
        }
        typingCoroutine = null;
    }

    private void EndDialog()
    {
        dialogUI.SetActive(false);
        isDialogActive = false;
        ShowUpgradeMenu();
    }

    private void ShowUpgradeMenu()
    {
        GameManager.Instance.ShowNPCUpgradeMenu();
        UpdateUI();
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

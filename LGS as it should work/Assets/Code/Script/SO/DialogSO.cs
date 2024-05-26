using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "Dialog/DialogData")]
public class DialogSO : ScriptableObject
{
    [System.Serializable]
    public struct DialogEntry
    {
        [TextArea(3, 10)]
        public string dialogLine;
        public Sprite characterSprite;
    }

    public DialogEntry[] dialogEntries; // Array to hold dialog entries
    public int TimesDialogueOpened;
    public string repeatDialog = "Do you want my blessing?"; // Message for repeat dialog
    public Sprite repeatDialogSprite; // Sprite for repeat dialog
}
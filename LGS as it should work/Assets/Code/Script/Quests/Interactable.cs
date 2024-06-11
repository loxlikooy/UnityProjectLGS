using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    public string interactionKey = "E";
    public string interactionTextObjectName = "Interact";

    private TextMeshProUGUI interactionText;

    private void Start()
    {
        GameObject interactionTextObject = GameObject.Find(interactionTextObjectName);

        if (interactionTextObject != null)
        {
            interactionText = interactionTextObject.GetComponent<TextMeshProUGUI>();

            if (interactionText != null)
            {
                interactionText.text = "";
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
                interactionText.text = $"To interact press {interactionKey}";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
                interactionText.text = "";
        }
    }
}
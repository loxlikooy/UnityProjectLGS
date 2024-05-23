using Code.Script.SO;
using UnityEngine;
using TMPro; // Import namespace for TextMeshPro
using UnityEngine.UI; // Import namespace for UI

public class BookInteraction : MonoBehaviour
{
    public GameObject bookUI; // Book UI
    public GameObject hudUI; // HUD UI
    public TMP_Text bookText; // Book text using TextMeshPro
    public TMP_Text titleText; // Page title using TextMeshPro
    public Image bookImage; // Image component to display sprites
    private int _pageIndex; // Page index

    [SerializeField] private BookData _bookData;

    private void Start()
    {
        bookUI.SetActive(false); // Book is hidden by default
    }

    public void OpenBook()
    {
        bookUI.SetActive(true);
        hudUI.SetActive(false);
        _pageIndex = 0;
        UpdatePage();
        Time.timeScale = 0; // Pause the game
    }

    public void CloseBook()
    {
        bookUI.SetActive(false);
        hudUI.SetActive(true);
        Time.timeScale = 1; // Resume the game
    }

    public void NextPage()
    {
        if (_pageIndex >= _bookData.Pages.Length - 1)
            return;
        
        _pageIndex++;
        UpdatePage();
    }
    
    public void PrevPage()
    {
        if (_pageIndex < 1) return;
        
        _pageIndex--;
        UpdatePage();
    }

    private void UpdatePage()
    {
        bookText.text = _bookData.Pages[_pageIndex];
        titleText.text = _bookData.Titles[_pageIndex]; // Update title according to the current page
        bookImage.sprite = _bookData.Sprites[_pageIndex]; // Update image according to the current page
    }
}
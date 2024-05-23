using Code.Script.SO;
using UnityEngine;
using TMPro; // Импорт пространства имен для TextMeshPro

public class BookInteraction : MonoBehaviour
{
    public GameObject bookUI; // UI книги
    public GameObject hudUI; // UI худа
    public TMP_Text bookText; // Текст книги с использованием TextMeshPro
    public TMP_Text titleText; // Заголовок страницы с использованием TextMeshPro
    private int _pageIndex; // Индекс страницы книги

    // Массивы для текста и заголовков страниц

    [SerializeField] private BookData _bookData;
   

    private void Start()
    {
        bookUI.SetActive(false); // По умолчанию книга скрыта
    }

    public void OpenBook()
    {
        bookUI.SetActive(true);
        hudUI.SetActive(false);
        _pageIndex = 0;
        UpdatePage();
        Time.timeScale = 0; // Приостановить игру
    }

    public void CloseBook()
    {
        bookUI.SetActive(false);
        hudUI.SetActive(true);
        Time.timeScale = 1; // Возобновить игру
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
        titleText.text = _bookData.Titles[_pageIndex]; // Обновляет заголовок в соответствии с текущей страницей
    }
}
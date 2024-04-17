using UnityEngine;
using TMPro; // Импорт пространства имен для TextMeshPro

public class BookInteraction : MonoBehaviour
{
    public GameObject bookUI; // UI книги
    public GameObject hudUI; // UI худа
    public TMP_Text bookText; // Текст книги с использованием TextMeshPro
    public TMP_Text titleText; // Заголовок страницы с использованием TextMeshPro
    private int pageIndex = 0; // Индекс страницы книги

    // Массивы для текста и заголовков страниц
    private string[] pages;
    private string[] titles;

    private void Start()
    {
        // Инициализация текстов и заголовков страниц
        pages = new string[] 
        {
            "болото с камышом, прудами и реками",
            "Приятные, но весьма опасные существа женского пола. Согласно казахскому фольклору, в степных реках, в протоках среди камышей, часто покрытые илом, в омутах могут обитать они - водяные красавицы-русалки. Сродни сиренам, они соблазняют юношей своими песнями и щекочут до смерти; в полночь могут приходить в юрты. Кульдыргыш убивает человека сильным щекотанием, откуда и происходит само название, обозначающее «заставляющая смеяться».",
            // Добавьте столько страниц, сколько нужно
        };

        titles = new string[] 
        {
            "Секция 1: Локация",
            "Секция 2: Описание",
            // Убедитесь, что количество заголовков соответствует количеству страниц
        };

        bookUI.SetActive(false); // По умолчанию книга скрыта
    }

    public void OpenBook()
    {
        bookUI.SetActive(true);
        hudUI.SetActive(false);
        pageIndex = 0;
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
        if (pageIndex >= pages.Length - 1)
            return;
        
        pageIndex++;
        UpdatePage();
        
    }
    
    public void PrevPage()
    {
        if (pageIndex < 1) return;
            
            
        pageIndex--;
        UpdatePage();
        
    }

    private void UpdatePage()
    {
        bookText.text = pages[pageIndex];
        titleText.text = titles[pageIndex]; // Обновляет заголовок в соответствии с текущей страницей
    }
}
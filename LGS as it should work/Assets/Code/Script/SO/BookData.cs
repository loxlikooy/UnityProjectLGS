using UnityEngine;

namespace Code.Script.SO
{
    [CreateAssetMenu(fileName = "NewBookData", menuName = "ScriptableObjects/BookData", order = 1)]
    public class BookData : ScriptableObject
    {
        [SerializeField]
        private string[] pages;

        [SerializeField]
        private string[] titles;

        // Свойства для доступа к полям (необязательно, но рекомендуется)
        public string[] Pages
        {
            get => pages;
            set => pages = value;
        }

        public string[] Titles
        {
            get => titles;
            set => titles = value;
        }
    }
}
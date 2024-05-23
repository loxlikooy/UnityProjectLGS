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

        [SerializeField]
        private Sprite[] sprites; // Add this line to include sprites

        // Properties for accessing the fields (optional but recommended)
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

        public Sprite[] Sprites // Add this property for sprites
        {
            get => sprites;
            set => sprites = value;
        }
    }
}
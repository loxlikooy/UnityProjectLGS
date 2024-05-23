using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Script.ProceduralGenerator
{
    public class DoorInteraction : MonoBehaviour
    {
        [SerializeField] private KeyCode interactionKey = KeyCode.F; // Key for interaction
        private ComponentGetter componentGetter;

        private void Start()
        {
            componentGetter = FindObjectOfType<ComponentGetter>();
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            if (!collider.CompareTag("Door")) return;

            if (Input.GetKey(interactionKey))
            {
                SaveManager.SavePlayerStats(componentGetter);
                SaveManager.SaveQuestStates();
                LoadNextLevel();
            }
        }

        private void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            // Check if the next scene index is within the valid range
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.LogWarning("No more levels to load!");
            }
        }
    }
}
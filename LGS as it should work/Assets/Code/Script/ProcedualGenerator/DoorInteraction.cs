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

        private static void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            // Check if the next scene index is within the valid range and not greater than 6
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings && nextSceneIndex <= 6)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }

    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Script.ProcedualGenerator
{
    public class DoorInteraction : MonoBehaviour
    {
        [SerializeField] private KeyCode interactionKey = KeyCode.F; // Key for interaction
        
        private void OnTriggerStay2D(Collider2D collider)
        {
            if (!collider.CompareTag("Player")) return;

            if (Input.GetKey(interactionKey))
            {
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
                // Optionally, restart the game or show a game complete message
                // SceneManager.LoadScene(0); // Restart the game
            }
        }
    }
}
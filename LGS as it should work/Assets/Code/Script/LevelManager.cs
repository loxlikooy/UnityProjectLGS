using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("allodolbaeb");
        if (other.CompareTag("Player")) // Убедитесь, что у вашего игрока установлен тег "Player"
        {
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = (currentSceneIndex + 1) % totalScenes; // Переход к следующей сцене по кругу
        SceneManager.LoadScene(1); // Загрузка следующей сцены
    }
}
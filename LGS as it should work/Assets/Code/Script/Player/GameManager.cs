using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public GameObject restartScreen;
    public GameObject HUD;

    public void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
    }




    public void ShowRestartScreen() {
        restartScreen.SetActive(true);
        HUD.SetActive(false);
        // Optionally, pause the game here if needed
    }

    public void RestartGame() {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
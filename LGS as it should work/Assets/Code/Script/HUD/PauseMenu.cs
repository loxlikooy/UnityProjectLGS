using UnityEngine;
using UnityEngine.SceneManagement;
using Code.Script.Music;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    private MusicController musicController;

    void Start()
    {
        // Find and reference the MusicController in the scene
        musicController = MusicController.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void Resume()
    {
        TogglePauseMenu();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Ensure game time is running when changing scenes
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the name of your main menu scene
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    void TogglePauseMenu()
    {
        if (pauseMenuPanel != null)
        {
            // Toggle the active state of the pause menu panel
            bool isPaused = !pauseMenuPanel.activeSelf;
            pauseMenuPanel.SetActive(isPaused);

            // Pause or resume game time based on the panel's active state
            Time.timeScale = isPaused ? 0f : 1f;

            // Pause or resume music
            if (musicController != null)
            {
                if (isPaused)
                {
                    musicController.PauseMusic(); // Pause the music
                }
                else
                {
                    musicController.ResumeMusic(); // Resume the music
                }
            }
        }
        else
        {
            Debug.LogWarning("PauseMenuPanel is not assigned. Please assign it in the inspector.");
        }
    }
}
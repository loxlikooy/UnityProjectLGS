using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;

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
            pauseMenuPanel.SetActive(!pauseMenuPanel.activeSelf);

            // Pause or resume game time based on the panel's active state
            Time.timeScale = pauseMenuPanel.activeSelf ? 0f : 1f;
        }
        else
        {
            Debug.LogWarning("PauseMenuPanel is not assigned. Please assign it in the inspector.");
        }
    }
}
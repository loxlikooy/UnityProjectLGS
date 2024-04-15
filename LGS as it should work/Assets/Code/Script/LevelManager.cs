using System;
using Code.Script;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private PlayerAttack _playerAttack;
    private Dash _dash;
    private EXP _exp;
    private Health _health;

    private void Start()
    {
        _playerAttack = GetComponent<PlayerAttack>();
        _dash = GetComponent<Dash>();
        _exp = GetComponent<EXP>();
        _health = GetComponent<Health>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish")) // Убедитесь, что у вашего игрока установлен тег "Player"
        {
            SavePlayerPrefs();
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = (currentSceneIndex + 1) % totalScenes; // Переход к следующей сцене по кругу
        SceneManager.LoadScene(nextSceneIndex); // Загрузка следующей сцены
    }

    void SavePlayerPrefs()
    {
       _playerAttack.SaveAttackStats();
       _dash.SaveDashCooldown();
       _exp.SaveEXP();
       _health.SaveHealth();
    }
}
using System.Collections;
using Code.Script;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform startPoint; 
    [SerializeField] private Transform endPoint; 
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

        MoveToStartPoint();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Finish"))
            return;
        
        SavePlayerPrefs();
        StartCoroutine(MoveToEndPoint(1.5f, () => LoadNextLevel()));
    }

    private static void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = (currentSceneIndex + 1) % totalScenes; // Переход к следующей сцене по кругу
        SceneManager.LoadScene(nextSceneIndex); // Загрузка следующей сцены
    }

    private void SavePlayerPrefs()
    {
       _playerAttack.SaveAttackStats();
       _dash.SaveDashCooldown();
       _exp.SaveEXP();
       _health.SaveHealth();
    }

    private IEnumerator MoveToStartPointCoroutine(Vector2 targetPosition, float duration)
    {
        Vector2 startPosition = transform.position;
        float time = 0f;

        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    private void MoveToStartPoint()
    {
        StartCoroutine(MoveToStartPointCoroutine(startPoint.position, 1.5f)); // 1.5 секунды для плавного перемещения
    }

    private IEnumerator MoveToEndPoint(float duration, System.Action onEnd)
    {
        Vector2 targetPosition = endPoint.position;
        Vector2 startPosition = transform.position;

        float time = 0f;
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;

        onEnd?.Invoke(); // Вызываем событие по завершению перемещения
    }
}

using System.Collections;
using Code.Script;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform startPoint; 
    [SerializeField] private Transform endPoint;
    private ComponentGetter _componentGetter;
    private void Start()
    {
        _componentGetter = GetComponent<ComponentGetter>();
        MoveToStartPoint();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish"))
        {
            SavePlayerPrefs();
            StartCoroutine(MoveToEndPoint(1.5f, () => LoadNextLevel()));
        }

        if (other.CompareTag("Quest"))
        {
            QuestManager.Instance.CompleteQuest("Explore the Forest");
        }
    }


    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = (currentSceneIndex + 1) % totalScenes; // Переход к следующей сцене по кругу
        SceneManager.LoadScene(nextSceneIndex); // Загрузка следующей сцены
    }

    private void SavePlayerPrefs()
    {
       _componentGetter.PlayerAttackComponent.SaveAttackStats();
       _componentGetter.PlayerDash.SaveDashCooldown();
       _componentGetter.HealthComponent.SaveHealth();
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

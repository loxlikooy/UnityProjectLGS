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
        SaveManager.LoadQuestStates();
        MoveToStartPoint();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish"))
        {
            SaveManager.SaveQuestStates();
            SaveManager.SavePlayerStats(_componentGetter);
            StartCoroutine(MoveToEndPoint(1.5f, LoadNextLevel));
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
        int nextSceneIndex = (currentSceneIndex + 1) % totalScenes; // Loop to the next scene
        SceneManager.LoadScene(nextSceneIndex); // Load the next scene
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
        StartCoroutine(MoveToStartPointCoroutine(startPoint.position, 1.5f)); // Smooth transition over 1.5 seconds
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

        onEnd?.Invoke(); // Invoke the event after moving
    }
}

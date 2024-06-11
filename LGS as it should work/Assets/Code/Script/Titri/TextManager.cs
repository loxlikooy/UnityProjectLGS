using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float fadeDuration = 2.0f;
    public string[] texts;

    private int currentIndex = 0;

    void Start()
    {
        StartCoroutine(FadeInAndOut());
    }

    private IEnumerator FadeInAndOut()
    {
        while (true)
        {
            textMeshPro.text = texts[currentIndex];
            yield return StartCoroutine(FadeIn());
            yield return new WaitForSeconds(2.0f);
            yield return StartCoroutine(FadeOut());
            yield return new WaitForSeconds(1.0f);

            currentIndex++;

            if (currentIndex >= texts.Length)
            {
                Application.Quit();
                // Для завершения приложения в редакторе Unity
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                yield break;
            }
        }
    }

    private IEnumerator FadeIn()
    {
        Color color = textMeshPro.color;
        for (float t = 0.01f; t < fadeDuration; t += Time.deltaTime)
        {
            textMeshPro.color = new Color(color.r, color.g, color.b, Mathf.Lerp(0, 1, t / fadeDuration));
            yield return null;
        }
        textMeshPro.color = new Color(color.r, color.g, color.b, 1);
    }

    private IEnumerator FadeOut()
    {
        Color color = textMeshPro.color;
        for (float t = 0.01f; t < fadeDuration; t += Time.deltaTime)
        {
            textMeshPro.color = new Color(color.r, color.g, color.b, Mathf.Lerp(1, 0, t / fadeDuration));
            yield return null;
        }
        textMeshPro.color = new Color(color.r, color.g, color.b, 0);
    }
}
using Code.Script;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Enemy enemy; // Ссылка на компонент Enemy
    [SerializeField] private SpriteRenderer healthBarSprite; // Ссылка на спрайт health bar

    private Transform healthBarTransform; // Ссылка на transform спрайта health bar
    private float initialScaleX; // Исходный масштаб спрайта по оси X

    private void Start()
    {
        if (enemy == null || healthBarSprite == null)
        {
            Debug.LogError("EnemyHealthBar отсутствуют ссылки!");
            enabled = false;
            return;
        }

        initialScaleX = healthBarSprite.transform.localScale.x;
        healthBarTransform = healthBarSprite.transform;
    }

    private void Update()
    {
        if (enemy != null)
        {
            // Вычисляем процент здоровья
            float healthPercentage = enemy.GetCurrentHealth() / enemy.GetMaxHealth();

            // Меняем масштаб спрайта health bar на основе процента здоровья
            healthBarTransform.localScale = new Vector3(initialScaleX * healthPercentage, healthBarTransform.localScale.y, 1f);
        }
        else
        {
            // Если ссылка на enemy равна null, выключаем спрайт health bar
            healthBarTransform.localScale = new Vector3(0f, healthBarTransform.localScale.y, 1f);
        }
    }
}
using Code.Script;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private SpriteRenderer healthBarSprite;

    private Transform _healthBarTransform;
    private float _initialScaleX;
    private float _targetScaleX;

    private void Start()
    {
        if (enemy == null || healthBarSprite == null)
        {
            Debug.LogError("EnemyHealthBar отсутствуют ссылки!");
            enabled = false;
            return;
        }

        _initialScaleX = healthBarSprite.transform.localScale.x;
        _healthBarTransform = healthBarSprite.transform;
        _targetScaleX = _initialScaleX;
    }

    private void Update()
    {
        if (enemy != null)
        {
            float healthPercentage = enemy.GetCurrentHealth() / enemy.GetMaxHealth();
            _targetScaleX = _initialScaleX * healthPercentage;

            // Плавное изменение масштаба полосы здоровья
            _healthBarTransform.localScale = new Vector3(
                Mathf.Lerp(_healthBarTransform.localScale.x, _targetScaleX, Time.deltaTime * 10),
                _healthBarTransform.localScale.y,
                1f
            );
        }
        else
        {
            _healthBarTransform.localScale = new Vector3(0f, _healthBarTransform.localScale.y, 1f);
        }
    }
}
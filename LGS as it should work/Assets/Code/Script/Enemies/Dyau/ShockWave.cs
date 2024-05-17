using System.Collections;
using Code.Script;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public float damage;
    public float radius;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject dustParticlePrefab; // Particle system prefab for dust
    [SerializeField] private int particleCount = 10; // Number of particle instances to create
    [SerializeField] private Vector2 positionOffset = new Vector2(-0.05f, -0.2f); // Offset values

    private void Start()
    {
        AdjustPosition();
        DealDamage();
        CreateDustParticles();
        Destroy(gameObject, 1f); // Destroy the shockwave after 1 second
    }

    private void AdjustPosition()
    {
        transform.position += (Vector3)positionOffset;
    }

    private void DealDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            IDamagable damagable = enemy.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage);
            }
        }
    }

    private void CreateDustParticles()
    {
        for (int i = 0; i < particleCount; i++)
        {
            Vector2 randomPoint = (Vector2)transform.position + Random.insideUnitCircle * 1f; // Radius of 1
            GameObject dustParticle = Instantiate(dustParticlePrefab, randomPoint, Quaternion.identity, transform); // Set Dyau as parent
            StartCoroutine(DestroyParticleAfterTime(dustParticle, 0.5f)); // Destroy after 0.5 seconds
        }
    }

    private IEnumerator DestroyParticleAfterTime(GameObject particle, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(particle);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
using UnityEngine;

namespace Code.Script
{
    public abstract class Enemy : MonoBehaviour, IDamagable, IAttackable
    {
        public float health;
        public float damage; // урон, который враг может нанести

        public void TakeDamage(float damageAmount)
        {
            health -= damageAmount;
            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            // Handle enemy death logic
            Destroy(gameObject);
        }

        public void Attack(IDamagable target)
        {
            target.TakeDamage(damage);
        }
    }

}
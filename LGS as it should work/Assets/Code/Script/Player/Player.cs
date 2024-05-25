using UnityEngine;

namespace Code.Script
{
    public class Player : MonoBehaviour, IDamagable, IAttackable
    {
        private ComponentGetter _componentGetter;

        private void Awake()
        {
            _componentGetter = GetComponent<ComponentGetter>();
        }

        public void TakeDamage(float damageAmount)
        {

            _componentGetter.HealthComponent.TakeDamage(damageAmount);
        }

        public void Attack(IDamagable target)
        {
            _componentGetter.PlayerAttackComponent.ExecuteAttack(target);
        }

    }
}
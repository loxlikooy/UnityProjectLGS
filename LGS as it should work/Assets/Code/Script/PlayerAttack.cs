using UnityEngine;

namespace Code.Script
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField]
        private Weapon currentWeapon;
        private InputHandler _inputHandler;

        private void Awake()
        {
            _inputHandler = GetComponent<InputHandler>();
        }

        private void Update()
        {
            // Здесь можно добавить логику определения цели, если нужно.
            if (_inputHandler.OnAttack())
            {
                // Пример: ExecuteAttack(someTarget);
            }
        }

        public void ExecuteAttack(IDamagable target)
        {
            currentWeapon.Attack();
        }
    }
}
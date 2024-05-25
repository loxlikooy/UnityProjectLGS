using UnityEngine;

namespace Code.Script
{
    public class MoveVelocity : MonoBehaviour, IMoveVelocity
    {
        [SerializeField] private PlayerStatsSO playerStatsSO;
        
        private float _moveSpeed;
        private Vector2 _velocityVector;
        private ComponentGetter _componentGetter;

        private void Awake()
        {
            _componentGetter = GetComponent<ComponentGetter>();
            LoadPermanentStats();
            LoadTemporaryStats();
        }

        private void LoadPermanentStats()
        {
            _moveSpeed = playerStatsSO.moveSpeed;
        }

        private void LoadTemporaryStats()
        {
            if (PlayerPrefs.HasKey("TemporaryMoveSpeed"))
            {
                _moveSpeed = PlayerPrefs.GetFloat("TemporaryMoveSpeed");
            }
        }

        public void SetVelocity(Vector2 vector)
        {
            _velocityVector = vector;
        }

        private void FixedUpdate()
        {
            _componentGetter.PlayerRGB.velocity = (_velocityVector * (_moveSpeed * Time.fixedDeltaTime));
        }

        public void IncreaseSpeed(float amount)
        {
            _moveSpeed += amount;
            SaveTemporaryStats(); // Сохраняем временные изменения
        }
        

        private void SaveTemporaryStats()
        {
            PlayerPrefs.SetFloat("TemporaryMoveSpeed", _moveSpeed);
            PlayerPrefs.Save();
        }
    }
}
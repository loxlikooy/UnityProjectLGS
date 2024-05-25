using System.Collections;
using UnityEngine;

namespace Code.Script
{
    public class Dash : MonoBehaviour, IDash
    {
        [SerializeField] private PlayerStatsSO playerStatsSO;

        private Vector3 _dashEndPosition;
        private Vector3 _dashStartPosition;
        private float _dashTimeCounter;

        [SerializeField] private float _dashDuration = 0.2f;
        private float _timeSinceLastDash = Mathf.Infinity;

        [SerializeField] private LayerMask layersToCheck;

        private ComponentGetter _componentGetter;
        private float _currentDashDistance;
        private float _dashCooldown;

        private void Awake()
        {
            _componentGetter = GetComponent<ComponentGetter>();
            LoadPermanentStats();
            LoadTemporaryStats();
        }

        private void LoadPermanentStats()
        {
            _dashCooldown = playerStatsSO.dashCooldown;
            _currentDashDistance = playerStatsSO.dashRange;
        }

        private void LoadTemporaryStats()
        {
            if (PlayerPrefs.HasKey("TemporaryDashCooldown"))
            {
                _dashCooldown = PlayerPrefs.GetFloat("TemporaryDashCooldown");
            }
            if (PlayerPrefs.HasKey("TemporaryDashRange"))
            {
                _currentDashDistance = PlayerPrefs.GetFloat("TemporaryDashRange");
            }
        }

        public void HandleDash()
        {
            if (_timeSinceLastDash < _dashCooldown) return;

            Vector2 lastMoveDirection = _componentGetter.PlayerMovement.LastMoveDirection;
            Vector3 dashDirection3D = new Vector3(lastMoveDirection.x, lastMoveDirection.y, 0);
            Vector3 targetDashPosition = transform.position + dashDirection3D * _currentDashDistance;

            Vector3 rayStartPoint = transform.position + dashDirection3D * 0.2f; // Optional adjustment

            RaycastHit2D raycastHit2D = Physics2D.Raycast(rayStartPoint, dashDirection3D, _currentDashDistance, layersToCheck);

            if (raycastHit2D.collider != null)
            {
                Vector3 hitPoint3D = new Vector3(raycastHit2D.point.x, raycastHit2D.point.y, 0);
                targetDashPosition = hitPoint3D - dashDirection3D.normalized * 0.2f;
            }

            StartDash(targetDashPosition);
        }

        private void StartDash(Vector3 targetPosition)
        {
            _timeSinceLastDash = 0f;
            _dashStartPosition = transform.position;
            _dashEndPosition = targetPosition;
            _dashTimeCounter = 0f;

            StartCoroutine(DashCoroutine());
            _componentGetter.PlayerAnimator.SetDashAnimation();
        }

        private IEnumerator DashCoroutine()
        {
            while (_dashTimeCounter < _dashDuration)
            {
                _dashTimeCounter += Time.deltaTime;
                float lerpValue = _dashTimeCounter / _dashDuration;
                transform.position = Vector3.Lerp(_dashStartPosition, _dashEndPosition, lerpValue);
                yield return null;
            }
            transform.position = _dashEndPosition;
        }

        public void HandleDashInput()
        {
            if (_componentGetter.PlayerInputHandler.OnDash())
            {
                HandleDash();
            }
        }

        private void Update()
        {
            if (_timeSinceLastDash < _dashCooldown)
            {
                _timeSinceLastDash += Time.deltaTime;
            }

            PlayerHUDManager.Instance.DashColdown(_dashCooldown, Mathf.Clamp(_timeSinceLastDash, 0, _dashCooldown));
            HandleDashInput();
        }

        public void DecreaseDashCooldown(float amount)
        {
            _dashCooldown -= amount;
            SaveTemporaryStats(); // Сохраняем временные изменения
        }

        public void IncreaseDashRange(float amount)
        {
            _currentDashDistance += amount;
            SaveTemporaryStats(); // Сохраняем временные изменения
        }

        public void SaveTemporaryStats()
        {
            PlayerPrefs.SetFloat("TemporaryDashCooldown", _dashCooldown);
            PlayerPrefs.SetFloat("TemporaryDashRange", _currentDashDistance);
            PlayerPrefs.Save();
        }
    }
}

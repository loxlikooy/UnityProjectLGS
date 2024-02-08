using System.Collections;
using UnityEngine;

namespace Code.Script
{
    public class Dash : MonoBehaviour, IDash
    {
        private bool _isDashing;
        private Vector3 _dashEndPosition;
        private Vector3 _dashStartPosition;
        private float _dashTimeCounter;
        private float _dashDuration = 0.2f; // Duration of the dash in seconds
        private float _dashCooldown = 2f;
        private float _timeSinceLastDash = Mathf.Infinity;
        
        
        [SerializeField]
        private LayerMask layersToCheck;
        
        private ComponentGetter _componentGetter;

        private void Awake()
        {
            _componentGetter = GetComponent<ComponentGetter>();
        }

        public void HandleDash()
        {
            if (_isDashing || _timeSinceLastDash < _dashCooldown) return;

            Vector2 lastMoveDirection = _componentGetter.PlayerMovement.LastMoveDirection;
            Vector3 dashDirection3D = new Vector3(lastMoveDirection.x, lastMoveDirection.y, 0);
            float dashAmount = 0.8f;
            Vector3 targetDashPosition = transform.position + dashDirection3D * dashAmount;

            Vector3 rayStartPoint = transform.position + dashDirection3D * 0.2f;
    
            // Добавляем layersToCheck в рейкаст
            RaycastHit2D raycastHit2D = Physics2D.Raycast(rayStartPoint, dashDirection3D, dashAmount, layersToCheck);

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
            _isDashing = true;

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
            _isDashing = false;
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

        public void DecreaseDashCooldown()
        {
            _dashCooldown = _dashCooldown - 0.5f;
        }
    }
}
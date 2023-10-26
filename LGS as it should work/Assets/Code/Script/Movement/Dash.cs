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
        private float _dashDuration = 0.2f;  // Duration of the dash in seconds
        private PlayerAnimator _playerAnimator;
        [SerializeField]
        private LayerMask layersToCheck;
        
        public InputHandler inputHandler;
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerAnimator = GetComponent<PlayerAnimator>();

        }

        public void HandleDash()
        {
            if (_isDashing) return;

            Vector2 lastMoveDirection = _playerMovement.LastMoveDirection;
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
            _dashStartPosition = transform.position;
            _dashEndPosition = targetPosition;
            _dashTimeCounter = 0f;
            _isDashing = true;

            StartCoroutine(DashCoroutine());
            _playerAnimator.SetDashAnimation();

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
            if (inputHandler.OnDash())
            {
                HandleDash();
            }
        }

        private void Update()
        {
            HandleDashInput();
        }
    }
}
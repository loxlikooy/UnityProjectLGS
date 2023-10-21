using UnityEngine;

namespace Code.Script
{
    public class DashAccelerated : MonoBehaviour, IDash
    {
        private bool _isDashButtonDown;
        private Rigidbody2D _rigidbody2D;
        public InputHandler inputHandler;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void HandleDash()
        {
            // Обновляем _lastMoveDirection перед использованием
            Vector2 _lastMoveDirection = GetComponent<PlayerMovement>().LastMoveDirection;
            
            if (_isDashButtonDown)
            {
                Vector3 dashDirection3D = new Vector3(_lastMoveDirection.x, _lastMoveDirection.y, 0);
                
                float dashAmount = 1.2f;
                Vector3 dashPosition = transform.position + dashDirection3D * dashAmount;
                Debug.Log(dashPosition);
                
                
                
                Vector3 rayStartPoint = transform.position + dashDirection3D * 0.2f;
                RaycastHit2D raycastHit2D = Physics2D.Raycast(rayStartPoint, dashDirection3D, dashAmount);
               

                if (raycastHit2D.collider != null)
                {
                    Vector3 hitPoint3D = new Vector3(raycastHit2D.point.x, raycastHit2D.point.y, 0);
                    dashPosition = hitPoint3D - dashDirection3D.normalized * 0.2f;
                }

                transform.position = dashPosition;

                _isDashButtonDown = false;
            }
        }


        public void HandleDashInput()
        {
            if (inputHandler.OnDash()) 
            {
                _isDashButtonDown = true;    
            }
        }
        
        private void Update()
        {
            HandleDashInput();
            HandleDash();
        }
    }
}
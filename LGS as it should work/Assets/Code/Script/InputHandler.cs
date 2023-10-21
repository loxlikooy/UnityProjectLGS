
using UnityEngine;


namespace Code.Script
{
    public class InputHandler : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;

        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();
        }

        public Vector2 GetMovementVectorNormalized()
        {
            Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
            inputVector = inputVector.normalized;
            return inputVector;
        }

        public bool OnDash()
        {
            bool isDashButtonDown = _playerInputActions.Player.Dash.triggered;
            return isDashButtonDown;
        }
    }
}
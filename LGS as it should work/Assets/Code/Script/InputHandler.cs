
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
            return inputVector.normalized;
        }

        public bool OnDash()
        {
            return _playerInputActions.Player.Dash.triggered;
        }

        public bool OnAttack()
        {
            return _playerInputActions.Player.Fire.triggered;
        }
    }
}

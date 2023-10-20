using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

        public delegate void MoveInputDelegate(Vector2 moveInput);
        public event MoveInputDelegate OnMoveInputReceived;

        private void OnMove(InputValue movementValue)
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            OnMoveInputReceived?.Invoke(moveInput);
            
        }

// надо использовать это:
// а не то что сверху
        public Vector2 GetMovementVectorNormalized() {
            Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

            inputVector = inputVector.normalized;
            return inputVector;
        }
        
        //
        public bool OnDash()
        {
            bool isDashButtonDown = _playerInputActions.Player.Dash.triggered;
            if(isDashButtonDown) Debug.Log("dash");
            return isDashButtonDown;

        }
    }
}
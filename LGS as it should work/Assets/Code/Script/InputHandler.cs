using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Script
{
    public class InputHandler : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;
        [SerializeField] private InputHandler _inputHandler;

        private void Start()
        {
            _inputHandler = GameObject.Find("GameInput").GetComponent<InputHandler>();

        }

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
            if(isDashButtonDown) Debug.Log("dash");
            return isDashButtonDown;
        }
    }
}
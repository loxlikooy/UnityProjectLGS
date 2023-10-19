using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Script
{
    public class InputHandler : MonoBehaviour
    {
        public delegate void MoveInputDelegate(Vector2 moveInput);
        public event MoveInputDelegate OnMoveInputReceived;

        private void OnMove(InputValue movementValue)
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            OnMoveInputReceived?.Invoke(moveInput);
        }
    }
}
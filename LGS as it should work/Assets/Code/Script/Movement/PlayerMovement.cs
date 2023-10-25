using System.Collections.Generic;
using UnityEngine;
// Assume Code.Script namespace contains InputHandler

// Ensure the GameObject has an InputHandler component
namespace Code.Script
{
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerAnimator _playerAnimator; 
        [SerializeField]
        private float moveSpeed = 1f; 
        private const float CollisionOffset = 0.01f; 
        private MoveVelocity _moveVelocity;

        [SerializeField]
        private ContactFilter2D movementFilter; 

        private Vector2 _movementInput;
    
        private Vector2 _lastMoveDirection;
    
        public Vector2 LastMoveDirection => _lastMoveDirection;
    
        private Rigidbody2D _rigidbody2D; 
        private List<RaycastHit2D> _castCollisions = new List<RaycastHit2D>();

        private InputHandler _inputHandler;
    
        private void Start()
        {
            _playerAnimator = GetComponent<PlayerAnimator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _inputHandler = GetComponent<InputHandler>();
             GetComponent<IMoveVelocity>().SetVelocity(_movementInput);
             _moveVelocity = GetComponent<MoveVelocity>();


        }
    
        private void AssignLastMoveDirection()
        {
            if ((_movementInput.x != 0 || _movementInput.y != 0))
            {
                _lastMoveDirection = _movementInput;  
            } 
        }
    
        private void FixedUpdate()
        {
            _movementInput = _inputHandler.GetMovementVectorNormalized();
            _playerAnimator.SetMovementAnimation(_movementInput); 
            HandleMovement();
            AssignLastMoveDirection();
        }

        private void HandleMovement()
        {
            if (!CheckIfPlayerCanMoveInDirection(_movementInput) &&
                !CheckIfPlayerCanMoveInDirection(new Vector2(_movementInput.x, 0)) &&
                !CheckIfPlayerCanMoveInDirection(new Vector2(0, _movementInput.y)))
            {
                // Add logic here if the player can't move in any direction
            }
        }

        private bool CheckIfPlayerCanMoveInDirection(Vector2 moveDirection)
        {
            if (CastAgainstCollidables(moveDirection) == 0)
            {
                _moveVelocity.SetVelocity(moveDirection * moveSpeed);
                return true;
            }
            return false;
        }

        private int CastAgainstCollidables(Vector2 moveDirection)
        {
            return _rigidbody2D.Cast(
                moveDirection,
                movementFilter,
                _castCollisions,
                moveSpeed * Time.fixedDeltaTime + CollisionOffset
            );
        }
    }
}
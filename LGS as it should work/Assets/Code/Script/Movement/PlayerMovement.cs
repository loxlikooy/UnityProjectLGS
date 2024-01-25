using System.Collections.Generic;
using UnityEngine;

namespace Code.Script
{
    public class PlayerMovement : MonoBehaviour
    {
        //
        private ComponentGetter _componentGetter;
        //
        [SerializeField]
        private float moveSpeed = 1f; 
        private const float CollisionOffset = 0.01f; 
        

        [SerializeField]
        private ContactFilter2D movementFilter; 

        private Vector2 _movementInput;
    
        private Vector2 _lastMoveDirection;
    
        public Vector2 LastMoveDirection => _lastMoveDirection;
    
       
        private List<RaycastHit2D> _castCollisions = new List<RaycastHit2D>();

        
    
        private void Start()
        {
             _componentGetter = GetComponent<ComponentGetter>();
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
            _movementInput = _componentGetter.PlayerInputHandler.GetMovementVectorNormalized();
            _componentGetter.PlayerAnimator.SetMovementAnimation(_movementInput); 
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
                _componentGetter.PlayerMoveVelocity.SetVelocity(moveDirection * moveSpeed);
                return true;
            }
            return false;
        }

        private int CastAgainstCollidables(Vector2 moveDirection)
        {
            return _componentGetter.PlayerRGB.Cast(
                moveDirection,
                movementFilter,
                _castCollisions,
                moveSpeed * Time.fixedDeltaTime + CollisionOffset
            );
        }
    }
}
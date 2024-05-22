using System.Collections.Generic;
using UnityEngine;

namespace Code.Script
{
    public class PlayerMovement : MonoBehaviour
    {
        private ComponentGetter _componentGetter;
        [SerializeField]
        private float moveSpeed = 1f; 
        private const float CollisionOffset = 0.01f; 
        [SerializeField]
        private ContactFilter2D movementFilter; 
        private Vector2 _movementInput;
        private Vector2 _lastMoveDirection;
        public Vector2 LastMoveDirection => _lastMoveDirection;
        private readonly List<RaycastHit2D> _castCollisions = new List<RaycastHit2D>();

        private bool _isConfused = false;
        private float _confusionDuration = 5f; // Duration of the confusion effect
        private float _confusionTime = 0f;

        private void Start()
        {
            _componentGetter = GetComponent<ComponentGetter>();
        }

        private void OnEnable()
        {
            Artifacts.OnArtifactQumyzPickedUp += ConfuseInput;
        }

        private void OnDisable()
        {
            Artifacts.OnArtifactQumyzPickedUp -= ConfuseInput;
        }

        private void FixedUpdate()
        {
            _movementInput = _componentGetter.PlayerInputHandler.GetMovementVectorNormalized();
            
            if (_isConfused)
            {
                _movementInput = ConfuseDirection(_movementInput);
                _confusionTime += Time.fixedDeltaTime;
                if (_confusionTime >= _confusionDuration)
                {
                    _isConfused = false;
                    _confusionTime = 0f;
                }
            }
            
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

        private void AssignLastMoveDirection()
        {
            if ((_movementInput.x != 0 || _movementInput.y != 0))
            {
                _lastMoveDirection = _movementInput;  
            } 
        }

        private void ConfuseInput()
        {
            _isConfused = true;
            _confusionTime = 0f;
        }

        private Vector2 ConfuseDirection(Vector2 originalDirection)
        {
            // Randomly alter the direction
            return new Vector2(
                _movementInput.y,
                _movementInput.x
            ).normalized;
        }
    }
}

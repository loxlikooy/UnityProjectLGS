using System.Collections.Generic;
using UnityEngine;

// Assume Code.Script namespace contains InputHandler
using Code.Script;
using UnityEngine.Serialization;

// Ensure the GameObject has an InputHandler component
[RequireComponent(typeof(InputHandler))]
public class PlayerController : MonoBehaviour
{
    [FormerlySerializedAs("_moveSpeed")] [SerializeField]
    private float moveSpeed = 1f; 

    private const float CollisionOffset = 0.001f; 

    [FormerlySerializedAs("_movementFilter")] [SerializeField]
    private ContactFilter2D movementFilter; 

    private Vector2 _movementInput; 
    private Rigidbody2D _rigidbody2D; 
    private List<RaycastHit2D> _castCollisions = new List<RaycastHit2D>();

    private InputHandler _inputHandler; 

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _inputHandler = GetComponent<InputHandler>();
        
        _inputHandler.OnMoveInputReceived += HandleMoveInput;
    }

    private void FixedUpdate()
    {
        if (_movementInput == Vector2.zero) return;

        if (!AttemptMove(_movementInput) && 
            !AttemptMove(new Vector2(_movementInput.x, 0)) && 
            !AttemptMove(new Vector2(0, _movementInput.y)))
        {
            // Add logic here if the player can't move in any direction
        }
    }

    private bool AttemptMove(Vector2 moveDirection)
    {
        int collisionCount = CastAgainstCollidables(moveDirection);

        if (collisionCount == 0)
        {
            _rigidbody2D.MovePosition( _rigidbody2D.position + (moveDirection * Time.deltaTime));
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

    private void HandleMoveInput(Vector2 moveInput)
    {
        _movementInput = moveInput;
    }
}
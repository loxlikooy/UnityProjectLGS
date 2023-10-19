using System;
using System.Collections.Generic;
using UnityEngine;

// Assume Code.Script namespace contains InputHandler
using Code.Script;
using Unity.VisualScripting;
using UnityEngine.Serialization;

// Ensure the GameObject has an InputHandler component
[RequireComponent(typeof(InputHandler))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 1f; 
    private const float CollisionOffset = 0.005f; 

    [SerializeField]
    private ContactFilter2D _movementFilter; 

    private Vector2 _movementInput; 
    private Rigidbody2D _rigidbody2D; 
    private List<RaycastHit2D> _castCollisions = new List<RaycastHit2D>();

    private InputHandler _inputHandler;

    private Vector2 _lastMoveDirection;
    
    public Vector2 LastMoveDirection { get; }
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _inputHandler = GetComponent<InputHandler>();
        
        _inputHandler.OnMoveInputReceived += HandleMoveInput;
    }

    private void Update()
    {
        AssignLastMoveDirection();
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
        HandleMovement();
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
            _rigidbody2D.MovePosition(_rigidbody2D.position + (moveDirection * Time.deltaTime));
            return true;
        }
        return false;
    }

    private int CastAgainstCollidables(Vector2 moveDirection)
    {
        return _rigidbody2D.Cast(
            moveDirection,
            _movementFilter,
            _castCollisions,
            _moveSpeed * Time.fixedDeltaTime + CollisionOffset
        );
    }

    private void HandleMoveInput(Vector2 moveInput)
    {
        _movementInput = moveInput;
    }
}
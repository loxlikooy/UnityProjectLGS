using System.Collections.Generic;
using Code.Script;
using UnityEngine;

// Требуется компонент InputHandler для работы
[RequireComponent(typeof(InputHandler))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 1f; // Скорость движения игрока

    private const float CollisionOffset = 0.001f; // Отступ для предотвращения застревания

    [SerializeField]
    private ContactFilter2D _movementFilter; // Фильтр для определения объектов, с которыми игрок может столкнуться

    private Vector2 _movementInput; // Направление движения игрока
    private Rigidbody2D _rigidbody2D; // Ссылка на Rigidbody2D компонент игрока
    private List<RaycastHit2D> _castCollisions = new List<RaycastHit2D>(); // Список для хранения результатов лучевого просеивания

    private InputHandler _inputHandler; // Ссылка на компонент обработки ввода

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _inputHandler = GetComponent<InputHandler>();
        
        // Подписываемся на событие получения данных о движении
        _inputHandler.OnMoveInputReceived += HandleMoveInput;
    }

    private void FixedUpdate()
    {
        // Если нет направления движения, прекращаем выполнение функции
        if (_movementInput == Vector2.zero) return;

        // Попытка движения игрока
        if (!AttemptMove(_movementInput) && 
            !AttemptMove(new Vector2(_movementInput.x, 0)) && 
            !AttemptMove(new Vector2(0, _movementInput.y)))
        {
            // Обработка ситуации, когда игрок не может двигаться ни в каком направлении
        }
    }

    // Метод попытки движения игрока в заданном направлении
    private bool AttemptMove(Vector2 moveDirection)
    {
        int collisionCount = CastAgainstCollidables(moveDirection);

        // Если нет препятствий, двигаем игрока
        if (collisionCount == 0)
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + moveDirection * _moveSpeed * Time.deltaTime);
            return true;
        }

        return false; // Возвращаем false, если есть препятствия
    }

    // Метод для определения столкновений в заданном направлении
    private int CastAgainstCollidables(Vector2 moveDirection)
    {
        return _rigidbody2D.Cast(
            moveDirection,
            _movementFilter,
            _castCollisions,
            _moveSpeed * Time.fixedDeltaTime + CollisionOffset
        );
    }

    // Метод обработки данных о направлении движения
    private void HandleMoveInput(Vector2 moveInput)
    {
        _movementInput = moveInput;
    }
}
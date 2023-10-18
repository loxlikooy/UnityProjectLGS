using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // Скорость движения игрока в единицах в секунду
    private float _moveSpeed = 1f;
    
    // Отступ для предотвращения застревания игрока из-за незначительных столкновений
    private float _collisionOffSet = 0.05f;
    
    // Фильтр для определения объектов, с которыми игрок может столкнуться
    public ContactFilter2D movementFilter; //Паблик для получения доступа в юнити
    
    // Вектор для отслеживания направления движения игрока на основе пользовательского ввода
    Vector2 _movementInput;
    
    // Ссылка на компонент Rigidbody2D, который управляет физикой игрока
    Rigidbody2D _rigidbody2D;
    
    // Список для хранения потенциальных точек столкновения при движении игрока
    List<RaycastHit2D> _castCollisions = new List<RaycastHit2D>();

    // Инициализация: получение ссылки на компонент Rigidbody2D в начале игры
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Логика перемещения игрока
    private void FixedUpdate()
    {     //// Проверяет, было ли задано направление движения игрока
        if (_movementInput != Vector2.zero)
        {
            //смотрит уперся ли игрок в объект
           bool success = TryMove(_movementInput);
           if (!success)
           {//запрещает ходить по X вектору
               success = TryMove(new Vector2(_movementInput.x, 0));
               if (!success)
               {  //запрещает ходить по Y вектору 
                   success = TryMove(new Vector2(0, _movementInput.y));
               }
               {
                   
               }
           }
        }
            
    }
    //логика для того, чтоб персонаж ходил если уперется в объект по одной оси
    private bool TryMove(Vector2 moveDirection)
    {
        // Выполняем лучевое просеивание (raycasting) в направлении движения игрока, чтобы определить потенциальные столкновения
        int count = _rigidbody2D.Cast(moveDirection, // Направление движения игрока
            movementFilter, // Фильтр, определяющий, с какими объектами может произойти столкновение
            _castCollisions, // Список, в который сохраняются результаты лучевого просеивания
            _moveSpeed * Time.fixedDeltaTime +
            _collisionOffSet); // Расстояние, на которое игрок пытается переместиться, увеличенное на отступ для предотвращения застревания
        if (count == 0)
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + moveDirection * _moveSpeed * Time.deltaTime);
            //возвращает тру если не уперся
            return true;
        }
        else
        {//возвращает фолс если уперся в объект
            return false;
        }
    }







    // Получение данных о направлении движения от пользователя
    void OnMove(InputValue movementValue)
    {
        _movementInput = movementValue.Get<Vector2>();
    }
}
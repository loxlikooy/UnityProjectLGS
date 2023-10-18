using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // Скорость движения игрока в единицах в секунду
    private float moveSpeed = 1f;
    
    // Отступ для предотвращения застревания игрока из-за незначительных столкновений
    private float collisionOffSet = 0.05f;
    
    // Фильтр для определения объектов, с которыми игрок может столкнуться
    public ContactFilter2D movementFilter; //Паблик для получения доступа в юнити
    
    // Вектор для отслеживания направления движения игрока на основе пользовательского ввода
    Vector2 movementInput;
    
    // Ссылка на компонент Rigidbody2D, который управляет физикой игрока
    Rigidbody2D rb;
    
    // Список для хранения потенциальных точек столкновения при движении игрока
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Инициализация: получение ссылки на компонент Rigidbody2D в начале игры
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Логика перемещения игрока
    private void FixedUpdate()
    {     //// Проверяет, было ли задано направление движения игрока
        if (movementInput != Vector2.zero)
        {
            //смотрит уперся ли игрок в объект
           bool success = TryMove(movementInput);
           if (!success)
           {//запрещает ходить по X вектору
               success = TryMove(new Vector2(movementInput.x, 0));
               if (!success)
               {  //запрещает ходить по Y вектору 
                   success = TryMove(new Vector2(0, movementInput.y));
               }
               {
                   
               }
           }
        }
            
    }
    //логика для того, чтоб персонаж ходил если уперется в объект по одной аксцизе
    private bool TryMove(Vector2 direction)
    {
        // Выполняем лучевое просеивание (raycasting) в направлении движения игрока, чтобы определить потенциальные столкновения
        int count = rb.Cast(direction, // Направление движения игрока
            movementFilter, // Фильтр, определяющий, с какими объектами может произойти столкновение
            castCollisions, // Список, в который сохраняются результаты лучевого просеивания
            moveSpeed * Time.fixedDeltaTime +
            collisionOffSet); // Расстояние, на которое игрок пытается переместиться, увеличенное на отступ для предотвращения застревания
        if (count == 0)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
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
        movementInput = movementValue.Get<Vector2>();
    }
}
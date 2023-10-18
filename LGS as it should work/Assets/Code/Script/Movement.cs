using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

////////////////////////////////

    public class Movement
    {
        // Вектор для отслеживания направления движения игрока на основе пользовательского ввода
        public Vector2 movementInput;
        
        // Скорость движения игрока в единицах в секунду
        private float moveSpeed = 1f;
    
        // Отступ для предотвращения застревания игрока из-за незначительных столкновений
        private float collisionOffSet = 0.05f;
    
        // Фильтр для определения объектов, с которыми игрок может столкнуться
         ContactFilter2D movementFilter; //Паблик для получения доступа в юнити
    
         // Ссылка на компонент Rigidbody2D, который управляет физикой игрока
         Rigidbody2D rb;
    
        // Список для хранения потенциальных точек столкновения при движении игрока
        List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
        
      
        public void movement()
        {
            xCollisionMovement();
        }
        private void xCollisionMovement()
        {
       
        
            // Выполняем лучевое просеивание (raycasting) в направлении движения игрока, чтобы определить потенциальные столкновения
            int count = rb.Cast(movementInput, // Направление движения игрока
                movementFilter,                // Фильтр, определяющий, с какими объектами может произойти столкновение
                castCollisions,                // Список, в который сохраняются результаты лучевого просеивания
                moveSpeed * Time.fixedDeltaTime + collisionOffSet); // Расстояние, на которое игрок пытается переместиться, увеличенное на отступ для предотвращения застревания
            if (count == 0)
            {
                rb.MovePosition(rb.position + movementInput * moveSpeed * Time.deltaTime);
                
            }

        }
        void OnMove(InputValue movementValue)
        {
            movementInput = movementValue.Get<Vector2>();
        }
    }
    


using UnityEngine;

namespace Code.Script
{
    public class Shekotalka : Enemy
    {
        public float attractionForce = 10f;     // Сила притяжения
        public float attractionCooldown = 5f;   // Кулдаун притяжения в секундах

        private Transform playerTransform;      // Позиция игрока
        private float nextAttractionTime = 0f;  // Время следующего притяжения

        public float attractionDuration = 0.5f;  // Duration for which the attraction force is applied
        private bool isAttracting = false;        // Flag to check if currently attracting
        private float attractionEndTime;          // Time when attraction should stop

        
        private void Start()
        {
            base.Start();  // Call the Start method of the Enemy class first

            playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Найдите игрока по тегу "Player"
        }

        // private void Update()
        // {
        //     base.Update();  // Call the Update method of the Enemy class
        //
        //     
        //     if (Time.time > nextAttractionTime)
        //     {
        //         AttractPlayer();
        //         nextAttractionTime = Time.time + attractionCooldown;  // Обновите время следующего притяжения
        //     }
        // }
        

        private void FixedUpdate()
        {
            if (!isAttracting && Time.time > nextAttractionTime)
            {
                StartAttraction();
            }

            if (isAttracting)
            {
                AttractPlayer();
                if (Time.time > attractionEndTime)
                {
                    StopAttraction();
                }
            }
        }
        
        private void StartAttraction()
        {
            isAttracting = true;
            attractionEndTime = Time.time + attractionDuration;
            nextAttractionTime = Time.time + attractionCooldown;
        }

        private void StopAttraction()
        {
            isAttracting = false;
        }

        private void AttractPlayer()
        {
            Vector2 directionToPlayer = transform.position - playerTransform.position;
            directionToPlayer.Normalize();

            Rigidbody2D playerRb = playerTransform.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.AddForce(directionToPlayer * attractionForce, ForceMode2D.Force);
            }
        }
        

    }
}
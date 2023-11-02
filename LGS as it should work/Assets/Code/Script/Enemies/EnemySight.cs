namespace Code.Script
{
    using UnityEngine;

    // EnemySight.cs
    public class EnemySight : MonoBehaviour
    {
        public bool CanSeePlayer(Transform player)
        {
            // Implement player detection logic, e.g., raycasting
            RaycastHit hit;
            Vector3 directionToPlayer = player.position - transform.position;

            if (Physics.Raycast(transform.position, directionToPlayer, out hit))
            {
                if (hit.transform == player)
                {
                    return true;
                }
            }

            return false;
        }
    }


}
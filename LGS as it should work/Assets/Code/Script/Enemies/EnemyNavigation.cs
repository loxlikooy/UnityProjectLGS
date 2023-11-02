using UnityEngine;

public class EnemyNavigation : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float arrivalThreshold = 0.1f; // How close the enemy needs to be to the target to consider it reached
    private Vector2 targetPosition;
    private Rigidbody2D rb;
    private bool hasTarget = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDestination(Vector2 destination)
    {
        targetPosition = destination;
        hasTarget = true;
    }

    private void FixedUpdate()
    {
        if (hasTarget)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        Vector2 direction = (targetPosition - rb.position).normalized;
        rb.velocity = direction * moveSpeed;
        
        // Check if the destination is reached within a certain threshold
        if (Vector2.Distance(rb.position, targetPosition) < arrivalThreshold)
        {
            hasTarget = false; // Target reached
        }
    }

    // Method to check if the enemy has reached its destination
    public bool HasReachedDestination()
    {
        return !hasTarget;
    }
}
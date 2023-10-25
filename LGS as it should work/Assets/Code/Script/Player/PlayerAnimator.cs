using UnityEngine;

namespace Code.Script
{
    public class PlayerAnimator : MonoBehaviour
    {
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetAttackAnimation()
        {
            _animator.SetTrigger("Attack");
        }

        public void SetMovementAnimation(Vector2 movement)
        {
            _animator.SetFloat("MoveX", movement.x);

            // Если персонаж не двигается по горизонтали, установите IsIdle в true
            if (movement.x == 0)
            {
                _animator.SetBool("IsIdle", true);
            }
            else
            {
                _animator.SetBool("IsIdle", false);
                _spriteRenderer.flipX = movement.x < 0;
            }
        }

        public bool IsAttacking()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
        }
    }
}
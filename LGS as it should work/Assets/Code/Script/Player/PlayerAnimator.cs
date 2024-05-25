using UnityEditor;
using UnityEngine;

namespace Code.Script
{
    public class PlayerAnimator : MonoBehaviour
    {
        
       
        private ComponentGetter _componentGetter;

        private void Awake()
        {
            _componentGetter = GetComponent<ComponentGetter>();
        }

        public void SetAttackAnimation()
        {
            _componentGetter.Animator.SetTrigger("Attack");
        }

        public void SetMovementAnimation(Vector2 movement)
        {
            _componentGetter.Animator.SetFloat("MoveX", movement.x);

            /*// Если персонаж не двигается по горизонтали, установите IsIdle в true
            if (movement.x == 0)
            {
                _componentGetter.Animator.SetBool("IsIdle", true);
            }
            else
            {
                _componentGetter.Animator.SetBool("IsIdle", false);
                _componentGetter.SpriteRenderer.flipX = movement.x < 0;
            }     */  
            // Если персонаж не двигается по горизонтали, установите IsIdle в true
            if (movement.x != 0 || movement.y !=0)
            {
                _componentGetter.Animator.SetBool("IsIdle", false);
                _componentGetter.SpriteRenderer.flipX = movement.x < 0;

            }
            else
            {
                _componentGetter.Animator.SetBool("IsIdle", true);
            }
        }

        public bool IsAttacking()
        {
            return _componentGetter.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
        }
        
        public void SetDashAnimation()
        {
            _componentGetter.Animator.SetTrigger("Dash");
        }

        public void PlayDamageTakingAnimation()
        {
            _componentGetter.Animator.SetTrigger("TakeDamage");
        }
        
        public void SetDeathAnimation()
        {
            _componentGetter.Animator.SetTrigger("Death");
        }
    }
}
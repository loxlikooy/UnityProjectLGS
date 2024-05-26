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

        public void SetMovementAnimation(Vector2 movement, Vector2 lastMoveDirection)
        {
            _componentGetter.Animator.SetFloat("MoveX", movement.x != 0 ? movement.x : movement.y);

            if (movement.x != 0 || movement.y != 0) 
            {
                _componentGetter.Animator.SetBool("IsIdle", false);
            }
            else
            {
                _componentGetter.Animator.SetBool("IsIdle", true);
            }
            
            if (lastMoveDirection.x != 0)
            {
                _componentGetter.SpriteRenderer.flipX = lastMoveDirection.x < 0;
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
using UnityEngine;

namespace Code.Script
{
    public class ComponentGetter : MonoBehaviour
    {
        public Health HealthComponent => GetComponent<Health>();
        public PlayerMovement Movement => GetComponent<PlayerMovement>();
        public PlayerAttack PlayerAttackComponent => GetComponent<PlayerAttack>();
        public Dash PlayerDash => GetComponent<Dash>();
        public Animator Animator => GetComponent<Animator>();
        public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();
        public Rigidbody2D PlayerRGB => GetComponent<Rigidbody2D>();
        public PlayerAnimator PlayerAnimator => GetComponent<PlayerAnimator>();
        public InputHandler PlayerInputHandler => GetComponent<InputHandler>();
        public MoveVelocity PlayerMoveVelocity => GetComponent<MoveVelocity>();
        public PlayerMovement PlayerMovement => GetComponent<PlayerMovement>();
    }
}
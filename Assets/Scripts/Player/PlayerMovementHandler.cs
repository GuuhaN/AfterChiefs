using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class PlayerMovementHandler : MonoBehaviour
    {
        [SerializeField, Range(100, 2000)] private int JumpForce;
        [SerializeField, Range(5, 20)] private int GroundedDistance;
        [SerializeField, Range(1, 10)] private int MovementSpeed;

        private Animator m_Animator;
        private Rigidbody2D m_Rigidbody2D;
        private SpriteRenderer m_SpriteRenderer;
        private PlayerInputActions m_PlayerInputActions;

        private bool _spriteFlipped;

        void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_SpriteRenderer = GetComponent<SpriteRenderer>();

            m_PlayerInputActions = new PlayerInputActions();
            m_PlayerInputActions.Enable();
            m_PlayerInputActions.Player.Jump.performed += Jump;
        }
        
        void Update()
        {
            Movement();
            
            m_Animator.SetBool("Jump",!IsGrounded());
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (IsGrounded() && context.performed)
            {
                m_Rigidbody2D.AddForce(Vector2.up * JumpForce);
            }
        }

        private void Movement()
        {
            var movementInput = m_PlayerInputActions.Player.Movement.ReadValue<Vector2>();
            m_Rigidbody2D.velocity = new Vector2(movementInput.x * MovementSpeed, m_Rigidbody2D.velocity.y);

            m_Animator.SetFloat("Movement", Mathf.Abs(m_Rigidbody2D.velocity.x));

            if (movementInput.x < 0 && !_spriteFlipped)
                _spriteFlipped = true;
            else if (movementInput.x > 0 && _spriteFlipped)
                _spriteFlipped = false;

            m_SpriteRenderer.flipX = _spriteFlipped;
        }

        private bool IsGrounded()
        {
            int layerMask = 1 << 9;
            int groundedDistanceNormalized = GroundedDistance / 5;
            layerMask = ~layerMask;

            return Physics2D.Raycast(transform.position, Vector2.down, groundedDistanceNormalized, layerMask) ||
                   Physics2D.Raycast(transform.position, new Vector2(.75f, -1f), groundedDistanceNormalized * 1.35f, layerMask) ||
                   Physics2D.Raycast(transform.position, new Vector2(-.75f, -1f), groundedDistanceNormalized * 1.35f, layerMask);
        }
    }
}

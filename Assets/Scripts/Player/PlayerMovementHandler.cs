using System;
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

        private bool _spriteFlipped;

        private Vector2 movementInput = Vector2.zero;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        void Update()
        {
            IsGrounded();
            Movement();
            
            m_Animator.SetBool("Jump",!IsGrounded());
            m_Animator.SetFloat("Movement", Mathf.Abs(m_Rigidbody2D.velocity.x));
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (IsGrounded() && context.performed)
            {
                m_Rigidbody2D.AddForce(Vector2.up * JumpForce);
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
        }

        public void Movement()
        {
            m_Rigidbody2D.velocity = new Vector2(movementInput.x * MovementSpeed, m_Rigidbody2D.velocity.y);

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

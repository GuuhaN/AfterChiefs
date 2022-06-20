using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Animator),typeof(PlayerStats))]
    public class PlayerDamageHandler : MonoBehaviour
    {
        [SerializeField] private GameObject AttackPoint;
        [SerializeField] private float AttackRadius;
        [SerializeField, Range(1, 10)] private int DamageMultiplier;
        [SerializeField] private LayerMask HitLayers;

        private Animator m_Animator;

        private PlayerStats _PlayerStats;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            _PlayerStats = GetComponent<PlayerStats>();
        }
        
        public void Attack(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            m_Animator.SetTrigger("Attack");

            Collider2D[] hitEnemies =
                Physics2D.OverlapCircleAll(AttackPoint.transform.position, AttackRadius, HitLayers);

            foreach (var hit in hitEnemies)
            {
                hit.gameObject.GetComponent<PlayerStats>().Damage(2 * DamageMultiplier);
                Debug.Log(hit.gameObject.name);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _PlayerStats.Damage(2 * DamageMultiplier);
                m_Animator.SetTrigger("Damaged");
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            if (!AttackPoint) return;
                Gizmos.DrawWireSphere(AttackPoint.transform.position, AttackRadius);
        }
    }
}

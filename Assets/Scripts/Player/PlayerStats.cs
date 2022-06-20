using System;
using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {

        private float _currentHealth;

        private PlayerDamageHandler _PlayerDamageHandler;
        void Start()
        {
            _PlayerDamageHandler = GetComponent<PlayerDamageHandler>();
        }

        public void Damage(int damageInput)
        {
            _currentHealth += damageInput;
            Debug.Log(_currentHealth);
        }
    }
}

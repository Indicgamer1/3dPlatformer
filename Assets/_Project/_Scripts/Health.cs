using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Platformer
{
    public class Health : MonoBehaviour,IDamageable
    {
        public UnityAction<float> OnHealthChanged = delegate { };
        [SerializeField] int maxHealth = 100;
        

        int currentHealth;
        AudioSource audioSource;

        private void Awake()
        {
            currentHealth = maxHealth;
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            UpdateHealthBar();
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        public void TakeDamage(int damage)
        {
            if(audioSource != null)
                audioSource.Play();
            currentHealth -= damage;
            UpdateHealthBar();

            if (currentHealth <= 0)
            {
                Die();
            }
            Debug.Log($"{this.gameObject.name} is taking damage of {damage}");
        }

        private void UpdateHealthBar()
        {
            float healthPercentage = (float)currentHealth / maxHealth;
            OnHealthChanged.Invoke(healthPercentage);
        }
    }
}
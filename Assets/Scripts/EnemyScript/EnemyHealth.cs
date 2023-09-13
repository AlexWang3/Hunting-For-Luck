using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private bool damageable = true;
        [SerializeField] private int healthAmount = 100;
        [SerializeField] private float invulnerabilityTime = .2f;
        public bool giveUpwardForce = true;
        private bool hit;
        private int currentHealth;

        private void Start()
        {
            currentHealth = healthAmount;
        }

        public void Damage(int amount)
        {
            if (damageable && !hit && currentHealth > 0)
            {
                hit = true;
                currentHealth -= amount;
                if (currentHealth <= 0) 
                {
                    currentHealth = 0;
                    gameObject.SetActive(false);
                }
                else
                {
                    StartCoroutine(TurnOffHit());
                }
            }
        }

        public IEnumerator TurnOffHit()
        {
            yield return new WaitForSeconds(invulnerabilityTime);
            hit = false;
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class LegnaHealth : MonoBehaviour
    {
        public int maxHealthPoints = 100;
        [HideInInspector] public int healthPoints;
        [HideInInspector] public bool giveUpwardForce;
        [HideInInspector] public bool left;
        [HideInInspector] public bool hit;
        
        private void Start()
        {
            Initialization();
        }
        
        private void Initialization()
        {
            healthPoints = maxHealthPoints;
            giveUpwardForce = true;
        }

        public virtual void DealDamage(int amount)
        {
            if (healthPoints > 0)
            {
                healthPoints -= amount;
                hit = true;
            }
            if (healthPoints <= 0 )
            {
                healthPoints = 0;
                Invoke("Death", 5);
            }
        }

        private void Death()
        {
            gameObject.SetActive(false);
        }
    }    
}


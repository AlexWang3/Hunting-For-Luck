using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class EnemyHealth : Health
    {
        public bool giveUpwardForce = true;

        public override void DealDamage(int amount)
        {
            base.DealDamage(amount);
            if (healthPoints <= 0 )// && gameObject.GetComponent<EnemyCharacter>())
            {
                healthPoints = 0;
                gameObject.SetActive(false);
                Invoke("Revive", 5);
            }
        }
        
        //This revives the enemy quickly so you can test out certain features when building game; this method probably shouldn't exist in real game
        protected virtual void Revive()
        {
            gameObject.GetComponent<Health>().healthPoints += 100;
            gameObject.SetActive(true);
        }

    }

}
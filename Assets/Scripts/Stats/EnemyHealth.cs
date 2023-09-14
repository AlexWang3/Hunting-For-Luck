using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class EnemyHealth : Health
    {
        public bool giveUpwardForce = true;

        //This method handles logic specific to dealing damage to an Enemy; this could also be used on a wall that the Player can destroy
        public override void DealDamage(int amount)
        {
            base.DealDamage(amount);
            //This handles what should happen if health is less than zero for an Enemy
            if (healthPoints <= 0 )// && gameObject.GetComponent<EnemyCharacter>())
            {
                healthPoints = 0;
                //Disables object from scene when killed
                gameObject.SetActive(false);
                //This is a testing method, probably shouldn't exist in real game
                Invoke("Revive", 1);
            }
        }

        //This revives the enemy quickly so you can test out certain features when building game; this method probably shouldn't exist in real game
        protected virtual void Revive()
        {
            gameObject.GetComponent<Health>().healthPoints = 100;
            gameObject.SetActive(true);
        }

    }

}
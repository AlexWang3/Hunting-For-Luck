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
            }
        }

    }

}
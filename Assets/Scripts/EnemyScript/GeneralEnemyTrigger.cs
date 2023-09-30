using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class GeneralEnemyTrigger : Managers
    {
        public int damageAmount = 20;
        public bool teleportAfterHit;
        public Transform teleportPosition = null;
        public bool hasDirection;
        public float verticalDamageForce;
        public float horizontalDamageForce;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth)
            {
                if (hasDirection)
                {
                    if (gameObject.transform.position.x < player.transform.position.x)
                    {
                        playerHealth.left = false;
                    }
                    else
                    {
                        playerHealth.left = true;
                    }
                }
                else
                {
                    playerHealth.left = !character.isFacingLeft;
                }
                if (teleportAfterHit)
                {
                    playerHealth.teleportAfterHit = true;
                    playerHealth.teleportPosition = teleportPosition;
                }
                else
                {
                    playerHealth.teleportAfterHit = false;
                }
                playerHealth.verticalDamageForce = verticalDamageForce;
                playerHealth.horizontalDamageForce = horizontalDamageForce;
                playerHealth.DealDamage(damageAmount);
                if (!teleportAfterHit)
                {
                    gameObject.GetComponent<Collider2D>().enabled = false;
                }
            }
        }
    }
}

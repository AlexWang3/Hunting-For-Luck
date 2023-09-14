using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class MeleeWeapon : MonoBehaviour
    {
        [SerializeField] private int damageAmount = 20;
        private Character character;
        private Rigidbody2D rb;
        private MeleeAttackManager meleeAttackManager;
        private Vector2 direction;
        private bool collided;
        private bool downwardStrike;

        private void Start()
        {
            character = GetComponentInParent<Character>();
            rb = GetComponentInParent<Rigidbody2D>();
            meleeAttackManager = GetComponentInParent<MeleeAttackManager>();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.GetComponent<EnemyHealth>())
            {
                HandleCollision(collision.GetComponent<EnemyHealth>());
            }
        }

        private void HandleCollision(EnemyHealth objHealth)
        {
            if(objHealth.giveUpwardForce && character.input.DownHeld() && !character.isGrounded)
            {
                direction = Vector2.up;
                downwardStrike = true;
                collided = true;
            }
            if(character.input.UpHeld() && !character.isGrounded)
            {
                direction = Vector2.down;
                collided = true;
            }
            if ((!character.input.UpHeld() && character.isGrounded) || 
                (!character.input.UpHeld() && !character.input.DownHeld()))
            {
                if (character.isFacingLeft)
                {
                    direction = Vector2.right;
                }
                else
                {
                    direction = Vector2.left;
                }
                collided = true;
            }
            objHealth.DealDamage(damageAmount);
            StartCoroutine(NoLongerColliding());
        }

        private void HandleMovement()
        {
            if (collided)
            {
                if (downwardStrike)
                {
                    rb.AddForce(direction * meleeAttackManager.upwardsForce);
                }
                else
                {
                    rb.AddForce(direction * meleeAttackManager.defaultForce);
                }
            }
        }

        private IEnumerator NoLongerColliding()
        {
            yield return new WaitForSeconds(meleeAttackManager.movementTime);
            collided = false;
            downwardStrike = false;
        }
    }

}

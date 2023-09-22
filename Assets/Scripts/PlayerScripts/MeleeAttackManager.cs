using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class MeleeAttackManager : Abilities
    {
        public float defaultForce = 300;
        public float upwardsForce = 600;
        public float movementTime = .1f;
        public float movementForce = 5000;


        private bool keepAttacking;
        private bool takeNextInput;
        private Animator meleeAnimator;
        private int triggerInfo; // 0 for reset, 1 for forward, 2 for downward
        private bool triggerMovement;

        protected override void Initialization()
        {
            base.Initialization();
            meleeAnimator = GetComponentInChildren<MeleeWeapon>().gameObject.GetComponent<Animator>();
            character.isMeleeAttacking = false;
            keepAttacking = false;
            takeNextInput = false;
            triggerInfo = 0;
            triggerMovement = false;
        }
        
        public void MeleeAttack() {
            if (!character.isShooting)
            {
                if (!character.isMeleeAttacking)
                {
                    PerformMeleeAttack();
                }
                else if (takeNextInput)
                {
                    keepAttacking = true;
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if (triggerMovement)
            {
                triggerMovement = false;
                rb.velocity = Vector2.zero;
                if (triggerInfo == 1)
                {
                    if (character.isFacingLeft)
                    {
                        rb.AddForce(Vector2.left * movementForce);
                    }
                    else
                    {
                        rb.AddForce(Vector2.right * movementForce);
                    }
                }
            }
        }


        private void PerformMeleeAttack()
        {
            character.isMeleeAttacking = true;
            takeNextInput = false;
            anim.SetBool("MeleeAttack", true);
            if (character.isJumping)
            {
                character.isJumping = false;
            }

            /*
            if (input.UpHeld())
            {
                anim.SetTrigger("UpwardMelee");
                meleeAnimator.SetTrigger("UpwardMeleeSwipe");
            }
            */

            if (input.DownHeld() && !character.isGrounded)
            {
                anim.SetTrigger("DownwardMelee");
                
                triggerInfo = 2;
            }

            if (!input.UpHeld() && !input.DownHeld())
            {
                anim.SetTrigger("ForwardMelee");
                triggerInfo = 1;
            }

            if (input.DownHeld() && character.isGrounded)
            {
                anim.SetTrigger("ForwardMelee");
                triggerInfo = 1;
            }
        }

        public void FinishMeleeAttack()
        {
            if (!keepAttacking)
            {
                character.isMeleeAttacking = false;
                anim.SetBool("MeleeAttack", false);
            }
            else
            {
                keepAttacking = false;
                PerformMeleeAttack();
            }
        }

        public void TriggerMeleeWeapon()
        {
            takeNextInput = true;
            triggerMovement = true;
            switch (triggerInfo)
            {
                case 1:
                    meleeAnimator.SetTrigger("ForwardMeleeSwipe");
                    break;
                case 2:
                    meleeAnimator.SetTrigger("DownwardMeleeSwipe");
                    break;
                default:
                    meleeAnimator.SetTrigger("ForwardMeleeSwipe");
                    Debug.Log("Unexpected Trigger Info Used for MeleeAttack");
                    break;
            }
        }
    }
}

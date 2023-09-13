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
        
        private bool keepAttacking;
        private bool takeNextInput;
        private Animator meleeAnimator;
        // 0 for reset, 1 for forward, 2 for downward
        private int triggerInfo;

        protected override void Initialization()
        {
            base.Initialization();
            meleeAnimator = GetComponentInChildren<MeleeWeapon>().gameObject.GetComponent<Animator>();
            character.isMeleeAttacking = false;
            keepAttacking = false;
            takeNextInput = false;
            triggerInfo = 0;
        }

        protected virtual void Update()
        {
            if (input.AttackPressed() && !character.isShooting)
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

        private void PerformMeleeAttack()
        {
            rb.velocity = new Vector2(0, 0);
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

        private void FinishMeleeAttack()
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

        private void TriggerMeleeWeapon()
        {
            takeNextInput = true;
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

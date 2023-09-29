using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public enum MeleeAttackType
    {
        NULL,
        DOWN,
        AIR,
        STAB1,
        STAB2,
        STAB3
    }
    
    public class MeleeAttackManager : Abilities
    {
        public float defaultForce = 300;
        public float upwardsForce = 600;
        public float movementTime = .1f;
        public float movementForce = 5000;
        public float postMoveInputInterval = .2f;


        private bool keepAttacking;
        private bool takeNextInput;
        private Animator meleeAnimator;
        private MeleeAttackType triggerInfo;
        private bool triggerMovement;
        private int curForwardSequence;
        private float postMoveInputCountDown;

        protected override void Initialization()
        {
            base.Initialization();
            meleeAnimator = GetComponentInChildren<MeleeWeapon>().gameObject.GetComponent<Animator>();
            character.isMeleeAttacking = false;
            keepAttacking = false;
            takeNextInput = false;
            triggerInfo = MeleeAttackType.NULL;
            triggerMovement = false;
            curForwardSequence = 0;
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
            HandlePhysicMovement();
            HandleForwardMeleePostMoveInput();
        }

        private void HandleForwardMeleePostMoveInput()
        {
            if (postMoveInputCountDown <= 0)
            {
                postMoveInputCountDown = 0;
                curForwardSequence = 0;
            }
            else
            {
                postMoveInputCountDown -= Time.deltaTime;
            }
                
        }
        
        private void HandlePhysicMovement()
        {
            if (triggerMovement)
            {
                triggerMovement = false;
                rb.velocity = Vector2.zero;
                if (triggerInfo == MeleeAttackType.STAB1)
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
            // anim.SetBool("MeleeAttack", true);
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
                triggerInfo = MeleeAttackType.DOWN;
            }
            else if (!character.isGrounded)
            {
                anim.SetTrigger(("AirStab"));
                triggerInfo = MeleeAttackType.STAB1;
            }
            else if (character.isGrounded)
            {
                SetForwardMelee();
            }
        }

        private void SetForwardMelee()
        {
            switch (curForwardSequence)
            {
                case 0:
                    anim.SetTrigger("Stab1");
                    triggerInfo = MeleeAttackType.STAB1;
                    curForwardSequence++;
                    postMoveInputCountDown = postMoveInputInterval;
                    break;
                case 1:
                    anim.SetTrigger("Stab2");
                    triggerInfo = MeleeAttackType.STAB1;
                    curForwardSequence++;
                    postMoveInputCountDown = postMoveInputInterval;
                    break;
                case 2:
                    anim.SetTrigger("Stab3");
                    triggerInfo = MeleeAttackType.STAB1;
                    curForwardSequence = 0;
                    break;
                default:
                    Debug.Log("Unexpected Forward Melee Sequence");
                    break;
            }
        }
        
        public void FinishMeleeAttack()
        {
            if (!keepAttacking)
            {
                character.isMeleeAttacking = false;
                // anim.SetBool("MeleeAttack", false);
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
                case MeleeAttackType.STAB1:
                    meleeAnimator.SetTrigger("ForwardMeleeSwipe");
                    break;
                case MeleeAttackType.DOWN:
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

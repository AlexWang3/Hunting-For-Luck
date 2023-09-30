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
        public float postMoveInputInterval = .75f;
        public float maxHoldTime = 2f;


        private bool keepAttacking;
        private bool takeNextInput;
        private Animator meleeAnimator;
        private MeleeAttackType triggerInfo;
        private bool triggerMovement;
        private int curForwardSequence;
        private float postMoveInputCountDown;
        private bool isForward4;
        private bool isForward4Hold;
        private bool isForward4Stabbing;
        private float holdCountDown;
        private bool triggerSet;

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
            triggerSet = false;
        }
        
        // Called By Input
        public void MeleeAttack() {
            if (!character.isShooting && !character.isDashing && !character.isGettingHit && !character.isDead)
            {
                if (!character.isMeleeAttacking)
                {
                    PerformMeleeAttack();
                }
                else if (takeNextInput)
                {
                    PreInputRoutine();
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            HandlePhysicMovement();
            HandleForwardMeleePostMoveInput();
            HandleForwardMeleeHold();
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

        private void HandleForwardMeleeHold()
        {
            if (isForward4Hold)
            {
                if (!input.MeleeHeld() || holdCountDown <= 0)
                {
                    anim.SetTrigger("Stab4Release");
                    isForward4Stabbing = true;
                    isForward4Hold = false;
                }
                else if (input.MeleeHeld())
                {
                    holdCountDown -= Time.deltaTime;
                }
            }
        }
        
        private void PreInputRoutine()
        {
            if (isForward4Hold || isForward4Stabbing)
            {
                return;
            }
            keepAttacking = true;
            triggerSet = true;
            SetAnimationTriggers();
        }
        
        private void PerformMeleeAttack()
        {
            character.isMeleeAttacking = true;
            takeNextInput = false;
            if (character.isJumping)
            {
                character.isJumping = false;
            }

            if (triggerSet)
                triggerSet = false;
            else
            {
                SetAnimationTriggers();
            }
            if (isForward4)
            {
                isForward4 = false;
                holdCountDown = maxHoldTime;
                isForward4Hold = true;
            }
            else
            {
                postMoveInputCountDown = postMoveInputInterval;
            }
        }

        private void SetAnimationTriggers()
        {
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
                    break;
                case 1:
                    anim.SetTrigger("Stab2");
                    triggerInfo = MeleeAttackType.STAB1;
                    curForwardSequence++;
                    break;
                case 2:
                    anim.SetTrigger("Stab3");
                    triggerInfo = MeleeAttackType.STAB1;
                    curForwardSequence++;
                    break;
                case 3:
                    anim.SetTrigger("Stab4Hold");
                    triggerInfo = MeleeAttackType.STAB1;
                    isForward4 = true;
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
            }
            else
            {
                keepAttacking = false;
                PerformMeleeAttack();
            }

            isForward4Stabbing = false;
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

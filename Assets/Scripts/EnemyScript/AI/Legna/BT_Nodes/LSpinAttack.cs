using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace MetroidvaniaTools
{
    public class LSpinAttack : Node
    {
        // Passing in
        private LegnaCharacter character;
        private float timeTillMaxSpeed;
        private float maxSpeed;
        private float maxSpinTime;
        
        // Local
        private int moveIndex = 0;
        private float spinTimeCountDown;
        
        public LSpinAttack(LegnaCharacter character, float timeTillMaxSpeed, float maxSpeed, float maxSpinTime)
            => (this.character, this.timeTillMaxSpeed, this.maxSpeed, this.maxSpinTime) =
                (character, timeTillMaxSpeed, maxSpeed, maxSpinTime);
        
        public override NodeState Evaluate()
        {
            if (character.curState != LegnaStates.SPINATTACK)
            {
                character.curState = LegnaStates.SPINATTACK;
                character.GeneralIdle();
                character.anim.SetTrigger("SpinAttackStart");
                character.FacingPlayer();
                moveIndex = 1;
                character.spinAttackHitBox.GetComponent<GeneralEnemyTrigger>().alreadyHit = false;
                character.SA_dashTrigger = false;
                character.SA_finishTrigger = false;
            }
            
            state = NodeState.RUNNING;
            if (moveIndex == 1)
            {
                if (character.SA_dashTrigger)
                {
                    character.SA_dashTrigger = false;
                    character.FacingPlayer();
                    spinTimeCountDown = maxSpinTime;
                    Physics2D.IgnoreCollision(character.col, character.playerCollider, true);
                    character.spinAttackHitBox.GetComponent<Animator>().SetBool("SA", true);
                    moveIndex = 2;
                }
            }
            else if (moveIndex == 2)
            {
                if (spinTimeCountDown <= 0 || character.spinAttackHitBox.GetComponent<GeneralEnemyTrigger>().alreadyHit)
                {
                    moveIndex = 3;
                    character.GeneralIdle();
                    character.spinAttackHitBox.GetComponent<Animator>().SetBool("SA", false);
                    Physics2D.IgnoreCollision(character.col, character.playerCollider, false);
                    character.anim.SetTrigger("SpinAttackFinish");
                }
                else
                {
                    character.GeneralMovement(timeTillMaxSpeed, maxSpeed);
                    spinTimeCountDown -= Time.deltaTime;
                }
            }
            else if (moveIndex == 3)
            {
                if (character.SA_finishTrigger)
                {
                    character.SA_finishTrigger = false;
                    character.curState = LegnaStates.NULL;
                    state = NodeState.SUCCESS;
                }
            }
            
            return state;
        }
    }   
}

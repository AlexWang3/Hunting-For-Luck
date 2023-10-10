using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace MetroidvaniaTools
{
    public class LCalibur : Node
    {
        // Pass In
        private LegnaCharacter character;
        private float maxChargeTime;
        private float minDistance;
        
        // Local
        private int moveIndex = 0;
        private float chargeCountDown;
        public LCalibur(LegnaCharacter character, float maxChargeTime, float minDistance)
            => (this.character, this.maxChargeTime, this.minDistance) =
                (character, maxChargeTime, minDistance);

        public override NodeState Evaluate()
        {
            if (character.curState != LegnaStates.EXCALIBUR)
            {
                // Enter move
                character.curState = LegnaStates.EXCALIBUR;
                character.FacingPlayer();
                character.GeneralIdle();
                character.NA_finishTrigger = false;
                chargeCountDown = maxChargeTime;
                character.anim.SetTrigger("CaliburStart");
                moveIndex = 1;
            }

            state = NodeState.RUNNING;

            if (moveIndex == 1)
            {
                if (character.NA_finishTrigger)
                {
                    character.NA_finishTrigger = false;
                    character.FacingPlayer();
                    moveIndex = 2;
                }
            }
            else if (moveIndex == 2)
            {
                if (chargeCountDown <= 0 || character.GetPlayerDistance() < minDistance)
                {
                    character.anim.SetTrigger("CaliburEnd");
                    moveIndex = 3;
                }
                else
                {
                    chargeCountDown -= Time.deltaTime;
                }
            }
            else if (moveIndex == 3)
            {
                if (character.NA_finishTrigger)
                {
                    character.NA_finishTrigger = false;
                    character.curState = LegnaStates.NULL;
                    state = NodeState.SUCCESS;
                }
            }

            return state;
        }
    }   
}

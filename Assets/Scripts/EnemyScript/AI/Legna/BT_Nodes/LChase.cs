using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace MetroidvaniaTools
{
    public class LChase : Node
    {
        // Passing in
        private LegnaCharacter character;
        private float timeTillMaxSpeed;
        private float maxSpeed;
        
        public LChase(LegnaCharacter character, float timeTillMaxSpeed, float maxSpeed)
            => (this.character, this.timeTillMaxSpeed, this.maxSpeed) =
                (character, timeTillMaxSpeed, maxSpeed);
        
        public override NodeState Evaluate()
        {
            if (character.playerDistanceClass < 3)
            {
                state = NodeState.SUCCESS;
                return state;
            }
            
            if (character.curState != LegnaStates.CHASE)
            {
                character.curState = LegnaStates.CHASE;
                character.anim.SetBool("Running", true);
            }
            character.FacingPlayer();
            character.GeneralMovement(timeTillMaxSpeed, maxSpeed);
            
            state = NodeState.RUNNING;
            return state;
        }
    }   
}

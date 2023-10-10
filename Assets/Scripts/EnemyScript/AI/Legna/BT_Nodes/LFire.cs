using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace MetroidvaniaTools
{
    public class LFire : Node
    {
        // Passing in
        private LegnaCharacter character;
        
        // Local
        private float dir;
        
        public LFire(LegnaCharacter character)
            => (this.character) =
                (character);
        
        public override NodeState Evaluate()
        {
            if (character.curState != LegnaStates.FIRE)
            {
                character.curState = LegnaStates.FIRE;
                character.FacingPlayer();
                character.GeneralIdle();
                character.Dodge_finishTrigger = false;
                character.anim.SetTrigger("Fire");
            }
            state = NodeState.RUNNING;
            if (character.Dodge_finishTrigger)
            {
                character.GeneralIdle();
                character.curState = LegnaStates.NULL;
                state = NodeState.SUCCESS;
            }
            
            return state;
        }
    }   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace MetroidvaniaTools
{
    public class LPhaseChange : Node
    {
        // Passing in
        private LegnaCharacter character;
        
        public LPhaseChange(LegnaCharacter character)
            => (this.character) =
                (character);
        
        public override NodeState Evaluate()
        {
            character.HandleHit();
            character.isStagger = false;
            if (character.curState != LegnaStates.PHASE_CHANGING)
            {
                character.curState = LegnaStates.PHASE_CHANGING;
                character.FacingPlayer();
                character.GeneralIdle();
                character.anim.SetTrigger("PhaseChange");
            }
            
            state = NodeState.RUNNING;
            if (character.PC_endTrggier)
            {
                character.PC_endTrggier = false;
                character.anim.SetTrigger("PhaseChangeEnd");
                state = NodeState.SUCCESS;
            }
            return state;
        }
    }   
}

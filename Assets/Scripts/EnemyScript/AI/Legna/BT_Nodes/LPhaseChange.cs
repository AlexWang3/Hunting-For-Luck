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

        private int moveIndex;
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
                moveIndex = 1;
                character.anim.SetTrigger("PhaseChange");
            }
            
            state = NodeState.RUNNING;
            if (moveIndex == 1)
            {
                if (character.PC_prepareEndTrigger)
                {
                    character.transform.position = character.centerRef.transform.position;
                    character.FacingPlayer();
                    moveIndex = 2;
                }
            }
            else if (moveIndex == 2)
            {
                if (character.PC_endTrggier)
                {
                    AudioManager.Instance.PlayBGM("BossSecond");
                    character.PC_endTrggier = false;
                    character.toughness = character.p2_MaxToughness;
                    character.anim.SetBool("P1", false);
                    character.anim.SetTrigger("PhaseChangeEnd");
                    character.curState = LegnaStates.NULL;
                    state = NodeState.SUCCESS;
                }   
            }
            return state;
        }
    }   
}

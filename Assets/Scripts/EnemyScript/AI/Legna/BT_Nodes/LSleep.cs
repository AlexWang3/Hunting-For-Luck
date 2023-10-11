using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace MetroidvaniaTools
{
    public class LSleep : Node
    {
        // Passing in
        private LegnaCharacter character;


        private int moveIndex;
        public LSleep(LegnaCharacter character)
            => (this.character) =
                (character);
        
        public override NodeState Evaluate()
        {
            if (character.curState != LegnaStates.SLEEPING)
            {
                character.curState = LegnaStates.SLEEPING;
                character.FacingPlayer();
                character.GeneralIdle();
                character.col.enabled = false;
                character.rb.simulated = false;
                character.AW_endTrigger = false;
                moveIndex = 1;
            }
            state = NodeState.RUNNING;
            if (moveIndex == 1)
            {
                if (character.player.transform.position.x > character.startRef.transform.position.x)
                {
                    character.anim.SetTrigger("SleepEnd");
                    character.anim.SetBool("P1", true);
                    character.col.enabled = true;
                    character.rb.simulated = true;
                    moveIndex = 2;
                }
            }
            if (moveIndex == 2)
            {
                if (character.AW_endTrigger)
                {
                    AudioManager.Instance.PlayBGM("BossFirst");
                    character.AW_endTrigger = false;
                    character.curState = LegnaStates.NULL;
                    state = NodeState.SUCCESS;
                }
            }
            return state;
        }
    }    
}


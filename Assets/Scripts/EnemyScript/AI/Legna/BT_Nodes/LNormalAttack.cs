using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace MetroidvaniaTools
{
    public class LNormalAttack : Node
    {
        private LegnaCharacter character;
        public LNormalAttack(LegnaCharacter character)
            => (this.character) =
                (character);

        public override NodeState Evaluate()
        {
            if (character.curState != LegnaStates.NORMALATTACK)
            {
                // Enter move
                character.curState = LegnaStates.NORMALATTACK;
                character.FacingPlayer();
                character.GeneralIdle();
                character.NA_finishTrigger = false;
                character.anim.SetTrigger("NormalAttackStart");
            }

            state = NodeState.RUNNING;
            if (character.NA_finishTrigger)
            {
                character.NA_finishTrigger = false;
                character.curState = LegnaStates.NULL;
                state = NodeState.SUCCESS;
            }

            return state;
        }
    }   
}

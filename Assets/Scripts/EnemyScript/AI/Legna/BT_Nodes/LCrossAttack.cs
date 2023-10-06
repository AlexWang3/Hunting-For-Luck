using UnityEngine;
using BehaviorTree;

namespace MetroidvaniaTools
{
    public class LCrossAttack : Node
    {
        private LegnaCharacter character;
        private int moveIndex = 0;
        public LCrossAttack(LegnaCharacter character)
            => (this.character) =
                (character);

        public override NodeState Evaluate()
        {
            if (character.curState != LegnaStates.CROSSATTACK)
            {
                // Enter move
                character.curState = LegnaStates.CROSSATTACK;
                character.FacingPlayer();
                character.GeneralIdle();
                character.NA_finishTrigger = false;
                character.anim.SetTrigger("CrossAttackStart");
                moveIndex = 1;
            }

            state = NodeState.RUNNING;
            if (character.NA_finishTrigger)
            {
                character.NA_finishTrigger = false;

                int rand = Random.Range(0, 2);
                if (moveIndex == 2 || character.playerDistanceClass > 2 || rand != 0)
                {
                    character.curState = LegnaStates.NULL;
                    character.anim.SetTrigger("CrossAttackFinish");
                    moveIndex = 0;
                    state = NodeState.SUCCESS;
                }
                else
                {
                    moveIndex++;
                    character.FacingPlayer();
                    character.GeneralIdle();
                    if (moveIndex == 2)
                        character.anim.SetTrigger("CrossAttack2");
                }
            }

            return state;
        }
    }
}

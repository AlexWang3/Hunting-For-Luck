using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace MetroidvaniaTools
{
    public class LIdle : Node
    {
        private LegnaCharacter character;
        private float idleTime;

        private float countDown;
        public LIdle(LegnaCharacter character, float idleTime)
            => (this.character, this.idleTime) =
                (character, idleTime);
        
        public override NodeState Evaluate()
        {
            if (character.curState != LegnaStates.IDLE)
            {
                character.curState = LegnaStates.IDLE;
                countDown = idleTime;
                character.GeneralIdle();
            }
            state = NodeState.RUNNING;
            if (countDown <= 0)
                state = NodeState.SUCCESS;
            else
                countDown -= Time.deltaTime;
            return state;
        }
    }   
}

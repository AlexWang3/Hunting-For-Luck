using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace MetroidvaniaTools
{
    public class Idle : Node
    {
        private NormalEnemyCharacter enemyCharacter;

        public Idle(NormalEnemyCharacter enemyCharacter) => 
            (this.enemyCharacter) =
            (enemyCharacter);
        
        public override NodeState Evaluate()
        {
            if (enemyCharacter.isFighting)
            {
                enemyCharacter.isFighting = false;
            }
            if (enemyCharacter.idleCountDown <= 0)
            {
                enemyCharacter.idleCountDown = enemyCharacter.unitIdleTime;
                state = NodeState.SUCCESS;
                return state;
            }
            enemyCharacter.rb.velocity = new Vector2(0, 0);
            enemyCharacter.idleCountDown -= Time.deltaTime;
            state = NodeState.RUNNING;
            return state;
        }
    }   
}

using UnityEngine;
using BehaviorTree;

namespace MetroidvaniaTools
{
    
    public class Patrol : Node
    {
        // Passing in
        private NormalEnemyCharacter enemyCharacter;
        private bool turnAroundOnCollision;
        private LayerMask collidersToTurnAroundOn;
        private float timeTillMaxSpeed;
        private float maxSpeed;
        private float unitPatrolTime;
        
        // Local
        // private Unity.Mathematics.Random rnd = new Unity.Mathematics.Random();

        public Patrol(NormalEnemyCharacter enemyCharacter, bool turnAroundOnCollision,
            LayerMask collidersToTurnAroundOn, float timeTillMaxSpeed, float maxSpeed)
            => (this.enemyCharacter, this.turnAroundOnCollision, this.collidersToTurnAroundOn
                    , this.timeTillMaxSpeed, this.maxSpeed) =
                (enemyCharacter, turnAroundOnCollision, collidersToTurnAroundOn, timeTillMaxSpeed, maxSpeed);
        
        
        public override NodeState Evaluate()
        {
            if (enemyCharacter.isFighting)
            {
                enemyCharacter.isFighting = false;
                state = NodeState.FAILURE;
                return state;
            }
            
            if (enemyCharacter.patrolCountDown <= 0)
            {
                enemyCharacter.patrolCountDown = enemyCharacter.unitPatrolTime;
                state = NodeState.SUCCESS;
                return state;
            }

            if (enemyCharacter.patrolCountDown == enemyCharacter.unitPatrolTime)
            {
                RandomFlip();
            }
            
            CheckForTurnAround();
            enemyCharacter.GeneralMovement(timeTillMaxSpeed, maxSpeed);
            enemyCharacter.patrolCountDown -= Time.deltaTime;
            state = NodeState.RUNNING;
            return state;
        }

        private void CheckForTurnAround()
        {
            if (!turnAroundOnCollision)
                return;
            if (!enemyCharacter.facingLeft)
            {
                if (enemyCharacter.CollisionCheck(Vector2.right, .5f, collidersToTurnAroundOn))
                {
                    enemyCharacter.Flip();
                }
            }
            else
            {
                if (enemyCharacter.CollisionCheck(Vector2.left, .5f, collidersToTurnAroundOn))
                {
                    enemyCharacter.Flip();
                }
            }
        }

        private void RandomFlip()
        {
            if (Random.Range(0, 2) == 0)
                enemyCharacter.Flip();
        }
        
    }   
}

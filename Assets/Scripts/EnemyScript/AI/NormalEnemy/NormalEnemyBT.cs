using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.PlayerLoop;

namespace MetroidvaniaTools
{
    public class NormalEnemyBT : AbstractBT
    {
        private NormalEnemyCharacter enemyCharacter;
        private EnemyHealth enemyHealth;
        
        [Header("Patrol Parameters")]
        [SerializeField] private bool turnAroundOnCollision;
        [SerializeField] private float P_TimeTillMaxSpeed;
        [SerializeField] private float P_MaxSpeed;
        [SerializeField] private LayerMask collidersToTurnAroundOn;
        [SerializeField] private float minPatrolTime;
        [SerializeField] private float maxPatrolTime;
        [SerializeField] private float minIdleTime;
        [SerializeField] private float maxIdleTime;
        
        [Header("Chasing Parameters")]
        [SerializeField] private float detectRange;
        [SerializeField] private float C_TimeTillMaxSpeed;
        [SerializeField] private float C_MaxSpeed;

        protected override void Initialization()
        {
            enemyCharacter = GetComponent<NormalEnemyCharacter>();
            enemyHealth = GetComponent<EnemyHealth>();
            base.Initialization();
        }

        protected override Node SetupTree()
        {
            Node chase = new Chase(enemyCharacter, detectRange, C_TimeTillMaxSpeed, C_MaxSpeed);
            Node patrol = new Patrol(enemyCharacter, turnAroundOnCollision, collidersToTurnAroundOn,
                P_TimeTillMaxSpeed, P_MaxSpeed, minPatrolTime, maxPatrolTime, minIdleTime, maxIdleTime);
            // Node idle = new Idle(enemyCharacter);

            Node root = new Selector(new List<Node>
            {
                chase,
                patrol
            });
            return root;
        }
    }   
}

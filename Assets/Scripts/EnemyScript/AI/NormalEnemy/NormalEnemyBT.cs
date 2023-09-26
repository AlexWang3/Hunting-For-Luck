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
        [SerializeField] private float unitPatrolTime;
        [SerializeField] private float unitIdleTime;
        
        [Header("Chasing Parameters")]
        [SerializeField] private float detectRange;
        [SerializeField] private float C_TimeTillMaxSpeed;
        [SerializeField] private float C_MaxSpeed;

        protected override void Initialization()
        {
            enemyCharacter = GetComponent<NormalEnemyCharacter>();
            enemyHealth = GetComponent<EnemyHealth>();
            base.Initialization();
            enemyCharacter.unitPatrolTime = unitPatrolTime;
            enemyCharacter.unitIdleTime = unitIdleTime;
            enemyCharacter.patrolCountDown = unitPatrolTime;
            enemyCharacter.idleCountDown = unitIdleTime;
        }

        protected override Node SetupTree()
        {
            Node chase = new Chase(enemyCharacter, detectRange, C_TimeTillMaxSpeed, C_MaxSpeed);
            Node patrol = new Patrol(enemyCharacter, turnAroundOnCollision, collidersToTurnAroundOn,
                P_TimeTillMaxSpeed, P_MaxSpeed);
            Node idle = new Idle(enemyCharacter);

            Node root = new Selector(new List<Node>
            {
                chase,
                new Successor(new RandomSelector( new List<Node>
                {
                    patrol, idle
                }))
            });
            return root;
        }
    }   
}

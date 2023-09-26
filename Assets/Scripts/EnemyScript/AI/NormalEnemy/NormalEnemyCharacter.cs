using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class NormalEnemyCharacter : EnemyCharacter
    {
        [HideInInspector] public bool isFighting;
        [HideInInspector] public float patrolCountDown;
        [HideInInspector] public float idleCountDown;
        [HideInInspector] public float unitPatrolTime;
        [HideInInspector] public float unitIdleTime;
        protected override void Initialization()
        {
            base.Initialization();
            isFighting = true;
        }

        public void resetNotFightingTime()
        {
            patrolCountDown = unitPatrolTime;
            idleCountDown = unitIdleTime;
        }
    }   
}

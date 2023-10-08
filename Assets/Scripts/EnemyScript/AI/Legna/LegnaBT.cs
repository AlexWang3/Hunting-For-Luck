using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class LegnaBT : AbstractBT
    {
        private LegnaCharacter character;
        // private EnemyHealth enemyHealth;
        
        [Header("追击")]
        [SerializeField] private float C_TimeTillMaxSpeed;
        [SerializeField] private float C_MaxSpeed;
        
        [Header("跳劈")]
        [SerializeField] private float JC_SuccessDistance;
        [SerializeField] private float JA_jumpHeight;
        [SerializeField] private float JA_distanceOffset;
        [SerializeField] private float JA_maxDistance;
        
        [Header("龙车")]
        [SerializeField] private float SA_SlowSpinTime;
        [SerializeField] private float SA_SlowMaxSpeed;
        [SerializeField] private float SA_FastSpinTime;
        [SerializeField] private float SA_FastMaxSpeed;

        [Header("闪避")] [SerializeField] private float Dodge_distance;
        
        [Header("平A")] 
        [SerializeField] private float NC_SuccessDistance;
        [SerializeField] private float N2_distance;
        
        [Header("防反")]
        [SerializeField] private float GC_maxGuardTime;
        [SerializeField] private float GC_maxDistance;
        [SerializeField] private float GC_closeDistance;
        [SerializeField] private float GC_dodgeForce;
        protected override void Initialization()
        {
            character = GetComponent<LegnaCharacter>();
            base.Initialization();
        }

        protected override Node SetupTree()
        {
            Node chaseForJump = new LChase(character, C_TimeTillMaxSpeed, C_MaxSpeed, JC_SuccessDistance);
            Node chaseForNormalAttack = new LChase(character, C_TimeTillMaxSpeed, C_MaxSpeed, NC_SuccessDistance);
            Node jumpAttack = new LJumpAttack(character, JA_jumpHeight, JA_distanceOffset, JA_maxDistance);
            Node spinAttack = new LSpinAttack(character, SA_SlowSpinTime, SA_SlowMaxSpeed, SA_FastSpinTime, SA_FastMaxSpeed);
            Node crossAttack = new LCrossAttack(character, N2_distance);
            Node guardCounter = new LGuardCounter(character, GC_maxGuardTime, GC_maxDistance, GC_closeDistance, GC_dodgeForce);
            Node dodge = new LBackToCenter(character, Dodge_distance);
            Node shortIdle = new LIdle(character, .5f);
            Node longIdle = new LIdle(character, 2f);

            Node crossAttackSequence = new Sequence(new List<Node>
            {
                chaseForNormalAttack,
                crossAttack,
                shortIdle
            });
            
            Node chaseAndJumpAttackSequence = new Sequence(new List<Node>
            {
                chaseForJump,
                jumpAttack,
                longIdle
            });;

            Node spinAttackSequence = new Sequence(new List<Node>
            {
                spinAttack,
                longIdle
            });
            
            Node veryShortBehavior = new RandomSelector(new List<Node>
            {
                crossAttackSequence,
                dodge,
                guardCounter
            });
            Node shortBehavior = new RandomSelector(new List<Node>
            {
                crossAttackSequence,
                dodge,
                guardCounter
            });
            Node longBehavior = new RandomSelector(new List<Node>
            {
                chaseAndJumpAttackSequence,
                spinAttackSequence
            });
            Node unSeenBehavior = new DetectPlayer(character, 0, dodge);
            Node veryShortDetector = new DetectPlayer(character, 1, veryShortBehavior);
            Node shortDetector = new DetectPlayer(character, 2, shortBehavior);
            Node longDetector = new DetectPlayer(character, 3, longBehavior);
            
            
            Node root = new Selector(new List<Node>
            {
                unSeenBehavior,
                veryShortDetector,
                shortDetector,
                longDetector
            });
            return root;
        }
    }
}


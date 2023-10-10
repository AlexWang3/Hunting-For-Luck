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

        [Header("闪现大跳")] [SerializeField] private float TA_interval;
        
        protected override void Initialization()
        {
            character = GetComponent<LegnaCharacter>();
            base.Initialization();
        }

        protected override Node SetupTree()
        {
            Node p1_chaseForJump = new LChase(character, C_TimeTillMaxSpeed, C_MaxSpeed, JC_SuccessDistance);
            Node p1_chaseForNormalAttack = new LChase(character, C_TimeTillMaxSpeed, C_MaxSpeed, NC_SuccessDistance);
            Node p1_jumpAttack = new LJumpAttack(character, JA_jumpHeight, JA_distanceOffset, JA_maxDistance);
            Node p1_spinAttack = new LSpinAttack(character, SA_SlowSpinTime, SA_SlowMaxSpeed, SA_FastSpinTime, SA_FastMaxSpeed);
            Node p1_crossAttack = new LCrossAttack(character, N2_distance);
            Node p1_guardCounter = new LGuardCounter(character, GC_maxGuardTime, GC_maxDistance, GC_closeDistance, GC_dodgeForce);
            Node dodge = new LBackToCenter(character, Dodge_distance);
            Node shortIdle = new LIdle(character, .5f);
            Node longIdle = new LIdle(character, 2f);
            Node p2_twinkleAttack = new LTwinkleAttack(character, TA_interval);

            Node phaseChange = new LPhaseChange(character);
            
            // Phase 1 Sequences
            Node p1_crossAttackSequence = new Sequence(new List<Node>
            {
                p1_chaseForNormalAttack,
                p1_crossAttack,
                shortIdle
            });
            
            Node p1_chaseAndJumpAttackSequence = new Sequence(new List<Node>
            {
                p1_chaseForJump,
                p1_jumpAttack,
                longIdle
            });;

            Node p1_spinAttackSequence = new Sequence(new List<Node>
            {
                p1_spinAttack,
                longIdle
            });
            
            // Phase 1 Behaviours
            Node p1_veryShortBehavior = new RandomSelector(new List<Node>
            {
                p1_crossAttackSequence,
                dodge,
                p1_guardCounter
            });
            Node p1_shortBehavior = new RandomSelector(new List<Node>
            {
                p1_crossAttackSequence,
                dodge,
                p1_guardCounter
            });
            Node p1_longBehavior = new RandomSelector(new List<Node>
            {
                p1_chaseAndJumpAttackSequence,
                p1_spinAttackSequence
            });
            Node p1_unSeenBehavior = new DetectPlayer(character, 0, dodge);
            
            // Phase1 Detectors
            Node p1_veryShortDetector = new DetectPlayer(character, 1, p1_veryShortBehavior);
            Node p1_shortDetector = new DetectPlayer(character, 2, p1_shortBehavior);
            Node p1_longDetector = new DetectPlayer(character, 3, p1_longBehavior);
            
            Node phase1Behavior = new Selector(new List<Node>
            {
                //p1_unSeenBehavior,
                //p1_veryShortDetector,
                //p1_shortDetector,
                p1_longDetector
            });

            Node phase1 = new PhaseChecker(character, 1, phase1Behavior);
            
            // Phase2 Sequences
            
            // Phase 2 Behaviours
            Node p2_veryShortBehavior = new RandomSelector(new List<Node>
            {
                dodge
            });
            Node p2_shortBehavior = new RandomSelector(new List<Node>
            {
                dodge
            });
            Node p2_longBehavior = new RandomSelector(new List<Node>
            {
                p2_twinkleAttack
            });
            Node p2_unSeenBehavior = new DetectPlayer(character, 0, dodge);
            
            // Phase1 Detectors
            Node p2_veryShortDetector = new DetectPlayer(character, 1, p2_veryShortBehavior);
            Node p2_shortDetector = new DetectPlayer(character, 2, p2_shortBehavior);
            Node p2_longDetector = new DetectPlayer(character, 3, p2_longBehavior);
            
            Node phase2Behavior = new Selector(new List<Node>
            {
                // p2_unSeenBehavior,
                // p2_veryShortDetector,
                // p2_shortDetector,
                p2_longDetector
            });

            Node phase2 = new PhaseChecker(character, 2, phase2Behavior);


            Node root = new Sequence(new List<Node>
            {
                phase1,
                phaseChange,
                phase2
            });
            return root;
        }
    }
}


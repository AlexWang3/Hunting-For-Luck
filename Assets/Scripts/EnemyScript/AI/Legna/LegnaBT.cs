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
        [SerializeField] private float JA_jumpHeight;
        [SerializeField] private float JA_distanceOffset;
        [SerializeField] private float JA_maxDistance;
        
        [Header("龙车")]
        [SerializeField] private float SA_TimeTillMaxSpeed;
        [SerializeField] private float SA_MaxSpeed;
        [SerializeField] private float SA_MaxSpinTime;

        [Header("闪避")] [SerializeField] private float Dodge_distance;
        
        protected override void Initialization()
        {
            character = GetComponent<LegnaCharacter>();
            base.Initialization();
        }

        protected override Node SetupTree()
        {
            Node chase = new LChase(character, C_TimeTillMaxSpeed, C_MaxSpeed);
            Node jumpAttack = new LJumpAttack(character, JA_jumpHeight, JA_distanceOffset, JA_maxDistance);
            Node spinAttack = new LSpinAttack(character, SA_TimeTillMaxSpeed, SA_MaxSpeed, SA_MaxSpinTime);
            Node crossAttack = new LCrossAttack(character);
            Node dodge = new LBackToCenter(character, Dodge_distance);

            Node crossAttackSequence = new Sequence(new List<Node>
            {
                crossAttack,
                new LIdle(character, .5f)
            });
            
            Node chaseAndJumpAttackSequence = new Sequence(new List<Node>
            {
                chase,
                jumpAttack,
                new LIdle(character, .5f)
            });;

            Node spinAttackSequence = new Sequence(new List<Node>
            {
                spinAttack,
                new LIdle(character, .5f)
            });
            
            Node veryShortBehavior = dodge;
            Node shortBehavior = new RandomSelector(new List<Node>
            {
                crossAttackSequence,
                dodge
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


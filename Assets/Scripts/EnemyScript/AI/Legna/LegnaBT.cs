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
        
        protected override void Initialization()
        {
            character = GetComponent<LegnaCharacter>();
            base.Initialization();
        }

        protected override Node SetupTree()
        {
            Node chase = new LChase(character, C_TimeTillMaxSpeed, C_MaxSpeed);
            Node jumpAttack = new LJumpAttack(character, JA_jumpHeight, JA_distanceOffset, JA_maxDistance);
            Node crossAttack = new LCrossAttack(character);
            Node veryShortBehavior = null;
            Node shortBehavior = new Sequence(new List<Node>
            {
                crossAttack,
                new LIdle(character, .5f)
            });
            Node longBehavior = new Sequence(new List<Node>
            {
                chase,
                jumpAttack,
                new LIdle(character, .5f)
            });;
            // Node unSeenBehavior = null;
            // Node veryShortDetector = new DetectPlayer(character, 1, veryShortBehavior);
            Node shortDetector = new DetectPlayer(character, 2, shortBehavior);
            Node longDetector = new DetectPlayer(character, 3, longBehavior);
            // Node unSeenDetector = new DetectPlayer(character, 0, unSeenBehavior);
            
            
            Node root = new Selector(new List<Node>
            {
                // veryShortDetector,
                shortDetector,
                longDetector
            });
            return root;
        }
    }
}


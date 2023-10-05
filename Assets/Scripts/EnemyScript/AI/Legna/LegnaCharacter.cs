using System.Collections;
using System.Collections.Generic;
using MetroidvaniaTools;
using UnityEngine;
using BehaviorTree;

namespace MetroidvaniaTools
{
    public enum LegnaStates
    {
        NULL,
        CHASE,
        IDLE,
        JUMPATTACK,
        NORMALATTACK
    }
    public class LegnaCharacter : EnemyCharacter
    {
        public float longRangeThreshold;
        public float shortRangeThreshold;
        public GameObject explosions;
        
        
        [HideInInspector] public LegnaStates curState;
        [HideInInspector] public LegnaHealth health;
        [HideInInspector] public bool isGrounded;
        [HideInInspector] public int playerDistanceClass;
        [HideInInspector] public bool canStagger;
        [HideInInspector] public bool isStagger;
        
        // JumpAttack
        [HideInInspector] public bool JA_airTrigger;
        [HideInInspector] public bool JA_finishTrigger;
        
        // NormalAttack
        [HideInInspector] public bool NA_finishTrigger;
        
        private bool groundCheckDisabled;
        protected override void Initialization()
        {
            base.Initialization();
            curState = LegnaStates.NULL;
            health = GetComponent<LegnaHealth>();
            groundCheckDisabled = false;
            canStagger = false;
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            curState = LegnaStates.NULL;
            groundCheckDisabled = false;
        }

        protected virtual void FixedUpdate()
        {
            GroundCheck();
            PlayerCheck();
        }

        private void PlayerCheck()
        {
            if ((CollisionCheck(Vector2.right, shortRangeThreshold, playerLayer))
                || (CollisionCheck(Vector2.left, shortRangeThreshold, playerLayer)))
            {
                playerDistanceClass = 1; // 贴脸了
            }
            else if ((CollisionCheck(Vector2.right, longRangeThreshold, playerLayer))
                     || (CollisionCheck(Vector2.left, longRangeThreshold, playerLayer)))
            {
                playerDistanceClass = 2; // 近距离
            }
            else if ((CollisionCheck(Vector2.right, 9999, playerLayer))
                     || (CollisionCheck(Vector2.left, 9999, playerLayer)))
            {
                playerDistanceClass = 3; // 远距离
            }
            else
            {
                playerDistanceClass = 0; // 没看见
            }
        }
        
        protected virtual void GroundCheck()
        {
            if (CollisionCheck(Vector2.down, .5f, platformLayer) && !groundCheckDisabled)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }

        public void disableGroundCheck(float time)
        {
            groundCheckDisabled = true;
            Invoke("enableGroundCheck", time);
        }

        private void enableGroundCheck()
        {
            groundCheckDisabled = false;
        }

        public override void GeneralIdle()
        {
            base.GeneralIdle();
            anim.SetBool("Running", false);
        }
        
        public bool HandleHit()
        {
            if (isStagger)
                return true;
            
            if (health.hit)
            {
                health.hit = false;
                if (canStagger)
                {
                    isStagger = true;
                    return true;
                }
            }

            return false;
        }

        public void TriggerSeriesDelayExplosion(int amount, float distanceInterval, float minDelayTime,
            float delayTimeIncrement)
        {
            if (facingLeft)
            {
                distanceInterval *= -1;
            }
            
            for (int i = 0; i < amount; i++)
            {
                GameObject currentItem = Instantiate(explosions);
                currentItem.GetComponent<DelayExplosion>().delayTime = minDelayTime + i * delayTimeIncrement;
                currentItem.transform.position = new Vector2(explosions.transform.position.x + i * distanceInterval,
                    explosions.transform.position.y);
                currentItem.SetActive(true);
            }
        }
    }   
}

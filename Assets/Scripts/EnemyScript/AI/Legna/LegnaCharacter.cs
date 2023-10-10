using System.Collections;
using System.Collections.Generic;
using MetroidvaniaTools;
using System;
using UnityEngine;
using BehaviorTree;
using Unity.VisualScripting;

namespace MetroidvaniaTools
{
    public enum LegnaStates
    {
        NULL,
        SLEEPING,
        STUN,
        DEATH,
        PHASE_CHANGING,
        CHASE,
        IDLE,
        JUMPATTACK,
        CROSSATTACK,
        SPINATTACK,
        POS_ADJUST,
        GUARD_COUNTER,
        TWINKLEATTACK,
        FIRE,
        EXCALIBUR,
    }
    public class LegnaCharacter : EnemyCharacter
    {
        public float longRangeThreshold;
        public float shortRangeThreshold;
        public GameObject hitPoint;
        public GameObject explosions;
        public GameObject spinAttackHitBox;
        public Transform centerRef;
        public Transform startRef;
        public GameObject shield;
        public int p1_MaxToughness;
        public int p2_MaxToughness;
        
        
        [HideInInspector] public LegnaStates curState;
        [HideInInspector] public LegnaHealth health;
        [HideInInspector] public bool isGrounded;
        [HideInInspector] public int playerDistanceClass;
        [HideInInspector] public int toughness;
        [HideInInspector] public bool isStagger;
        
        // Awake
        [HideInInspector] public bool AW_endTrigger;
        
        // Stun
        [HideInInspector] public bool ST_endTrigger;
        
        // JumpAttack
        [HideInInspector] public bool JA_airTrigger;
        [HideInInspector] public bool JA_finishTrigger;
        
        // NormalCross/Slash Attack
        [HideInInspector] public bool NA_finishTrigger;
        
        // SpinAttack
        [HideInInspector] public bool SA_dashTrigger;
        [HideInInspector] public bool SA_finishTrigger;
        
        // Dodge
        [HideInInspector] public bool Dodge_startTrigger;
        [HideInInspector] public bool Dodge_finishTrigger;
        
        // Guard Counter
        [HideInInspector] public bool Guard_startTrigger;
        [HideInInspector] public bool Guard_endTrigger;
        [HideInInspector] public bool Counter_startTrigger;
        [HideInInspector] public bool Counter_finishTrigger;
        [HideInInspector] public bool guardCancelTriggered;
        [HideInInspector] public ContactFilter2D shieldFilter;
        private List<Collider2D> shildOverlapResult;
        private Collider2D shieldCollider;
        
        // PhaseChange (Throw Cross)
        [HideInInspector] public bool PC_prepareEndTrigger;
        [HideInInspector] public bool PC_endTrggier;
        
        // TwinkleAttack
        [HideInInspector] public bool TA_prepareEnd;
        [HideInInspector] public bool TA_endTrigger;
        
        // Excalibur
        [HideInInspector] public HorizontalMovement playerHorizontalMovement;
        private bool groundCheckDisabled;

        private MeshRenderer mr;
        private MaterialPropertyBlock mpb_red;
        private MaterialPropertyBlock mpb_white;
        private float tintResetCountDown;
        private bool needResetTint;

        
        protected override void Initialization()
        {
            base.Initialization();
            curState = LegnaStates.NULL;
            health = GetComponent<LegnaHealth>();
            groundCheckDisabled = false;
            toughness = p1_MaxToughness;
            isStagger = false;
            
            mr = GetComponentInChildren<MeshRenderer>();
            mpb_red = new MaterialPropertyBlock();
            mpb_white = new MaterialPropertyBlock();
            mpb_red.SetColor("_Color", Color.red);
            mpb_white.SetColor("_Color", Color.white);
            
            guardCancelTriggered = false;
            shield.SetActive(false);
            shieldFilter = new ContactFilter2D();
            shieldFilter.SetLayerMask(playerLayer);
            shildOverlapResult = new List<Collider2D>();
            shieldCollider = shield.GetComponent<Collider2D>();

            playerHorizontalMovement = player.GetComponent<HorizontalMovement>();
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
            ResetTint();
        }

        public float GetPlayerDistance()
        {
            return Math.Abs(transform.position.x - player.transform.position.x);
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

        public void shieldCheck()
        {
            if (!shield.activeSelf)
                return;
            
            if (shieldCollider.OverlapCollider(shieldFilter, shildOverlapResult) > 0)
            {
                shildOverlapResult[0].transform.position = new Vector2(shield.transform.position.x, shildOverlapResult[0].transform.position.y);
            }
        }
        
        public bool HandleHit()
        {
            if (isStagger)
                return true;
            
            if (health.hit)
            {
                health.hit = false;
                if (health.isGuarding)
                {
                    if (!guardCancelTriggered)
                    {
                        guardCancelTriggered = true; // 等isGuarding变为false时变为false
                        Guard_endTrigger = true; // 触发等待一小段时间cancle
                    }
                    return false;
                }
                else
                {
                    mr.SetPropertyBlock(mpb_red);
                    needResetTint = true;
                    tintResetCountDown = .3f;
                    TriggerHitPointEffect();
                    toughness--;
                }
                
                if (toughness == 0 && health.healthPoints > 0)
                {
                    isStagger = true;
                    return true;
                }
            }

            return false;
        }

        private void ResetTint()
        {
            if (tintResetCountDown > 0)
            {
                tintResetCountDown -= Time.deltaTime;
            }
            else if (needResetTint)
            {
                mr.SetPropertyBlock(mpb_white);
                needResetTint = false;
            }
        }

        public void TriggerSeriesDelayExplosion1D(int amount, float distanceInterval, float minDelayTime,
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
        
        public void TriggerSeriesDelayExplosion2D(int amount, float distanceInterval, float minDelayTime,
            float delayTimeIncrement)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject currentItem = Instantiate(explosions);
                currentItem.GetComponent<DelayExplosion>().delayTime = minDelayTime + i * delayTimeIncrement;
                currentItem.transform.position = new Vector2(explosions.transform.position.x + i * distanceInterval,
                    explosions.transform.position.y);
                currentItem.SetActive(true);
            }

            distanceInterval *= -1;
            
            for (int i = 0; i < amount; i++)
            {
                GameObject currentItem = Instantiate(explosions);
                currentItem.GetComponent<DelayExplosion>().delayTime = minDelayTime + i * delayTimeIncrement;
                currentItem.transform.position = new Vector2(explosions.transform.position.x + i * distanceInterval,
                    explosions.transform.position.y);
                currentItem.SetActive(true);
            }
        }

        public void TriggerSingleDelayExplosion(float delayTime)
        {
            GameObject currentItem = Instantiate(explosions);
            currentItem.GetComponent<DelayExplosion>().delayTime = delayTime;
            currentItem.transform.position = explosions.transform.position;
            currentItem.SetActive(true);
        }

        public void TriggerHitPointEffect()
        {
            GameObject currentItem = Instantiate(hitPoint);
            currentItem.transform.position = hitPoint.transform.position;
            currentItem.SetActive(true);
            currentItem.GetComponent<ParticleSystem>().Play();
        }
    }   
}

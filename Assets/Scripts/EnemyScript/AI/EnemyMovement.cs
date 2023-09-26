using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  MetroidvaniaTools
{
    public class EnemyMovement : AIManager
    {
        [SerializeField] protected bool spawnFacingLeft;
        [SerializeField] protected bool turnAroundOnCollision;
        [SerializeField] protected float timeTillMaxSpeed;
        [SerializeField] protected float maxSpeed;
        [SerializeField] private LayerMask collidersToTurnAroundOn;
        
        private bool spawning = true;
        private float acceleration;
        private float direction;
        private float runTime;

        protected float currentSpeed;

        protected override void Initialization()
        {
            base.Initialization();
            if (spawnFacingLeft)
            {
                enemyCharacter.facingLeft = true;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }

            Invoke("Spawning", .01f);
        }

        protected void FixedUpdate()
        {
            Movement();
        }

        protected virtual void Movement()
        {
            if (!enemyCharacter.facingLeft)
            {
                direction = 1;
                if (CollisionCheck(Vector2.right, .5f, collidersToTurnAroundOn) && turnAroundOnCollision && !spawning)
                {
                    enemyCharacter.facingLeft = true;
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                }
            }
            else
            {
                direction = -1;
                if (CollisionCheck(Vector2.left, .5f, collidersToTurnAroundOn) && turnAroundOnCollision && !spawning)
                {
                    enemyCharacter.facingLeft = false;
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                }
            }
            acceleration = maxSpeed / timeTillMaxSpeed;
            runTime += Time.deltaTime;
            currentSpeed = direction * acceleration * runTime;
            CheckSpeed();
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        }

        protected virtual void CheckSpeed()
        {
            if (currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }

            if (currentSpeed < -maxSpeed)
            {
                currentSpeed = -maxSpeed;
            }
        }

        protected virtual void Spawning()
        {
            spawning = false;
        }
        
    }
}


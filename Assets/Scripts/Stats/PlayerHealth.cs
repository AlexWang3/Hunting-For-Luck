using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MetroidvaniaTools
{
    //Health script specific to Player; it houses a lot of extra data that normally an Enemy wouldn't need
    public class PlayerHealth : Health
    {
        //How long the time value needs to be adjusted to better visualize when the player is hit; this is an effects feature, not needed for actual gameplay
        [SerializeField] protected float slowDownTimeAmount;
        //How much the time value needs to be adjusted to better visualize when the player is hit; this is an effects feature, not needed for actual gameplay
        [SerializeField] protected float slowDownSpeed;
        [SerializeField] protected float forceApplyTime;
        [SerializeField] protected int recoverAmount;
        [SerializeField] protected float recoverTime;

        //A reference to all the different sprites that make up the Player; this is used to make the Player slightly transparent when hit to visualize the Player received damage
        protected SpriteRenderer[] sprites;
        // protected Image deadScreenImage;
        // protected Text deadScreenText;
        protected float originalTimeScale;
        

        [HideInInspector] public bool teleportAfterHit;
        [HideInInspector] public Transform teleportPosition;
        [HideInInspector] public float verticalDamageForce;
        [HideInInspector] public float horizontalDamageForce;

        private bool applyForce;
        private float recoverTimeCountdown;
        private Rigidbody2D rb;

        protected override void Initialization()
        {
            base.Initialization();
            sprites = GetComponentsInChildren<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            // deadScreenImage = uiManager.deadScreen.GetComponent<Image>();
            // deadScreenText = uiManager.deadScreen.GetComponentInChildren<Text>();
        }

        protected virtual void Update()
        {
            character.isGettingHit = hit;
        }

        private void FixedUpdate()
        {
            HandleIFrames();
            HandleDamageMovement();
            HandleRecovery();
            HandleDeath();
        }

        public override void DealDamage(int amount)
        {
            if (!character.isDead)
            {
                if (hit || character.isDashing)
                {
                    return;
                }
                base.DealDamage(amount);
                CharacterGetHit();
                if (healthPoints <= 0)
                {
                    character.isDead = true;
                    healthPoints = 0;
                }
                
                originalTimeScale = Time.timeScale;
                hit = true;
                //Slows down time to the damage speed
                Time.timeScale = slowDownSpeed;
                applyForce = true;
                Invoke("Cancel", iFrameTime);
                Invoke("HitCancel", slowDownTimeAmount);
                Invoke("ForceCancel", forceApplyTime);
            }
        }

        private void CharacterGetHit()
        {
            character.isGettingHit = true;
            character.isJumping = false;
            character.isShooting = false;
            character.isMeleeAttacking = false;
            character.anim.SetBool("GettingHit", true);
            character.input.DisableInput();
        }

        public void HandleDamageMovement()
        {
            if (hit)
            {
                if (applyForce)
                {
                    rb.velocity = Vector2.zero;
                    //Handles vertical and horizontal knockback depending on what direction the Player is facing
                    rb.AddForce(Vector2.up * verticalDamageForce);
                    if (!left)
                    {
                        rb.AddForce(Vector2.right * horizontalDamageForce);
                    }
                    else
                    {
                        rb.AddForce(Vector2.left * horizontalDamageForce);
                    }
                }
                else
                {
                    if (teleportAfterHit)
                    {
                        teleportAfterHit = false;
                        character.rb.velocity = Vector2.zero;
                        character.transform.position = teleportPosition.position;
                    }
                }
            }

        }

        protected virtual void HandleIFrames()
        {
            Color spriteColors = new Color();
            if (hit)
            {
                foreach (SpriteRenderer sprite in sprites)
                {
                    spriteColors = sprite.color;
                    spriteColors.a = .5f;
                    sprite.color = spriteColors;
                }
            }
            else
            {
                foreach (SpriteRenderer sprite in sprites)
                {
                    spriteColors = sprite.color;
                    spriteColors.a = 1;
                    sprite.color = spriteColors;
                }
            }
        }

        protected virtual void HandleRecovery()
        {
            if (character.isDead)
            {
                return;
            }
            if (hit)
            {
                recoverTimeCountdown = recoverTime;
            }
            else
            {
                if (recoverTimeCountdown > 0)
                {
                    recoverTimeCountdown -= Time.deltaTime;
                }
                else
                {
                    recoverTimeCountdown = recoverTime;
                    GainCurrentHealth(recoverAmount);
                }
            }
        }

        protected virtual void HitCancel()
        {
            Time.timeScale = originalTimeScale;
        }

        protected virtual void ForceCancel()
        {
            applyForce = false;
        }

        public virtual void GainCurrentHealth(int amount)
        {
            healthPoints += amount;
            if (healthPoints > maxHealthPoints)
            {
                healthPoints = maxHealthPoints;
            }
        }

        protected override void Cancel()
        {
            if (!character.isDead)
            {
                base.Cancel();
                character.isGettingHit = false;
                character.anim.SetBool("GettingHit", false);
                character.input.EnableInput();
            }
        }

        private void HandleDeath()
        {
            if (character.isDead)
            {
                if (character.isGrounded)
                {
                    character.anim.SetBool("Dying", true);
                }
                StartCoroutine(Dead());
            }
        }

        private IEnumerator Dead()
        {
            yield return new WaitForSeconds(5);
            character.gameObject.SetActive(false);
        }
    }
}
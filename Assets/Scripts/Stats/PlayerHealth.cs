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
            HandleRecovery();
        }

        public override void DealDamage(int amount)
        {
            //If the Player is alive
            if (!character.isDead)
            {
                //If invulnerable or dashing, we return out of this method and deal no damage
                if (hit || character.isDashing)
                {
                    return;
                }
                //If not invulnerable or dashing, then damage is dealt
                base.DealDamage(amount);
                //If health is less than or equal to zero, we manage the Player death state
                if (healthPoints <= 0)
                {
                    character.isDead = true;
                    healthPoints = 0;
                    // player.GetComponent<Animator>().SetBool("Dying", true);
                    // StartCoroutine(Dead());
                }
                //Puts the Player into a damage state, and quickly sets everything up so we can handle all the damage effects
                originalTimeScale = Time.timeScale;
                hit = true;
                //Slows down time to the damage speed
                Time.timeScale = slowDownSpeed;
                applyForce = true;
                // Cancle Iframe
                Invoke("Cancel", iFrameTime);
                //Cancle SlowDown
                Invoke("HitCancel", slowDownTimeAmount);
                // Cancle force Apply
                Invoke("ForceCancel", forceApplyTime);
            }
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

        //Special effect that makes Player transparent when hit
        protected virtual void HandleIFrames()
        {
            Color spriteColors = new Color();
            if (hit)
            {
                character.input.disabled = true;
                foreach (SpriteRenderer sprite in sprites)
                {
                    spriteColors = sprite.color;
                    spriteColors.a = .5f;
                    sprite.color = spriteColors;
                }
            }
            else
            {
                character.input.disabled = false;
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

        //Method that removes player from Damage state
        protected virtual void HitCancel()
        {
            Time.timeScale = originalTimeScale;
        }

        protected virtual void ForceCancel()
        {
            applyForce = false;
        }

        //This method is called when the Player grabs a health item and it restores Player health; it is called from the HealthConsumable script when player enters trigger collider
        public virtual void GainCurrentHealth(int amount)
        {
            healthPoints += amount;
            if (healthPoints > maxHealthPoints)
            {
                healthPoints = maxHealthPoints;
            }
        }
    }
}
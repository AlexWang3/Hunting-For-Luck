using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class LegnaHealth : MonoBehaviour, IDamagebale
    {
        public int maxHealthPoints = 100;
        public int initialLuck;
        public int currentLuck;
        public string enemyName;
         public int healthPoints;
        [HideInInspector] public bool giveUpwardForce;
        [HideInInspector] public bool left;
        [HideInInspector] public bool hit;
        [SerializeField] protected int recoverAmount;
        [SerializeField] protected float recoverTimeAfterGainHealth;
        [SerializeField] protected float recoverTimeAfterHit;
        [SerializeField] protected float recoverInterval;
        public float recoverTimeCountdown;
        private bool gainHealthFromAttack = false;
        private void Start()
        {
            Initialization();
        }
        
        private void Initialization() {
            currentLuck = initialLuck;
            healthPoints = currentLuck;
            giveUpwardForce = true;
        }

        public bool IsGiveUpwardForce() {
            return giveUpwardForce;
        }

        public void TakeDamage(int amount) {
            DealDamage(amount);
            if (G.I.UIMain.currentTarget == this) {
                G.UI.enemyHealthState.enemyHealth = healthPoints;
                G.UI.enemyHealthState.MarkDirty();
            } else {
                UpdateEnemyInformation();
            }
        }

        public virtual void DealDamage(int amount)
        {
            if (healthPoints > 0)
            {
                healthPoints -= amount;
                hit = true;
            }
            if (healthPoints <= 0 )
            {
                healthPoints = 0;
                Invoke("Death", 5);
            }
        }

        public void Update() {
            HandleRecovery();
        }

        protected virtual void HandleRecovery()
        {

            if (gainHealthFromAttack) {
                recoverTimeCountdown = healthPoints < currentLuck? recoverInterval : recoverTimeAfterGainHealth;
                gainHealthFromAttack = false;
            }

            if (hit)
            {
                recoverTimeCountdown = recoverTimeAfterHit;
            }
            else
            {
                if (recoverTimeCountdown > 0)
                {
                    recoverTimeCountdown -= Time.deltaTime;
                }
                else
                {
                    recoverTimeCountdown = recoverInterval;
                    if (healthPoints > currentLuck) {
                        GainCurrentHealth(Mathf.Max(-recoverAmount, currentLuck - healthPoints));
                    } else {
                        GainCurrentHealth(Mathf.Min(recoverAmount, currentLuck - healthPoints));
                    }
                }
            }
        }
        public virtual void GainCurrentHealth(int amount) {
            healthPoints = Mathf.Clamp(healthPoints + amount, 0, maxHealthPoints);
            if (G.I.UIMain.currentTarget == this) {
                G.UI.enemyHealthState.enemyHealth = healthPoints;
                G.UI.enemyHealthState.MarkDirty();
            }
        }
        public void GainHealthFromAttack(int amount) {
            GainCurrentHealth(amount);
            gainHealthFromAttack = true;
        }
        private void Death()
        {
            gameObject.SetActive(false);
        }
    [ContextMenu("Set This As Current Enemy")]
        public void UpdateEnemyInformation() {
            G.I.UIMain.UpdateCurrentEnemy(enemyName, healthPoints, maxHealthPoints, currentLuck, this);
        }
        public void ModifyLuckBarValue(int amount) {
            currentLuck = Mathf.Clamp(currentLuck  + amount, 0, maxHealthPoints);
            if (G.I.UIMain.currentTarget == this) {
                G.UI.enemyHealthState.enemyCurrentLuckValue = currentLuck;
                G.UI.enemyHealthState.MarkDirty();
            }
        }

        public int ReturnHealthPoint() {
            return healthPoints;
        }
    }    
}


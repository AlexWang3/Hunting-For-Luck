using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;
namespace MetroidvaniaTools
{
    public class Weapon : Abilities
    {
        [SerializeField] List<WeaponTypes> weaponTypes;
        [SerializeField] Transform gunBarrel;
        [SerializeField] Transform gunRotation;

        public List<GameObject> currentPool = new List<GameObject>();
        public Solver2D aimingRightHand;
        public Transform aimStartPosition;
        public Transform aimEndPosition;
        public Transform aimTarget;
        public float shootInterval;
        public float disableTime;
        public float recoilAnimTime;
        [HideInInspector] public GameObject currentProjectile;
        private GameObject projectileParentFolder;
        private bool keepShooting;
        private float shootCountDown;
        private float disableCountDown;

        protected override void Initialization()
        {
            base.Initialization();
            foreach(WeaponTypes weapon in weaponTypes) 
            {
                GameObject newPool = new GameObject();
                projectileParentFolder = newPool;
                objectPooler.CreatePool(weapon, currentPool, projectileParentFolder);
            }
            aimingRightHand.enabled = false;
            character.isShooting = false;
            keepShooting = false;
        }

        protected virtual void Update()
        {
            if(input.WeaponFired() && !character.isMeleeAttacking)
            {
                if (!character.isShooting)
                {
                    FireWeapon();
                }
                else 
                {
                    keepShooting = true;
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if (character.isShooting)
            {
                if (shootCountDown <= 0)
                {
                    if (!keepShooting)
                    {
                        character.isShooting = false;
                    }
                    else 
                    {
                        keepShooting = false;
                        FireWeapon();
                    }
                }
                else 
                {
                    shootCountDown -= Time.deltaTime;
                    aimTarget.position = Vector2.Lerp(aimStartPosition.position, aimEndPosition.position, 1 - (shootCountDown / recoilAnimTime));
                }
            }

            if (!character.isShooting && aimingRightHand.enabled) 
            {
                if (disableCountDown <= 0)
                {
                    aimingRightHand.enabled = false;
                }                     
                else
                {
                    disableCountDown -= Time.deltaTime;
                }
            }
        }

        protected virtual void FireWeapon()
        {
            character.isShooting = true;
            shootCountDown = shootInterval;
            disableCountDown = disableTime;
            aimTarget.position = aimStartPosition.position;
            aimingRightHand.enabled = true;

            currentProjectile = objectPooler.GetObject(currentPool);
            if(currentProjectile != null) 
            {
                Invoke("PlaceProjectile", .1f);
            }
        }

        protected virtual void PlaceProjectile()
        {
            currentProjectile.transform.position = gunBarrel.position;
            currentProjectile.transform.rotation = gunRotation.rotation;
            currentProjectile.SetActive(true);
            if(!character.isFacingLeft)
            {
                currentProjectile.GetComponent<Projectile>().left = false;
            }
            else
            {
                currentProjectile.GetComponent<Projectile>().left = true;
            }
            currentProjectile.GetComponent<Projectile>().fired = true;
            
        }
    }
}

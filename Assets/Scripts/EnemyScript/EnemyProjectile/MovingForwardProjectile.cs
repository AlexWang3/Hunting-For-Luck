using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MovingForwardProjectile : MonoBehaviour
{
        public float lifeTime;
        public float speed;
        public bool left;
        private bool flipped;
        private float projectileLifeTime;

        protected virtual void OnEnable()
        {
            projectileLifeTime = lifeTime;
        }
        protected virtual void FixedUpdate()
        {
            Movement();
        }

        public virtual void Movement()
        {
            projectileLifeTime -= Time.deltaTime;
            if(projectileLifeTime > 0)
            {
                if(!left)
                {
                    if (flipped)
                    {
                        flipped = false;
                        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                    }
                    transform.Translate(Vector2.right * speed * Time.deltaTime);
                }
                else 
                {
                    if (!flipped)
                    {
                        flipped = true;
                        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                    }
                    transform.Translate(Vector2.left * speed * Time.deltaTime);
                }
            }
            else
            {
                DestroyProjectile();
            }
        }
        
        protected virtual void DestroyProjectile()
        {
            Destroy(gameObject);
        }
}

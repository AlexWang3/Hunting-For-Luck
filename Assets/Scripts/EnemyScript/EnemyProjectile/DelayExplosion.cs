using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayExplosion : MonoBehaviour
{
    public ParticleSystem smokingParticle;
    public ParticleSystem explodingParticle;
    
    public float delayTime;
    
    private float countDown;
    private bool triggered;
    private void OnEnable()
    {
        countDown = delayTime;
        smokingParticle.Play();
        explodingParticle.Stop();
        triggered = false;
    }

    private void FixedUpdate()
    {
        if (triggered)
        {
            if (explodingParticle.isStopped)
            {
                Destroy(gameObject);
            }
            return;
        }
        
        if (countDown <= 0)
        {
            smokingParticle.Stop();
            explodingParticle.Play();
            triggered = true;
        }
        else
        {
            countDown -= Time.deltaTime;   
        }
    }
}

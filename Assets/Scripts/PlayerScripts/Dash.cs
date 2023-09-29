using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class Dash : Abilities
    {
        [SerializeField] protected float dashForce;

        [SerializeField] protected float dashCoolDownTime;

        [SerializeField] protected float dashAmountTime;

        [SerializeField] protected LayerMask dashingLayers;

        private bool canDash;

        private float dashCountDown;

        // Update is called once per frame
        void Update()
        {
        
        }
    }   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class LegnaAnimationEvent : MonoBehaviour
    {
        private LegnaCharacter character;
        void Start()
        {
            character = GetComponentInParent<LegnaCharacter>();
        }

        public void JumpAttackAir()
        {
            character.JA_airTrigger = true;
        }
        
        public void JumpAttackFinish()
        {
            character.JA_finishTrigger = true;
        }

        public void NormalAttackFinish()
        {
            character.NA_finishTrigger = true;
            character.TriggerSeriesDelayExplosion(5, 5f, .1f, .2f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{ 
    public class InputManager : MonoBehaviour
    {
        [SerializeField] protected KeyCode jump;
        [SerializeField] protected KeyCode weaponFired;
        [SerializeField] protected KeyCode attack;

        public virtual bool JumpHeld()
        {
            if (Input.GetKey(jump))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool JumpPressed()
        {
            if (Input.GetKeyDown(jump))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool AttackPressed()
        {
            if (Input.GetKeyDown(attack))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool WeaponFired()
        {
            if (Input.GetKeyDown(weaponFired))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}

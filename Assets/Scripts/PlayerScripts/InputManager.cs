using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{ 
    public class InputManager : MonoBehaviour
    {
        [HideInInspector] public bool disabled = false;
        [SerializeField] protected KeyCode jump;
        [SerializeField] protected KeyCode weaponFired;
        [SerializeField] protected KeyCode attack;
        [SerializeField] protected KeyCode down;
        [SerializeField] protected KeyCode up;

        public virtual float GetHorizontal()
        {
            if (disabled) return 0;
            return Input.GetAxis("Horizontal");
        }

        public virtual bool DownHeld()
        {
            if (disabled) return false;
            if (Input.GetKey(down))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public virtual bool UpHeld()
        {
            if (disabled) return false;
            if (Input.GetKey(up))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool DownPressed()
        {
            if (disabled) return false;
            if (Input.GetKeyDown(down))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool UpPressed()
        {
            if (disabled) return false;
            if (Input.GetKeyDown(up))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool JumpHeld()
        {
            if (disabled) return false;
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
            if (disabled) return false;
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
            if (disabled) return false;
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
            if (disabled) return false;
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

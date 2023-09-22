using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace MetroidvaniaTools
{ 
    public class InputManager : MonoBehaviour {
        public static InputManager Instance;

        public void Awake() {
            playerInput = GetComponent<PlayerInput>();
            if (Instance) {
                Debug.Log("Mutiple InputManager");
            }
            if (!Instance) {
                Instance = this;
            }
        }

        public void DisableInput() {
            playerInput.DeactivateInput();
        }

        public void EnableInput() {
            playerInput.ActivateInput();
        }

        public PlayerInput playerInput;
        // Change the way of disabling it;
        [HideInInspector] public bool disabled = false;
        public Vector2 moveInput;
        public bool jump;
        public bool meleeAttack;
        public bool rangeAttack;
        public virtual float GetHorizontal() {
            return moveInput.x;
        }
        // input
        #region Input

        public void OnMove(InputAction.CallbackContext ctx) {
            moveInput = ctx.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext ctx) {
            jump =ctx.performed;
        }

        public void OnMeleeAttack(InputAction.CallbackContext ctx) {
            meleeAttack = ctx.performed;
        }
        public void OnRangeAttack(InputAction.CallbackContext ctx) {
            rangeAttack = ctx.performed;
        }

        private void Update() {
            Debug.Log(moveInput);
        }
        #endregion

        public virtual bool DownHeld() {
            return moveInput.y < 0;
        }
        
        public virtual bool UpHeld()
        {
            return moveInput.y > 0;
        }
        

        public virtual bool JumpHeld() {
            return jump;
        }

        public virtual bool JumpPressed() {
            return jump;
        }

        public virtual bool AttackPressed() {
            return meleeAttack;
        }

        public virtual bool WeaponFired() {
            return rangeAttack;
        }
    }

}

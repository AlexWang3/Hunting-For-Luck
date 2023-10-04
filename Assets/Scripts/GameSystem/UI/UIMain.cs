using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UIMainState : UIState {
    public UIPlayerHealthState playerHealthState;
    public UIEnemyHealthState enemyHealthState;
    public UIMiddleDiceState uiMiddleDiceState;
}

public class UIMain : UIBase<UIMainState> {
    public UIPlayerHealth playerHealth;
    public GameObject enemyStatusBar;
    public UIEnemyHealth enemyHealth;
    public UIMiddleDice middleDice;
    public override void ApplyNewStateInternal() {
        
    }

    private void Start() {
        playerHealth = GetComponentInChildren<UIPlayerHealth>();
        enemyHealth = GetComponentInChildren<UIEnemyHealth>();
        middleDice = GetComponentInChildren<UIMiddleDice>();
    }

    public override void UpdateChildren() {
        playerHealth.state = state.playerHealthState;
        playerHealth.ApplyNewState();
        enemyHealth.state = state.enemyHealthState;
        enemyHealth.ApplyNewState();
        middleDice.state = state.uiMiddleDiceState;
        middleDice.ApplyNewState();
    }
}

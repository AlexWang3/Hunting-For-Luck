using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UIMainState : UIState {
    public UIPlayerDiceState playerDiceState;
    public UIPlayerHealthState playerHealthState;
}

public class UIMain : UIBase<UIMainState> {
    public UIPlayerDice playerDice;
    public UIPlayerHealth playerHealth;
    public override void ApplyNewStateInternal() {
        
    }

    private void Start() {
        playerDice = GetComponentInChildren<UIPlayerDice>();
        playerHealth = GetComponentInChildren<UIPlayerHealth>();
    }

    public override void UpdateChildren() {
        playerDice.state = state.playerDiceState;
        playerDice.ApplyNewState();
        playerHealth.state = state.playerHealthState;
        playerHealth.ApplyNewState();
    }
}

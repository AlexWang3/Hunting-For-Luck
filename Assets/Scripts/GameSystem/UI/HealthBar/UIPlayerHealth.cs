using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
[System.Serializable]
public class UIPlayerHealthState : UIState {
    public int playerHealth;
    public int maxHealth;
}

public class UIPlayerHealth : UIBase<UIPlayerHealthState> {
    
    public Image frontHealthBar;
    public Image backHealthBar;
    public float fastFillSpeed;
    public float slowFillSpeed;
    public override void ApplyNewStateInternal() {
        float previousFill = frontHealthBar.fillAmount;
        float newTargetFill = (float)state.playerHealth / state.maxHealth;
        if (newTargetFill >= previousFill) {
            // Regain Health
            frontHealthBar.DOFillAmount(newTargetFill, slowFillSpeed);
            backHealthBar.DOFillAmount(newTargetFill, fastFillSpeed);
        } else {
            // Take Damage
            frontHealthBar.DOFillAmount(newTargetFill, fastFillSpeed);
            backHealthBar.DOFillAmount(newTargetFill, slowFillSpeed);
        }
    }
}

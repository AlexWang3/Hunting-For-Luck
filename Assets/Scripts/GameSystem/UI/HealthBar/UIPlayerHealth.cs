using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
[System.Serializable]
public class UIPlayerHealthState : UIState {
    public int playerHealth;
    public int maxHealth;
    public int playerCurrentLuckValue;
}

public class UIPlayerHealth : UIBase<UIPlayerHealthState> {
    
    public Image frontHealthBar;
    public Image backHealthBar;
    public float fastFillSpeed;
    public float slowFillSpeed;
    public TMP_Text luckText;
    public float currentLuck;
    public float midBarLuckValue;
    public Slider slider;
    public TMP_Text MidbarText;
    private void Start() {
        currentLuck = 0;
        midBarLuckValue = 0;
    }

    private float midBarTransitDuration = 1.2f;

    public override void ApplyNewStateInternal() {
        float previousFill = frontHealthBar.fillAmount;
        float newTargetFill = (float)state.playerHealth / state.maxHealth;
       
        DOTween.To(SetLuckNumber,currentLuck, state.playerHealth, 1.6f);
        slider.DOValue(midBarLuckValue / state.maxHealth, midBarTransitDuration);
        DOTween.To(SetMidBarNumber,midBarLuckValue, state.playerCurrentLuckValue, midBarTransitDuration);
        currentLuck = state.playerHealth;
        midBarLuckValue = state.playerCurrentLuckValue;
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
    public void SetLuckNumber(float targetValue) {
        int value = Mathf.RoundToInt(targetValue);
        luckText.text = $"{value}/{state.maxHealth}";
    }

    public void SetMidBarNumber(float targetValue) {
        int value = Mathf.RoundToInt(targetValue);
        MidbarText.text = $"{value}";
    }
}

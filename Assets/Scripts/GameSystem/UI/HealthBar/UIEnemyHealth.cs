using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Serialization;

[System.Serializable]
public class UIEnemyHealthState : UIState {
    public int enemyHealth;
    public int maxHealth;
    public int enemyCurrentLuckValue;
}

public class UIEnemyHealth : UIBase<UIEnemyHealthState> {
    
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
        float newTargetFill = (float)state.enemyHealth / state.maxHealth;
       
        DOTween.To(SetLuckNumber,currentLuck, state.enemyHealth, 1.6f);
        slider.DOValue(midBarLuckValue / state.maxHealth, midBarTransitDuration);
        DOTween.To(SetMidBarNumber,midBarLuckValue, state.enemyCurrentLuckValue, midBarTransitDuration);
        currentLuck = state.enemyHealth;
        midBarLuckValue = state.enemyCurrentLuckValue;
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

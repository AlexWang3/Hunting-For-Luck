using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIMiddleDiceState : UIState {
    public int diceNumber;
}

public class UIMiddleDice : UIBase<UIMiddleDiceState> {
    public int currentDiceNumber;
    public TMP_Text middleText;
    public float diceNumberChangeDuration = 1.6f;
    private void Start() {
        currentDiceNumber = 0;
    }
    public override void ApplyNewStateInternal() {
        DOTween.To(SetDiceNumber,currentDiceNumber, state.diceNumber, diceNumberChangeDuration);
        currentDiceNumber = state.diceNumber;
    }
    
    public void SetDiceNumber(float targetValue) {
        int value = Mathf.RoundToInt(targetValue);
        middleText.text = $"{value}";
    }
}

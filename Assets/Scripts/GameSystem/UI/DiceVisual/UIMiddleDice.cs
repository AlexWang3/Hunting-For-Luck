using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIMiddleDiceState : UIState {
    public int diceNumber;
    public GameSystem.AttackSource diceOwner;
    public GameSystem.AttackLevel attackLevel;
}

public class UIMiddleDice : UIBase<UIMiddleDiceState> {
    public int currentDiceNumber;
    public TMP_Text middleText;
    public float diceNumberChangeDuration = 1.6f;
    public Color playerColor;
    public Color enemyColor;
    public Color normalColor;
    public float normalFontSize;
    public Sequence lastSequence;
    private void Start() {
        currentDiceNumber = 0;
    }
    public override void ApplyNewStateInternal() {
        lastSequence.Kill();
        if (state.diceOwner == GameSystem.AttackSource.None) {
            return;
        }
        
        Sequence mySequence = DOTween.Sequence();
        switch (state.diceOwner) {
            case GameSystem.AttackSource.Player:
                mySequence.Insert(0,middleText.DOColor(playerColor, diceNumberChangeDuration));
                break;
            case GameSystem.AttackSource.Enemy:
                mySequence.Insert(0,middleText.DOColor(enemyColor, diceNumberChangeDuration));
                break;
        }
        
        switch (state.attackLevel) {
            case GameSystem.AttackLevel.GreatSuccess:
                mySequence.Insert(0.3f,middleText.DOFontSize(normalFontSize + 10f, diceNumberChangeDuration));
                break;
            case GameSystem.AttackLevel.Success:
                mySequence.Insert(0.3f,middleText.DOFontSize(normalFontSize + 5f, diceNumberChangeDuration));
                break;
            case GameSystem.AttackLevel.Normal:
                mySequence.Insert(0.3f,middleText.DOFontSize(normalFontSize, diceNumberChangeDuration));
                break;
            case GameSystem.AttackLevel.Fail:
                mySequence.Insert(0.3f,middleText.DOFontSize(normalFontSize - 5f, diceNumberChangeDuration));
                break;
            case GameSystem.AttackLevel.GreatFail:
                mySequence.Insert(0.3f,middleText.DOFontSize(normalFontSize - 10f, diceNumberChangeDuration));
                break;
        }
        mySequence.Insert(0f,DOTween.To(SetDiceNumber,currentDiceNumber, state.diceNumber, diceNumberChangeDuration));
        mySequence.Insert(5f,middleText.DOFontSize(normalFontSize, diceNumberChangeDuration));
        mySequence.Insert(5f,middleText.DOColor(normalColor, diceNumberChangeDuration));
        mySequence.Play();
        lastSequence = mySequence;
        currentDiceNumber = state.diceNumber;
    }
    
    public void SetDiceNumber(float targetValue) {
        int value = Mathf.RoundToInt(targetValue);
        middleText.text = $"{value}";
    }
}

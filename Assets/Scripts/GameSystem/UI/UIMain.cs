using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MetroidvaniaTools;
using UnityEngine.Serialization;

public class UIMainState : UIState {
    public UIPlayerHealthState playerHealthState;
    public UIEnemyHealthState enemyHealthState;
    public UIMiddleDiceState uiMiddleDiceState;
}

public class UIMain : UIBase<UIMainState> {
    public UIPlayerHealth playerHealth;
    public UIEnemyHealth enemyHealth;
    public UIMiddleDice middleDice;
    public Image[] enemyStatusImages;
    public TMP_Text[] enemyStatusText;
    public Image[] playerHealthImages;
    public TMP_Text[] playerStatusText;
    public TMP_Text[] middleDiceText;
    public Image[] middleDiceImage; 
    [FormerlySerializedAs("imageFadeTIme")] public float imageFadeTime = 1.2f;
    public float textFadeTime = 1.2f;
    public float midDiceFadeTime = 1.2f;

    public LegnaHealth currentTarget;
    public override void ApplyNewStateInternal() {
        
    }
    

    private void Start() {
        playerHealth = GetComponentInChildren<UIPlayerHealth>();
        enemyHealth = GetComponentInChildren<UIEnemyHealth>();
        middleDice = GetComponentInChildren<UIMiddleDice>();
        enemyStatusImages = enemyHealth.GetComponentsInChildren<Image>();
        enemyStatusText = enemyHealth.GetComponentsInChildren<TMP_Text>();
        playerHealthImages = playerHealth.GetComponentsInChildren<Image>();
        playerStatusText = playerHealth.GetComponentsInChildren<TMP_Text>();
        middleDiceImage= middleDice.GetComponentsInChildren<Image>();
        middleDiceText = middleDice.GetComponentsInChildren<TMP_Text>();
        HideEnemyStatus();
    }
    
    public override void UpdateChildren() {
        playerHealth.state = state.playerHealthState;
        playerHealth.ApplyNewState();
        enemyHealth.state = state.enemyHealthState;
        enemyHealth.ApplyNewState();
        middleDice.state = state.uiMiddleDiceState;
        middleDice.ApplyNewState();
    }
    
    public  void UpdateCurrentEnemy(string enemyName, int currentHealth,int maxHealth, int currentLuck, LegnaHealth target) {
        currentTarget = target;
        Sequence mySequence = DOTween.Sequence();
        foreach (var image in enemyStatusImages) {
            mySequence.Insert(0, image.DOFade(1, imageFadeTime));
        }
        foreach (var text in enemyStatusText) {
            mySequence.Insert(0.1f, text.DOFade(1, textFadeTime));
        }
        foreach (var image in middleDiceImage) {
            mySequence.Insert(0f, image.DOFade(1, midDiceFadeTime));
        }
        foreach (var text in middleDiceText) {
            mySequence.Insert(0.5f, text.DOFade(1, midDiceFadeTime));
        }
        state.enemyHealthState.enemyHealth = currentHealth;
        state.enemyHealthState.maxHealth = maxHealth;
        state.enemyHealthState.enemyCurrentLuckValue = currentLuck;
        state.enemyHealthState.SetEnemyName(enemyName);
        state.enemyHealthState.MarkDirty();
    }

    public void HideEnemyStatus() {
        Sequence mySequence = DOTween.Sequence();
        foreach (var image in enemyStatusImages) {
            mySequence.Insert(0f, image.DOFade(0, 0));
        }
        foreach (var text in enemyStatusText) {
            mySequence.Insert(0f, text.DOFade(0, 0));
        }
        foreach (var image in middleDiceImage) {
            mySequence.Insert(0f, image.DOFade(0, 0));
        }
        foreach (var text in middleDiceText) {
            mySequence.Insert(0f, text.DOFade(0, 0));
        }
        mySequence.Play();
    }
}

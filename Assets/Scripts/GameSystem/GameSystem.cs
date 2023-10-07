using System;
using System.Collections;
using System.Collections.Generic;
using MetroidvaniaTools;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public interface IDamagebale {
    public void TakeDamage(int amount);
    public bool IsGiveUpwardForce();
    public int ReturnHealthPoint();
}

interface IAttacker {
    int DamageAmound();
}

public class G {
    public static GameSystem I {
        get {
            if (_I == null) {
                _I = GameObject.FindObjectOfType<GameSystem>();
            }
            return _I;
        }
    }
    public static UIMainState UI {
        get {
            return G.I.UIMain.state;
        }
    }
    static GameSystem _I;
}

public class GameSystem : MonoBehaviour {
    public UIMain UIMain;
    public GameObject player;
    public Character character;
    public PlayerHealth playerHealth;
    public bool finishInitialize;
    private void Start() {
        player = PlayerHealth.Instance.gameObject;
        playerHealth = player.GetComponent<PlayerHealth>();
        character = Character.Instance;
        finishInitialize = false;
        InitializeUIMain();
    }
    public void Update() {
        UIMain.ApplyNewState();
    }

    public void InitializeUIMain() {
        UIMain.state = new UIMainState() {
            playerHealthState = new UIPlayerHealthState() {
                maxHealth = playerHealth.maxHealthPoints,
                playerHealth = 0,
                playerCurrentLuckValue = playerHealth.playerCurrentLuckValue,
                previousFill = (float)playerHealth.playerCurrentLuckValue/playerHealth.maxHealthPoints,
            },
            enemyHealthState = new UIEnemyHealthState() {
                maxHealth = 10,
                enemyHealth = 0,
                enemyCurrentLuckValue = 0,
            },
            uiMiddleDiceState = new UIMiddleDiceState() {
                diceNumber = 0,
                diceOwner = AttackSource.None,
            }
        };
        finishInitialize = true;
    }
    public enum AttackSource {
        None,
        Enemy,
        Player,
    }

    public enum AttackLevel {
        GreatSuccess,
        Success,
        Normal,
        Fail,
        GreatFail,
    }

    public int DamageCalculation(int selfHealth, int targetHealth, int damageAmount,AttackSource attackSource) {
        int randomNumber = Random.Range(0, selfHealth + targetHealth + 1);
        G.UI.uiMiddleDiceState.diceNumber = randomNumber;
        G.UI.uiMiddleDiceState.diceOwner = attackSource;
        if (playerHealth.luckSkill && attackSource == AttackSource.Player) {
            G.UI.uiMiddleDiceState.attackLevel = AttackLevel.GreatSuccess;
            G.UI.uiMiddleDiceState.diceNumber = 0;
            G.UI.uiMiddleDiceState.MarkDirty();
            return damageAmount * 3;
        }

        if (randomNumber < 0.1f * selfHealth) {
            G.UI.uiMiddleDiceState.attackLevel = AttackLevel.GreatSuccess;
            G.UI.uiMiddleDiceState.MarkDirty();
            return damageAmount * 3;
        } 
        if (randomNumber < 0.5f * selfHealth) {
            G.UI.uiMiddleDiceState.attackLevel = AttackLevel.Success;
            G.UI.uiMiddleDiceState.MarkDirty();
            return damageAmount * 2;
        } 
        if (randomNumber < selfHealth) {
            G.UI.uiMiddleDiceState.attackLevel = AttackLevel.Normal;
            G.UI.uiMiddleDiceState.MarkDirty();
            return damageAmount;
        } 
        if (randomNumber < 0.5f * targetHealth + selfHealth) {
            G.UI.uiMiddleDiceState.attackLevel = AttackLevel.Fail;
            G.UI.uiMiddleDiceState.MarkDirty();
            return Mathf.RoundToInt(damageAmount * 0.5f);
        } 
        G.UI.uiMiddleDiceState.attackLevel = AttackLevel.GreatFail;
        G.UI.uiMiddleDiceState.MarkDirty();
        return Mathf.RoundToInt(damageAmount * 0.3f);
    }

}

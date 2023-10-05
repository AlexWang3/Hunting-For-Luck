using System;
using System.Collections;
using System.Collections.Generic;
using MetroidvaniaTools;
using UnityEngine;

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

    private void Awake() {
        player = Character.Instance.gameObject;
        character = Character.Instance;
        InitializeUIMain();
    }
    public void Update() {
        UIMain.ApplyNewState();
    }

    public void InitializeUIMain() {
        UIMain.state = new UIMainState() {
            playerHealthState = new UIPlayerHealthState() {
                maxHealth = player.GetComponent<Health>().maxHealthPoints,
                playerHealth = 0,
                playerCurrentLuckValue = player.GetComponent<PlayerHealth>().playerCurrentLuckValue,
            },
            enemyHealthState = new UIEnemyHealthState() {
                maxHealth = 100,
                enemyHealth = 0,
                enemyCurrentLuckValue = 0,
            },
            uiMiddleDiceState = new UIMiddleDiceState() {
                diceNumber = 0,
            }
        };
    }
}

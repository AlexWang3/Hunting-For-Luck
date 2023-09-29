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
        player = FindObjectOfType<Character>().gameObject;
        character = player.GetComponent<Character>();
        InitializeUIMain();
    }
    public void Update() {
        UIMain.ApplyNewState();
    }

    public void InitializeUIMain() {
        UIMain.state = new UIMainState() {
            playerDiceState = new UIPlayerDiceState() {
                activeDiceList = new List<UIPlayerSingleDiceState>()
            },
            playerHealthState = new UIPlayerHealthState() {
                maxHealth = player.GetComponent<Health>().maxHealthPoints,
                playerHealth = player.GetComponent<Health>().maxHealthPoints,
            }
        };
    }
}

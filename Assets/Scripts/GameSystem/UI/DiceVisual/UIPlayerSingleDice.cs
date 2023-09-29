using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiceType {
    Red,
    Yellow,
    Blue,
}
[System.Serializable]
public class UIPlayerSingleDiceState : UIState {
    public int diceNumber;
    public DiceType diceType;
}
public class UIPlayerSingleDice : UIBase<UIPlayerSingleDiceState> {
    public Animator animator;
    public override void ApplyNewStateInternal() {
        //change the appearance
    }
}

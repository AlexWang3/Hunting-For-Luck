using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UIPlayerDiceState : UIState {
    public List<UIPlayerSingleDiceState> activeDiceList;
}

public class UIPlayerDice : UIBase<UIPlayerDiceState> {
    public RectTransform diceContainer;
    public GameObject dicePrefab;
    public override void ApplyNewStateInternal() {
        ApplyNewStateArray<UIPlayerSingleDiceState, UIPlayerSingleDice>(diceContainer, dicePrefab, state.activeDiceList);
    }
}

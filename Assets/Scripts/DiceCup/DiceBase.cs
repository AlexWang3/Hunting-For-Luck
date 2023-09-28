using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceBase : Hittable {
    public int rollNumber;
    public DiceType dicetype;
    public override void OnHit() {
        rollNumber = Random.Range(1, 7);
        G.UI.playerDiceState.activeDiceList.Add(new UIPlayerSingleDiceState() { diceNumber = rollNumber, diceType = this.dicetype});
        G.UI.playerDiceState.MarkDirty();
    }
}

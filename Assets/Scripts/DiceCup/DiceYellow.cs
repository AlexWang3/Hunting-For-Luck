using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceYellow : DiceBase
{
    private void Start() {
        dicetype = DiceType.Yellow;
    }
    public override void OnHit() {
        base.OnHit();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiceRed : DiceBase
{
    private void Start() {
        dicetype = DiceType.Red;
    }

    public override void OnHit() {
        base.OnHit();
    }
}

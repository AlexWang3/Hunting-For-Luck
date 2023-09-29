using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceBlue : DiceBase
{
   private void Start() {
      dicetype = DiceType.Blue;
   }
   public override void OnHit() {
      base.OnHit();
   }
}

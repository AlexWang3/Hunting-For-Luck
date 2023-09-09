using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponState: PlayerBaseState
{   
    public override void EnterState(PlayerStateManager player)
    {
        player.shootEnd = false;
        player.rb.velocity = new Vector2(0, 0);
        player.anim.SetBool("Shooting", true);
        player.anim.SetBool("ShootLoop", true);
    }
    
    public override void UpdateState(PlayerStateManager player)
    {
        if (player.input.WeaponFired()) 
        {
            player.anim.SetBool("Shooting", true);
            player.anim.SetBool("ShootLoop", true);
        }
        if (player.shootEnd) {
            player.shootEnd = false;
            player.SwitchState(player.idelState);
        }
    }

}

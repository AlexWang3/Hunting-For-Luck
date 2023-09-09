using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void UpdateState(PlayerStateManager player)
    {
        player.input.MovementPressed(player);
        if (!player.isGrounded) {
            player.SwitchState(player.fallState);
            return;
        }
        if (player.numberOfJumpsLeft > 0 && player.input.JumpPressed())
        {
            player.SwitchState(player.jumpState);
            return;
        }
        if (player.input.WeaponFired())
        {
            player.SwitchState(player.weaponState);
            return;
        }
        
    }

    public override void FixedUpdateState(PlayerStateManager player) {
        player.Movement();
        player.GroundCheck();
    }
}

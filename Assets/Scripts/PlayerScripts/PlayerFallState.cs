using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public override void UpdateState(PlayerStateManager player)
    {
        player.input.MovementPressed(player);
        if (player.isGrounded) {
            player.SwitchState(player.idelState);
        }
        else if (player.numberOfJumpsLeft > 0 && player.input.JumpPressed())
        {
            player.SwitchState(player.jumpState);
        }
    }

    public override void FixedUpdateState(PlayerStateManager player) {
        player.Movement();
        player.GroundCheck();
    }
}

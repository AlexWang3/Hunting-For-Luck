using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.numberOfJumpsLeft--;
        player.rb.velocity = new Vector2(player.rb.velocity.x, 0);
        player.jumpCountDown = player.buttonHoldTime;
        player.fallCountDown = player.glideTime;
        player.isJumping = true;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (!player.input.JumpHeld()){
            player.SwitchState(player.fallState);
        }
        player.input.MovementPressed(player);
    }

    public override void FixedUpdateState(PlayerStateManager player)
    {
        player.Movement();
        player.GroundCheck();
        player.rb.AddForce(Vector2.up * player.jumpForce);
        if (player.numberOfJumpsLeft <= player.maxJumps - 2)
        {
            player.anim.SetBool("DoubleJump", true);
        }
        AdditionalAir(player);
    }

    public override void ExitState(PlayerStateManager player) {
        player.isJumping = false;
    }

    protected virtual void AdditionalAir(PlayerStateManager player)
    {
        if (player.input.JumpHeld())
        {
            player.jumpCountDown -= Time.deltaTime;
            if (player.jumpCountDown <= 0)
            {
                player.jumpCountDown = 0;
                player.SwitchState(player.fallState);
            }
            else
            {
                player.rb.AddForce(Vector2.up * player.holdForce);
            }
        }
        else
        {
            player.SwitchState(player.fallState);
        }
        if (player.rb.velocity.y > player.maxJumpSpeed)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.maxJumpSpeed);
        }
    }
    
}

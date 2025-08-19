using Godot;
using System;
using Characters;
using System.Threading.Tasks;
using Datas;

public enum PlayerActionsEnum {
    Idle,
    Crouching,
    Moving,
    PreJumping,
    Jumping,
    Attacking,
    Rolling,
}

public partial class Player : SuperCharacter {

    public MoveStateEnum MoveState { get; private set; } = MoveStateEnum.Walking;

    public Player() : base(1) {

    }

    public override void _Ready() {
        base._Ready();
    }

    public override void _PhysicsProcess(double delta) {
        Direction = Input.GetVector("Left", "Right", "Up", "Down");
        if (Input.IsActionJustPressed("Roll")) {

        } else if (ReadyToMove) {
            if (Direction.X != 0 || Direction.Y != 0) {
                MoveAndFlipAndPlayAnim(delta, "Walking", "Running");
            } else {
                PlayAnim("Idle");
            }
        }
    }
}

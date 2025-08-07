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

public partial class Player : UltraSimpleCharacter {

    public MoveStateEnum MoveState { get; private set; } = MoveStateEnum.Walking;
    public PlayerActions Actions { get; private set; }

    public Sprite2D Sprite;
    public AnimationPlayer Anim;

    public float Gravity { get; private set; }
    public float VelX { get; set; } = 0f;
    public float VelY { get; set; } = 0f;


    public Player() {
        SetCharacter(100, 75f, 1.75f, true);
        Actions = new PlayerActions(this);
    }

    public override void _Ready() {
        Sprite = GetNode<Sprite2D>("Sprite2D");
        Anim = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _PhysicsProcess(double delta) {
        float deltaF = (float)delta;
        Vector2 Axis = Input.GetVector("Left", "Right", "Up", "Down");
        if (Input.IsActionJustPressed("Roll") && Actions.Rolling.Avaliable && !Actions.Rolling.IsRunning) {
            Actions.Rolling.Start();
            GD.Print("PRESS Q");
        }
        Velocity = new Vector2(VelX, 0);
        MoveAndSlide();
    }

    public void PlayAnim(string animName, float speedScale = 1f) {
        if (Anim.CurrentAnimation != animName) {
            Anim.SpeedScale = speedScale;
            Anim.Play(animName);
        }
    }
}

public partial class PlayerActions : Node {
    public PlayerActionsEnum Current { get; private set; } = PlayerActionsEnum.Idle;
    public RollingAction Rolling { get; private set; }

    public PlayerActions(Player _player) {
        Rolling = new RollingAction(_player, new SimpleEffectNumFloat(200), new SimpleEffectNumFloat(1, 50f), new SimpleEffectNumFloat(2, 100f), true);
    }
}


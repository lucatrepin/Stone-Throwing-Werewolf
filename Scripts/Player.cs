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

public partial class Player : UltraCharacter01 {

    public MoveStateEnum MoveState { get; private set; } = MoveStateEnum.Walking;
    public PlayerActions Actions { get; private set; }

    public AnimationPlayer Anim;

    public float Gravity { get; private set; }

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
        if (Input.IsActionJustPressed("Roll") && Actions.Rolling.Start()) {
            GD.Print("Rolando");
        }
        Velocity = new Vector2(VelX, 0);
        MoveAndSlide();
        //GD.Print(VelX);
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
    public RollingActionPlayer Rolling { get; private set; }

    public PlayerActions(Player _player) {
        Rolling = new RollingActionPlayer(_player, new SimpleEffectNumFloat(500), new SimpleEffectNumFloat(1, 50f), new SimpleEffectNumFloat(1, 50f), "Roll", true);
    }
}


public partial class RollingActionPlayer : RollingAction<Player> {
    public RollingActionPlayer(BaseCharacter character, SimpleEffectNumFloat power, SimpleEffectNumFloat duration, SimpleEffectNumFloat cooldown, string animName, bool avaliable) : base(character, power, duration, cooldown, animName, avaliable) {

    }

    public override void PlayAnim(string animName, float speedScale) {
        ((Player)_character).PlayAnim(animName, speedScale);
    }
}
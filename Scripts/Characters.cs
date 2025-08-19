using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datas;
using System.Runtime.Intrinsics;


namespace Characters {
    public enum AttributesEnum {
        Vigor,
        Endurance,
        Strength,
        Dexterity,
        Intelligence,
        Focus,
        Arcane,
        Faith
    }

    public enum EffectsEnum {
        Slashing,
        Piercing,
        Bludgeoning,
        Fire,
        Ice,
        Water,
        Electric,
        Earth,
        Air,
        Light,
        Darkness,
        Arcane,
        Bleed,
        Burn,
        Freeze,
        paralysis,
        Stun,
        Silence,
        Weakness,
        Curse,
        Slow,
        Fear,
        Confusion,
        Petrification,
    }

    public enum MoveStateEnum {
        Walking,
        Running,
    }

    public enum CharacterTypeEnum {
        Player,
        NPC,
        Enemy,
    }

    /// <summary>
    /// A base pra todo character não tem movimento, em base só nivel
    /// </summary>
    public abstract partial class BaseCharacter : CharacterBody2D {
        public uint Level { get; protected set; } = 0;
        public Sprite2D Sprite { get; set; }
        public AnimationPlayer Anim { get; set; }

        public virtual void PlayAnim(string animName, float speedScale = 1f) {
            if (Anim.CurrentAnimation != animName) {
                Anim.SpeedScale = speedScale;
                Anim.Play(animName);
            }
        }

        public override void _Ready() {
            Sprite = GetNode<Sprite2D>("Sprite2D");
            Anim = GetNode<AnimationPlayer>("AnimationPlayer");
        }

        public BaseCharacter(uint level) {
            Level = level;
        }
    }

    /// <summary>
    /// com movimento unico
    /// </summary>
    public partial class SimpleCharacter01 : BaseCharacter {
        public float Direction { get; set; } = 1f;
        public SimpleEffectNumFloat Speed { get; set; } = new SimpleEffectNumFloat(200);

        public SimpleCharacter01(uint level, SimpleEffectNumFloat speed) : base(level) {
            Speed = speed;
        }

        public override void _Ready() {
            base._Ready();
        }

        public virtual void Move(double delta) {
            float deltaF = (float)delta;
            Velocity = new Vector2(Direction * Speed.Value * deltaF, 0);
            MoveAndSlide();
        }
    }
    
    /// <summary>
    /// com movimento Run / Walk
    /// </summary>
    public partial class SimpleCharacter02 : SimpleCharacter01 {
        public bool Run { get; set; } = false;

        public SimpleCharacter02(uint level, SimpleEffectNumFloat speed) : base(level, speed) {

        }

        public override void _Ready() {
            base._Ready();
        }

        public override void Move(double delta) {
            float deltaF = (float)delta;
            Velocity = new Vector2(Direction * Speed.Value * (Run ? 1f : 0.5f) * deltaF, 0);
            MoveAndSlide();
        }
    }

    /// <summary>
    /// com voo e movimento Run / Walk
    /// </summary>
    public partial class AdvancedCharacter : BaseCharacter {
        public Vector2 Direction { get; set; } = new Vector2(1f, 0f);
        public bool Run { get; set; } = false;
        public bool Flying { get; set; } = false;
        public SimpleEffectNumFloat Speed { get; set; }
        public SimpleEffectNumFloat FlySpeed { get; set; }

        public override void _Ready() {
            base._Ready();
            Speed = Speed ?? new SimpleEffectNumFloat(5000);
            FlySpeed = FlySpeed ?? new SimpleEffectNumFloat(2500);
        }

        public AdvancedCharacter(uint level, SimpleEffectNumFloat speed = null, SimpleEffectNumFloat flySpeed = null) : base(level) {
            Speed = speed ?? Speed;
            FlySpeed = flySpeed ?? flySpeed;
        }

        public virtual void Move(double delta) {
            float deltaF = (float)delta;
            Velocity = new Vector2(Direction.X * Speed.Value * (Run ? 1f : 0.5f) * deltaF, Direction.Y * FlySpeed.Value * (Flying ? 1f : 0f) * deltaF);
            MoveAndSlide();
        }

        public virtual void MoveAndFlipAndPlayAnim(double delta, string animNameWalk, string animNameRun) {
            Move(delta);
            Sprite.FlipH = Direction.X < 0;
            if (Velocity.Y == 0) {
                if (Run) PlayAnim(animNameRun);
                else PlayAnim(animNameWalk);
            }
        }
    }
    
    /// <summary>
    /// Adequado para personagens com mais actions
    /// </summary>
    public partial class SuperCharacter : AdvancedCharacter {
        public bool ReadyToMove { get; set; } = true;

        public SuperCharacter(uint level, SimpleEffectNumFloat speed = null, SimpleEffectNumFloat flySpeed = null) : base(level, speed, flySpeed) {

        }

        public override void _Ready() {
            base._Ready();
        }

        public override void Move(double delta) {
            base.Move(delta);
        }
    }

    /// <summary>
    /// com movimento Run / Walk e atributos
    /// </summary>
    public partial class UltraCharacter : BaseCharacter {
        protected float Weight { get; private set; }
        protected float Height { get; private set; }

        public UltraCharacter(uint level, SimpleEffectNumFloat speed, SimpleEffectNumFloat flySpeed) : base(level) {

        }

        protected Dictionary<AttributesEnum, SimpleEffectNumInt> Attributes { get; private set; } = new Dictionary<AttributesEnum, SimpleEffectNumInt> {
            { AttributesEnum.Vigor, null },
            { AttributesEnum.Endurance, null },
            { AttributesEnum.Strength, null },
            { AttributesEnum.Dexterity, null },
            { AttributesEnum.Intelligence, null },
            { AttributesEnum.Focus, null },
            { AttributesEnum.Arcane, null },
            { AttributesEnum.Faith, null }
        };

        public async void SetCharacter(uint level = 1, float weight = 70f, float height = 1.75f, bool printAll = false) {
            Level = level;
            Weight = weight;
            Height = height;
            await Task.Run(() => SetAttributes(Level));
        }

        private void SetAttributes(uint level, float initialNumNoExponential = 100f) {
            Random random = new Random();
            int uniformValue = (int)(level / Attributes.Count);
            int diference = (int)(level % Attributes.Count);
            foreach (AttributesEnum attribute in Attributes.Keys) {
                Attributes[attribute] = new SimpleEffectNumInt(Math.Max(1, uniformValue), initialNumNoExponential);
            }
            while (diference > 0) {
                Attributes[(AttributesEnum)diference - 1].AddNumInt(1);
                diference--;
            }
        }

        public void Move(double delta) {

        }
    }

    

}
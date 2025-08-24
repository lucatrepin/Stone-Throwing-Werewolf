using Godot;
using System;
using Datas;
using System.Collections.Generic;
using System.Threading.Tasks;


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
    /// com movimento simples, só anda pra frente e pra trás
    /// </summary>
    public partial class SimpleCharacter : BaseCharacter {
        public SimpleEffectNumFloat Speed { get; private set; } = new SimpleEffectNumFloat(5000);
        public float Direction { get; set; } = 1f;

        public SimpleCharacter(uint level, SimpleEffectNumFloat speed = null) : base(level) {
            Speed = speed ?? speed;
        }

        public virtual void Move(float deltaF) {
            Velocity = new Vector2(Speed.Value * Direction * deltaF, 0);
            MoveAndSlide();
        }
    }

    /// <summary>
    /// com movimento Run / Walk, mas sem atributos
    /// </summary>
    public partial class AdvancedCharacter : SimpleCharacter {
        public bool Run { get; set; } = false;

        public AdvancedCharacter(uint level, SimpleEffectNumFloat speed = null) : base(level, speed) {

        }

        public override void Move(float deltaF) {
            Velocity = new Vector2((Run ? 1f : 0.5f) * Speed.Value * Direction * deltaF, 0);
            MoveAndSlide();
        }
    }

    /// <summary>
    /// com movimento, Fly
    public partial class SuperCharacter : BaseCharacter {
        public SimpleEffectNumFloat Speed { get; private set; } = new SimpleEffectNumFloat(5000);
        public SimpleEffectNumFloat FlySpeed { get; private set; } = new SimpleEffectNumFloat();
        public Vector2 Direction { get; set; }
        public bool Fly { get; set; } = false;
        public bool Run { get; set; } = false;

        public SuperCharacter(uint level, SimpleEffectNumFloat speed = null, SimpleEffectNumFloat flySpeed = null) : base(level) {
            FlySpeed = flySpeed ?? flySpeed;
        }

        public virtual void Move(float deltaF) {
            Velocity = new Vector2((Run ? 1f : 0.5f) * Speed.Value * Direction.X * deltaF, Fly ? FlySpeed.Value * Direction.Y * deltaF : 0);
            MoveAndSlide();
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
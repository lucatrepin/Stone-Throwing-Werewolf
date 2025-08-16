using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datas;


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

    public abstract partial class BaseCharacter : CharacterBody2D {
        public uint Level { get; protected set; } = 0;
        public float VelX { get; set; } = 0;
        public float VelY { get; set; } = 0;
        public Sprite2D Sprite { get; set; }
    }
    public partial class UltraCharacter01 : BaseCharacter {
        protected float Weight { get; private set; }
        protected float Height { get; private set; }

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

        public void PrintAll() {
            GD.Print("LEVEL: " + Level);
            GD.Print("WEIGHT: " + Weight);
            GD.Print("HEIGHT: " + Height);
            GD.Print("ATTRIBUTES:");
            foreach (var attribute in Attributes) {
                GD.Print($"{attribute.Key}: {attribute.Value.NumInt}");
            }
        }

        public void Print(AttributesEnum attribute) {
            GD.Print(attribute + ": " + Attributes[attribute].Value);
        }
    }

    public abstract partial class CharacterAction : Node {
        public BaseCharacter _character { get; protected set; }
        public string AnimName { get; protected set; } = "Idle";
        public SimpleEffectNumFloat TimeDuration { get; protected set; } = null;
        public SimpleEffectNumFloat Cooldown { get; protected set; } = null;
        public SimpleEffectNumFloat Power { get; protected set; } = null;
        public bool IsRunning { get; protected set; } = false;
        public bool Avaliable { get; protected set; } = false;
        public bool Released { get; protected set; } = false;
        public float TimeTimer { get; protected set; } = 0;

        public CharacterAction(BaseCharacter character, SimpleEffectNumFloat power, SimpleEffectNumFloat duration, SimpleEffectNumFloat cooldown, string animName, bool released = false) {
            _character = character;
            _character.AddChild(this);
            Power = power;
            TimeDuration = duration;
            Cooldown = cooldown;
            AnimName = animName;
            if (released) {
                Released = true;
                Avaliable = true;
            }
        }

        public override void _Ready() {
            this.SetPhysicsProcess(false);
        }

        public virtual bool Start() {
            if (!Released) {
                return false;
            } else if (IsRunning) {
                return false;
            } else if (Avaliable) {
                IsRunning = true;
                Avaliable = false;
                PlayAnim(AnimName, 1f / TimeDuration.Value);
                this.SetPhysicsProcess(true);
                return true;
            }
            return false;
        }

        public abstract void PlayAnim(string animName, float speedScale);

        public override void _PhysicsProcess(double delta) {
            TimeTimer += (float)delta;
            Process();
            if (TimeTimer >= TimeDuration.Value) End();
        }

        public abstract void Process();

        public async void End() {
            IsRunning = false;
            TimeTimer = 0;
            this.SetPhysicsProcess(false);
            await Task.Delay((int)(Cooldown.Value * 1000));
            Avaliable = true;
        }

    }

    public partial class RollingAction<Parent> : CharacterAction {
        public override void _Ready() {
            base._Ready();
        }

        public override void Process() {
            _character.VelX = Math.Max(0, (TimeDuration.Value - TimeTimer) * Power.Value) * (_character.Sprite.FlipH ? -1 : 1);
        }

        public RollingAction(BaseCharacter character, SimpleEffectNumFloat power, SimpleEffectNumFloat duration, SimpleEffectNumFloat cooldown, string animName, bool avaliable) : base(character, power, duration, cooldown, animName, avaliable) {

        }

        public override void PlayAnim(string animName, float speedScale = 1f) {

        }
    }

}
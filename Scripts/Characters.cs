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

    public partial class UltraSimpleCharacter : CharacterBody2D {
        protected uint Level { get; private set; }

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
        public Player _player;
        public SimpleEffectNum<float> TimeDuration { get; protected set; } = null;
        public SimpleEffectNum<float> Cooldown { get; protected set; } = null;
        public SimpleEffectNum<float> Power { get; protected set; } = null;
        public bool IsRunning { get; protected set; } = false;
        public bool Avaliable { get; protected set; } = true;
        public float TimeTimer { get; protected set; } = 0;

        public CharacterAction(Player player, SimpleEffectNumFloat power, SimpleEffectNumFloat duration, SimpleEffectNumFloat cooldown, bool avaliable = false) {
            _player = player;
            _player.AddChild(this);
            Power = power;
            TimeDuration = duration;
            Cooldown = cooldown;
            Avaliable = avaliable;
        }

        public override void _Ready() {
            this.SetPhysicsProcess(false);
        }

        public virtual void Start() {
            if (IsRunning) {
            } else {
                IsRunning = true;
                this.SetPhysicsProcess(true);
            }
        }

        public override void _PhysicsProcess(double delta) {
            TimeTimer += (float)delta;
            if (TimeTimer >= TimeDuration.Value) End();
            else Process();
        }

        public abstract void Process();

        public async void End() {
            IsRunning = false;
            TimeTimer = 0;
            this.SetPhysicsProcess(false);
            await Task.Delay((int)(Cooldown.Value * 1000));
        }
    }

    public partial class RollingAction : CharacterAction {
        public override void _Ready() {
            base._Ready();
            GD.Print("RollingAction ready for player: " + _player);
        }

        public override void Process() {
            _player.VelX = Math.Max(0, (TimeDuration.Value - TimeTimer) * Power.Value);
        }

        public RollingAction(Player player, SimpleEffectNumFloat power, SimpleEffectNumFloat duration, SimpleEffectNumFloat cooldown, bool avaliable) : base(player, power, duration, cooldown, avaliable) {

        }
    }

}
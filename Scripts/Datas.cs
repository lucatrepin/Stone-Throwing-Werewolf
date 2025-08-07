using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Datas {

    public abstract class SimpleEffectNum<Type> {
        public int NumInt { get; protected set; }
        public float NumNoExponential { get; protected set; }
        public Type Value { get; protected set; }
        public Dictionary<string, (int, float)> EquipamentEffect { get; protected set; } = new Dictionary<string, (int, float)>();
        public Dictionary<string, (int, float)> Effects { get; protected set; } = new Dictionary<string, (int, float)>();

        public SimpleEffectNum(int numInt, float numNoExponential) {
            NumInt = numInt;
            NumNoExponential = numNoExponential;
        }

        protected object Get() {
            return (NumInt + EquipamentEffect.Sum(x => x.Value.Item1) + Effects.Sum(x => x.Value.Item1)) * ((NumNoExponential + EquipamentEffect.Sum(x => x.Value.Item2) + Effects.Sum(x => x.Value.Item2)) / 100f);
        }
        public abstract void Set();

        public void AddNumInt(int numInt) {
            NumInt += numInt;
            Set();
        }
        public void AddNumNoExponential(float numNoExponential) {
            NumNoExponential += numNoExponential;
            Set();
        }
        public void RemoveNumInt(int numInt) {
            NumInt -= numInt;
            Set();
        }
        public void RemoveNumNoExponential(float numNoExponential) {
            NumNoExponential -= numNoExponential;
            Set();
        }

        public void AddEquipamentEffect(string name, int numInt = 0, float numNoExponential = 0f) {
            EquipamentEffect.Add(name, (numInt, numNoExponential));
            Set();
        }
        public void RemoveEquipamentEffect(string name) {
            if (EquipamentEffect.ContainsKey(name)) {
                EquipamentEffect.Remove(name);
                Set();
            } else GD.Print("Erro De Remover Equipamento: " + name + " nao existe");
        }

        public async void AddEffect(int numInt = 0, float numNoExponential = 0, int duration = 5000, int tier = 1) {
            string id = GetId(tier.ToString());
            Effects.Add(id, (numInt, numNoExponential));
            Set();
            await Task.Delay(duration);
            RemoveEffect(id);
        }
        public void RemoveEffect(string id) {
            if (Effects.Keys.Contains(id)) {
                Effects.Remove(id);
                Set();
            } else GD.Print("ERRO De Remover Efeito: " + id + " nao existe");
        }

        private string GetId(string name = "Nome", int tier = 1, int count = 6) {
            string codigo = GenerateRandomCode(count).ToString();
            return $"{name}_{tier}_{codigo}";
        } // nome_tier_codigo == CapaceteDeOuro_9_fbNa8b / Sangramento_1_8bNa8b

        private IEnumerable<int> GenerateRandomCode(int count) {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            for (int i = 0; i < count; i++) {
                yield return chars[random.Next(chars.Length)];
            }
        }
    }

    public class SimpleEffectNumInt : SimpleEffectNum<int> {
        public override void Set() {
            Value = (int)((NumInt + EquipamentEffect.Sum(x => x.Value.Item1) + Effects.Sum(x => x.Value.Item1)) * ((NumNoExponential + EquipamentEffect.Sum(x => x.Value.Item2) + Effects.Sum(x => x.Value.Item2)) / 100f));
        }
        public SimpleEffectNumInt(int numInt = 1, float numNoExponential = 100f) : base(numInt, numNoExponential) {
            Set();
        }
    }
    public class SimpleEffectNumFloat : SimpleEffectNum<float> {
        public override void Set() {
            Value = (float)((NumInt + EquipamentEffect.Sum(x => x.Value.Item1) + Effects.Sum(x => x.Value.Item1)) * ((NumNoExponential + EquipamentEffect.Sum(x => x.Value.Item2) + Effects.Sum(x => x.Value.Item2)) / 100f));
        }
        public SimpleEffectNumFloat(int numInt = 1, float numNoExponential = 100f) : base(numInt, numNoExponential) {
            Set();
        }
    }
}


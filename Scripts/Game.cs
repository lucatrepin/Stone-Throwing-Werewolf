using Godot;
using System;

public partial class Game : Node2D {
    [Export(PropertyHint.Dir)] public string fasesPath = "res://Scenes/Fases/";
    public int fase;
    public override void _Ready() {
        fase = 1;
        LoadFase(fase);
    }
    public void LoadFase(int numFase) {
        string fasePath = $"{fasesPath}Fase{numFase}.tscn";
        PackedScene fase = GD.Load<PackedScene>(fasePath);
        try {
            AddChild(fase.Instantiate());
        } catch {
            GD.Print("Erro ao carregar nova fase");
        }
    }

}

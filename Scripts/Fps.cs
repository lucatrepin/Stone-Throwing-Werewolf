using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Fps : Label {
    private List<double> listFps = new List<double>();
    private int maxNumList = 3600; //ultimo minuto
    public override void _PhysicsProcess(double delta) {
        listFps.Add(Engine.GetFramesPerSecond());
        if (listFps.Count > maxNumList) listFps.RemoveAt(0);
        this.Text = $"{(int)(listFps.Sum() / listFps.Count)}";
    }
}
using Godot;
using System;

public class ParticalBrain : Particles2D {
    public override void _Ready() {
        Emitting = true;
    }

    public override void _Process(float delta) {
        if (Emitting == false) {
            GetParent().RemoveChild(this);
            this.QueueFree();
        }
    }
}

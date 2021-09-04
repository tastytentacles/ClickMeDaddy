using Godot;
using System;

public class UpdateBarBasic : Panel {
    //TODO most of the function of this class, ATM I am just playing with tool tips.
    [Export] String type = "none";
    [Export] float cost_base = 1;
    [Export] float cost_scale = .5f;
    [Export] float cycle_time = 10f;
    [Export] float cycle_profit = 1f;
    [Export] float unlock_cost = 0;

    public int count = 0;

    private GameOverMind overmind;

    private Button button;
    private Label count_text;

    //TODO this is linear scalling I want it to be exponetual at some point
    public float cost {
        get {
            return cost_base + cost_base * (cost_scale * count);
        }
    }

    private void update_tooptip() {
        button.HintTooltip = "add one " + type + "\ncost: " + cost;
    }

    public override void _Ready() {
        overmind = GetTree().Root.GetNode<GameOverMind>("Control");

        button = GetNode<Button>("Button");
        count_text = GetNode<Label>("Label");
        
        Visible = false;
        if (overmind.honey >= unlock_cost) {
            Visible = true;
        }

        update_tooptip();
    }
}

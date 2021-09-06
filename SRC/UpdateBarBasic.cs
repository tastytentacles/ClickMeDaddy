using Godot;
using System;

public class UpdateBarBasic : Panel {
    //TODO graphical representation of the thing in the timer doing a little animation of some kind.
    //TODO loading bar
    //TODO tool tip info about production
    //TODO tool tip flavor text
    
    [Export] String type = "none";
    [Export] float cost_base = 1;
    [Export] float cost_scale = .5f;
    [Export] float cycle_time = 10f;
    [Export] float cycle_profit = 1f;
    [Export] float unlock_cost = 0;
    [Export] int max_count = 100;

    public int count = 0;

    private GameOverMind overmind;

    private Button button;
    private Label count_text;
    private Timer timer;

    //TODO this is linear scalling I want it to be exponetual at some point
    public float cost {
        get {
            return cost_base + cost_base * (cost_scale * count);
        }
    }

    public float persec {
        get {
            return (cycle_profit / cycle_time) * count;
        }
    }

    private void update_tooptip() {
        button.HintTooltip = "add one " + type + "\ncost: " + cost;
    }

    private void button_pressed() {
        if (count < 100) {
            if (overmind.honey >= cost) {
                overmind.honey -= cost;
                count += 1;
                update_tooptip();
            }
            
        } else {
            UpdateBarBasic dupe = this.Duplicate() as UpdateBarBasic;
            dupe.count = 0;

            GetParent().AddChildBelowNode(this, dupe);
            button.Visible = false;
        }
    }

    private void timer_tick() {
        overmind.honey += cycle_profit * count;
    }

    public override void _Ready() {
        overmind = GetTree().Root.GetNode<GameOverMind>("Control");

        count_text = GetNode<Label>("Label");

        button = GetNode<Button>("Button");
        button.Connect("pressed", this, nameof(button_pressed));

        timer = GetNode<Timer>("Timer");
        timer.Connect("timeout", this, nameof(timer_tick));
        timer.Start(cycle_time);

        Visible = false;        
        update_tooptip();
    }

    public override void _Process(float delta) {
        if (overmind.honey >= unlock_cost) {
            Visible = true;
        }

        count_text.Text = "" + count;
    }
}

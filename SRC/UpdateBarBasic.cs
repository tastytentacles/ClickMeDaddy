using Godot;
using System;

public class UpdateBarBasic : Panel {
    //TODO graphical representation of the thing in the timer doing a little animation of some kind.
    //TODO loading bar
    //TODO tool tip info about production
    //TODO tool tip flavor text
    
    [Export] String type = "none";
    [Export] String tip_text = "fill me";
    [Export] float cost_base = 1;
    [Export] float cost_scale = .5f;
    [Export] float cycle_time = 10f;
    [Export] float cycle_profit = 1f;
    [Export] float unlock_cost = 0;
    [Export] int max_count = 100;

    public int count = 0;
    public int legacy_count = 0;

    private GameOverMind overmind;
    private bool start_lock = true;

    private Button button;
    private Label count_text;
    private Label name_text;
    private Timer timer;

    private ColorRect bar;
    private Tween tween;

    //TODO this is linear scalling I want it to be exponetual at some point
    public float cost {
        get {
            return cost_base + cost_base * (cost_scale * (count + legacy_count));
        }
    }

    public float persec {
        get {
            return (cycle_profit / cycle_time) * (count + legacy_count);
        }
    }

    private void update_tooptip() {
        button.HintTooltip = "add one " + type
        + "\ncost: " + cost
        + "\nproduces: " + cycle_profit + " every " + cycle_time + " seconds"
        + "\n\"" + tip_text + "\"";
    }

    private void button_pressed() {
        if (count < max_count) {
            if (overmind.honey >= cost) {
                overmind.honey -= cost;
                count += 1;
                
                if (start_lock) {
                    timer.Start(cycle_time);

                    tween.InterpolateProperty(
                        bar,
                        "rect_scale",
                        new Vector2(0, 1),
                        new Vector2(1, 1),
                        cycle_time
                    );
                    tween.Start();

                    start_lock = false;
                }
                
                update_tooptip();

                if (count == max_count) {
                    button.HintTooltip = "get one free plot of land for your " + type;
                }
            }
            
            
        } else {
            UpdateBarBasic dupe = this.Duplicate() as UpdateBarBasic;
            dupe.count = 0;
            dupe.legacy_count = count + legacy_count;

            Particles2D part = ResourceLoader.Load<PackedScene>("res://MenuSpace/SubParts/Partical.tscn").Instance<Particles2D>();
            part.Position = RectSize * new Vector2(.5f, 1);
            AddChild(part);

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
        name_text = GetNode<Label>("Label2");
        name_text.Text = type;

        button = GetNode<Button>("Button");
        button.Connect("pressed", this, nameof(button_pressed));

        timer = GetNode<Timer>("Timer");
        timer.Connect("timeout", this, nameof(timer_tick));
        timer.Start(cycle_time);

        tween = GetNode<Tween>("Tween");

        bar = GetNode<ColorRect>("bar");
        bar.RectScale = new Vector2(0, 1);

        Visible = false;        
        update_tooptip();
    }

    public override void _Process(float delta) {
        if (overmind.honey >= unlock_cost) {
            Visible = true;
        }

        count_text.Text = count + " / " + max_count;
    }
}

using Godot;
using System;

public class GameOverMind : Control {
    public float honey = 0;
    
    private TextureButton button;

    private Label honey_count;
    private Label persec_count;

    private float persec_calc() {
        float rtn = 0;
        foreach(UpdateBarBasic n in GetTree().GetNodesInGroup("Timer")) {
            rtn += n.persec;
        }

        return rtn;
    }

    private void button_pressed() {
        honey += 1;
    }

    public override void _Ready() {
        button = GetNode<TextureButton>("menu_stack/click_pain/vbox/big_button");
        button.Connect("pressed", this, nameof(button_pressed));

        honey_count = GetNode<Label>("menu_stack/click_pain/vbox/click_info_case/vbox/unit_count");
        persec_count = GetNode<Label>("menu_stack/click_pain/vbox/click_info_case/vbox/unit_per_sec");
    }

    public override void _Process(float delta) {
        honey_count.Text = honey + " Drams of Honey";
        persec_count.Text = persec_calc() + " Drams per second";
    }
}

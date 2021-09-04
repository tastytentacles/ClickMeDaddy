using Godot;
using System;

public class GameOverMind : Control {
    public float honey = 0;
    
    private TextureButton button;

    private Label honey_count;

    private void button_pressed() {
        honey += 1;
    }

    public override void _Ready() {
        button = GetNode<TextureButton>("menu_stack/click_pain/vbox/big_button");
        button.Connect("pressed", this, nameof(button_pressed));

        honey_count = GetNode<Label>("menu_stack/click_pain/vbox/click_info_case/vbox/unit_count");
    }

    public override void _Process(float delta) {
        honey_count.Text = honey + " Drams of Honey";
    }
}

using Godot;
using System;

public class HiveAnimator : Spatial {
    private Tween tween;
    private Node button;

    private void click_tween() {
        tween.StopAll();

        tween.InterpolateProperty(
            button,
            "scale",
            Vector3.One * .65f,
            Vector3.One * .6f,
            .5f);
        tween.Start();
    }

    public override void _Ready() {
        AnimationPlayer ap = GetNode<AnimationPlayer>("button_base/AnimationPlayer");
        ap.CurrentAnimation = "idle";
        ap.GetAnimation("idle").Loop = true;

        button = GetNode("button_base");
        tween = GetNode<Tween>("Tween");

        TextureButton tb = GetTree().Root.GetNode<TextureButton>("Control/menu_stack/click_pain/vbox/big_button");
        tb.Connect("pressed", this, nameof(click_tween));
    }
}

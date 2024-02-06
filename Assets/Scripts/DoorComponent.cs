using Godot;
using System;

public partial class DoorComponent : Area2D{

    [ExportCategory("Variables")]
    [Export]
    public Vector2 teleportPosition;
    //1200 27

    public Player player;
    public Sprite2D sprite;
    public AnimationPlayer animationPlayer;

    public override void _Ready(){
        sprite = GetNode<Sprite2D>("Sprite");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    private void OnBodyEntered(Player body){
        if (body is Player) 
        player = body;
        animationPlayer.Play("open");

    }

    private void OnAnimationFinished (String animationName){
        if (animationName == "open"){
        player.GlobalPosition = teleportPosition;
        animationPlayer.Play("close");
        }
    }
}

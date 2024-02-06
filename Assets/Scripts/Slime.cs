using Godot;
using System;

public partial class Slime : CharacterBody2D{
    public Sprite2D sprite;
    public AnimationPlayer animationPlayer;
    public Player player;
    public Vector2 direction;
    public float distance;
    public bool isDead = false;

    public override void _Ready(){
        sprite = GetNode<Sprite2D>("Sprite");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    private void OnBodyEntered(Node2D body){
        if (body is Player)
        player = (Player)body;
    }

    private void OnBodyExit(Node2D body){
        if (body is Player)
        player = null;
    }

    public override void _PhysicsProcess(double delta){
       if (isDead == true)
       return;

       Animation();
       Detection();
    }

    private void Detection(){
         if (player != null){
            if (player.isDead == true){
                Velocity = Vector2.Zero;
                MoveAndSlide();
                return;
            }

            direction = GlobalPosition.DirectionTo(player.GlobalPosition);
            distance = GlobalPosition.DistanceTo(player.GlobalPosition);

            if (distance < 15){
                player.Die();
            }
            Velocity = direction * 35;
            MoveAndSlide();
            return;
        }
        Velocity = Vector2.Zero;
    }

    private void Animation(){
        if (direction.X > 0)
            sprite.FlipH = false;

        if (direction.X < 0)
            sprite.FlipH = true;
        
        if (Velocity != Vector2.Zero){
            animationPlayer.Play("walk");
            return;
        }
        animationPlayer.Play("idle");
    }

    public void UpdateHealth(){
        isDead = true;
        animationPlayer.Play("death");
    }

    private void OnAnimationFinished(String animation){
        QueueFree();
    }
		
}

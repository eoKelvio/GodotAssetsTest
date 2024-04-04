using Godot;
using System;

public partial class Slime : Characters{
    public Sprite2D sprite;
    public Player player;
    public Vector2 direction;
    public float distance;
    public RandomNumberGenerator random = new RandomNumberGenerator();
    // public bool isDead = false;
    // public ProgressBar healthBar;
    

    public override void _Ready(){
        sprite = GetNode<Sprite2D>("Sprite");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        healthBar = GetNode<ProgressBar>("HealthBar");
        Health(5);        
    }

     public override void _PhysicsProcess(double delta){
       if (isDead == true)
       return;
       Animation();
       Detection();
       MonsterDie();
       MoveAndSlide();
    }

    private void OnBodyEntered(Node2D body){
        if (body is Player)
        player = (Player)body;
    }

    private void OnBodyExit(Node2D body){
        if (body is Player)
        player = null;
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

            // if (distance < 15){
            //     player.Die();
            // }
            Velocity = direction * 30;
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


    private void OnAnimationFinished(String animation){
        QueueFree();
    }

    

    private void OnAttackAreaBodyEntered(Characters body){

        if (body is Player){

		    body.DamageInPlayer(random.RandiRange(1,3));
            return;
		}
    }

    private void OnAttackAreaBodyExit (Characters body){

    }
}

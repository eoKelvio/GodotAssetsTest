using Godot;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;

public partial class Player : CharacterBody2D {

	public AnimationPlayer animationPlayer;
	public AnimationTree animationTree = null;
	public AnimationNodeStateMachinePlayback stateMachine;
	private Vector2 motion;
	private float speed = 64f;
	private float acceleration = 0.4f;
	private float friction = 0.2f;
	private string animation;
	private bool attacking = false;
	private Sprite2D sprite;
	private Timer attackTimer;
	private AnimationMixer animationMixer;
	public bool isDead = false;
	private Slime slime;
	

	public void GetInput(){
		motion = Input.GetVector("move_left", "move_right", "move_up", "move_down");
	
		if (motion != Vector2.Zero){
		animationTree.Set("parameters/idle/blend_position", motion);
		animationTree.Set("parameters/walk/blend_position", motion);
		animationTree.Set("parameters/attack/blend_position", motion);
		Velocity = Velocity.Lerp(motion * speed, acceleration);
		return;
		}
		
		Velocity = Velocity.Lerp(motion * speed, friction);
	}


	public override void _Ready(){
		sprite = GetNode<Sprite2D>("Sprite");
		attackTimer = GetNode<Timer>("AttackTimer");
		animationTree = GetNode<AnimationTree>("AnimationTree");
		stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
		animationMixer = GetNode<AnimationMixer>("AnimationTree");
		animationMixer.Active = true;
		AddToGroup("Player");
	}

	public override void _PhysicsProcess(double delta){
		if (isDead == true)
		return;

		GetInput();
		UpdateAnimation();
		Attack();
		MoveAndSlide();
	}


	private void Attack(){
		if (Input.IsActionJustPressed("attack") && attacking == false){
		attackTimer.Start();
		attacking = true;
		}
	}

	private void UpdateAnimation(){
		if (attacking == true){
			stateMachine.Travel("attack");
			return;
		}

		if (Velocity.Length() > 8){
			stateMachine.Travel("walk");
			return;
		}

		stateMachine.Travel("idle");
	}

	private void OnAttackTimer(){
		attacking = false;
	}

	public async void Die(){
		isDead = true;
		stateMachine.Travel("death");
		await ToSignal(GetTree().CreateTimer(1.0),"timeout");
		GetTree().ReloadCurrentScene();
	}

	
	private void OnAttackAreaBodyEntered(CharacterBody2D body){
		if (body is Slime){
			slime = (Slime)body;
			slime.UpdateHealth();
		}
	}
}
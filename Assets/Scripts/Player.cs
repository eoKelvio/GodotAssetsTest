using Godot;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;

public partial class Player : Characters {

	public AnimationTree animationTree = null;
	private Vector2 motion;
	private Vector2 idle;
	private Vector2 walk;
	private Vector2 attack;
	private float speed = 64f;
	private float acceleration = 0.4f;
	private float friction = 0.2f;
	private string animation;
	private bool attacking = false;
	private Sprite2D sprite;
	private Timer attackTimer;
	private Timer reginTimer;
	private AnimationMixer animationMixer;
	private Slime slime;
	public RandomNumberGenerator random = new RandomNumberGenerator();
	public CharacterBody2D enemy;
	public new double life;

	public override void _Ready(){
		healthBar = GetNode<ProgressBar>("HealthBar");
		sprite = GetNode<Sprite2D>("Sprite");
		attackTimer = GetNode<Timer>("AttackTimer");
		animationTree = GetNode<AnimationTree>("AnimationTree");
		stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
		animationMixer = GetNode<AnimationMixer>("AnimationTree");
		invulnerabilityTimer = GetNode<Timer>("InvulnerabilityTimer");
		reginTimer = GetNode<Timer>("ReginTimer");
		animationMixer.Active = true;
		AddToGroup("Player");
		Health(10);
		life = healthBar.Value;
	}

	public override void _PhysicsProcess(double delta){
		if (isDead == true)
		return;

		GetInput();
		UpdateAnimation();
		Attack();
		MoveAndSlide();
		PlayerDie();
		InvulnerabilityTimeOut();
	}

	public void GetInput(){

		motion = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		
		if (motion != Vector2.Zero){

			idle = (Vector2)animationTree.Get("parameters/idle/blend_position");
			
			animationTree.Set("parameters/idle/blend_position", motion);
			animationTree.Set("parameters/walk/blend_position", motion);

			if (Input.IsActionJustPressed("attack") && attacking == false)
			animationTree.Set("parameters/attack/blend_position", motion);

		Velocity = Velocity.Lerp(motion * speed, acceleration);
		return;
		}

		if (Input.IsActionJustPressed("attack") && attacking == false){
				if(attack != idle){
					animationTree.Set("parameters/attack/blend_position", idle);
				}
				animationTree.Set("parameters/attack/blend_position", idle);
			}
		

		Velocity = Velocity.Lerp(motion * speed, friction);
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

	

	
	private void OnAttackAreaBodyEntered(Characters body){
		if (body.IsInGroup("Enemy")){
			body.DamageInEnemies(random.RandiRange(1,2));
		}
	}

	private void InvulnerabilityTimeOut(){
		

		// if(life > healthBar.Value){
		// 	invulnerabilityTimer.Start();
		// 	GD.Print(life);
		// 	life = healthBar.Value;
		// }
	}


	// public void ReginTimeOut(int health){
	// 	healthBar.Value = health;

	// 	if (health < healthBar.Value){
	// 		health += 2;
	// 		if (health > healthBar.Value)
	// 		health = (int)healthBar.Value;
	// 	}
	// }

}
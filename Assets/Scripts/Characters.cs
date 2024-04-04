using Godot;
using System;

public partial class Characters : CharacterBody2D{

    public bool isDead = false;
    public int life;
    public ProgressBar healthBar;
    public AnimationPlayer animationPlayer;
    public AnimationNodeStateMachinePlayback stateMachine;
    public Timer invulnerabilityTimer;


    public virtual void Health(int health){
        healthBar.MaxValue = health;
        healthBar.Value = health;
        healthBar.Visible = false;
        }
    
    public virtual void DamageInEnemies(int damage){
        healthBar.Visible = true;
        healthBar.Value -= damage;
    }

    public virtual void DamageInPlayer(int damage){

        if(invulnerabilityTimer.IsStopped()){
        healthBar.Visible = true;
        healthBar.Value -= damage;
        invulnerabilityTimer.Start();
        }
    }


    public virtual void MonsterDie(){
		if (healthBar.Value <= healthBar.MinValue){
		isDead = true;
        animationPlayer.Play("death");
        }
}

    public async void PlayerDie(){
        if (healthBar.Value <= healthBar.MinValue){
		isDead = true;
		stateMachine.Travel("death");
		await ToSignal(GetTree().CreateTimer(1.0),"timeout");
		GetTree().ReloadCurrentScene();
        }
	}
}
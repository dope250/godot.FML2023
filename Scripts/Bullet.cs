using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export]public const float SPEED = 800.0f;
	public Vector2 area_direction = new Vector2(0,0);
	bool debounce = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.Translate(area_direction * SPEED * ((float)delta));		
	}

	private void _on_body_entered(Node2D body)
	{
		// Stops an error that crashes the game.
		if (debounce)
		{
			return;
		}
		debounce = true;
		// Make sure walls aren't destroyed!
		if (body.IsInGroup("Enemy"))
		{
			body.GetNode<Enemy>("").hit();
			//body.hit();
			this.hit();
		}

	}
	public void poof(){
		this.GetNode<GpuParticles2D>("Poof").Emitting = true;
		this.GetNode<AudioStreamPlayer2D>("Poof/Sound").Play();
		this.GetNode<GpuParticles2D>("Poof").Reparent(GetParent().GetParent());
		this.QueueFree();
	}
	public void hit(){
		this.GetNode<GpuParticles2D>("Hit").Emitting = true;
		this.GetNode<AudioStreamPlayer2D>("Hit/Sound").Play();
		this.GetNode<GpuParticles2D>("Hit").Reparent(GetParent().GetParent());
		this.QueueFree();
	}
}




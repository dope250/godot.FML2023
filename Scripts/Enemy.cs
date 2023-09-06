using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[ExportGroup("Stats")]
	[Export] float speed = 100.0f;
	[Export] int health = 1;

	[ExportGroup("Player Things")]
	[Export] string player_name = "Character";
	Node player;
	Node absolute_parent;
	[Export] int score_value = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		absolute_parent = GetParent();
		if (absolute_parent.GetNodeOrNull(player_name) != null)
		{
			player = absolute_parent.GetNode(player_name);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (player != null)
		{
			this.LookAt(((Vector2)player.Get("position")));
			this.Velocity = new Vector2(0, 0);
			this.Position.MoveToward(((Vector2)player.Get("position")), speed * ((float)delta));
		}
		this.MoveAndSlide();
	}
	public void hit()
	{
		health -= 1;
		if (health == 0)
		{
		absolute_parent.GetNode<ScoreScript>("Main Scene").Score += score_value;
		this.GetNode<GpuParticles2D>("Kill").Emitting = true;
		this.GetNode<AudioStreamPlayer2D>("Kill/Sound").Play();
		this.GetNode("Kill").Reparent(GetParent().GetParent());
		this.QueueFree();
		}
	}

	private void _on_area_detector_body_entered(Node2D body)
	{
		if (body.Name == player_name && (body.GetNode<CharacterMovement>(".").die != true))
		{
			body.GetNode<CharacterMovement>(".").Die();
		}
	}

}




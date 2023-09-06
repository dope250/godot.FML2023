using Godot;
using System;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;


public partial class CharacterMovement : CharacterBody2D
{
	const float SPEED = 300.0f;
	[Export] PackedScene Bullet;
	Camera2D Camera;
	[Export] double fire_rate = 0.2;
	double actual_rate = 0.2;
	double timer = 0;

	bool power = false;
	double power_timer = 0;
	Node absolute_parent;

	public bool die = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Camera = this.GetNode<Camera2D>("Camera2D");
		absolute_parent = this.GetParent();
		Camera.Set("position", new Vector2(100, 0));

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		timer += delta;
		if (power)
		{
			power_timer += delta;
			actual_rate = fire_rate / 2;
			if (power_timer >= 10)
			{
				power = false;
			}
		}
		else
		{
			actual_rate = fire_rate;
			power_timer = 0;
		}

		var direction_x = Input.GetAxis("Left", "Right");
		var direction_y = Input.GetAxis("Up", "Down");
		this.Velocity = new Vector2(0, 0);

		if (die)
		{
			if (Input.IsActionPressed("Respawn"))
			{
				Respawn();
			}
			return;
		}

		if (Input.IsActionPressed("Shoot") && timer >= actual_rate)
		{
			
			Vector2 SpawnPosition = GetNode<Node2D>("Gun/BulletSpawn").GlobalPosition;
			Bullet temp = (Bullet)Bullet.Instantiate();
			AddSibling(temp);
			temp.GlobalPosition = SpawnPosition;
			temp.area_direction = (GetGlobalMousePosition() - temp.GlobalPosition).Normalized();
			//temp.GetNode<Bullet>(".").area_direction = GetGlobalMousePosition() - this.GlobalPosition;
			temp.Rotation = GetGlobalMousePosition().AngleToPoint(GlobalPosition);
			

			Camera.Set("offset", new Vector2(GD.RandRange(-4, 4), GD.RandRange(-4, 4)));
			timer = 0;
		}
		else
		{
			Camera.Set("offset", new Vector2(0, 0));
		}
		if (direction_x != 0)
		{
			var velocity = this.Velocity;
			velocity.X = direction_x * SPEED;
			this.Velocity = velocity;
		}
		if (direction_y != 0)
		{
			var velocity = this.Velocity;
			velocity.Y = direction_y * SPEED;
			this.Velocity = velocity;
		}

		this.LookAt(GetGlobalMousePosition());
		MoveAndSlide();

	}


	public void Die()
	{
		GetNode<CpuParticles2D>("Explosive").Emitting = true;
		GetNode<AudioStreamPlayer>("Explosive/Sound").Play();
		this.GetNode<AnimatedSprite2D>("Player_Sprite").Visible = false;
		//this.GetNode<Sprite2D>("Decal_Body").Visible = true;
		this.GetNode<Sprite2D>("Gun").Visible = false;
		this.GetNode<AnimatedSprite2D>("Decal_Blood").Visible = true;
		this.GetNode<AnimatedSprite2D>("Decal_Blood").Play();
		Camera.Set("position", new Vector2(0, 0));
		die = true;
	}
	public void Respawn()
	{
		GetTree().ReloadCurrentScene();
	}
}

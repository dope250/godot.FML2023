using Godot;
using System;

public partial class ScoreScript : Node2D
{
	public int Score = 0;
	public int Coin = 0;
	public Node Background;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Background = GetNode<Node>("Background");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Background.GetNode<Label>("Score").Text = "Score: " + Score.ToString() + "\nCoins: " + Coin.ToString();
	}

}

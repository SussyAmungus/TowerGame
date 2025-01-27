using Godot;
using System;

public partial class TitleScreen : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Visible = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void OnButtonPressedPlay()
	{
		GetTree().Root.GetNode<MainScene>("MainScene").startGame();
		this.Visible = false;
		GD.Print("ASd");

	}

	public void OnButtonPressedCredits()
	{



	}

	public void OnButtonPressed3()
	{
		GetTree().Quit();

	}
}

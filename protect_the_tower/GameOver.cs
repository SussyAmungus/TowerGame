using Godot;
using System;

public partial class GameOver : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnButtonPressedRestart()
	{

		GetTree().Root.GetNode<MainScene>("MainScene").restartGame();
		this.Visible = false;
		GD.Print("Restarted");

	}


	public void OnButtonPressedMenu()
	{

		GetTree().ReloadCurrentScene();

	}

	public void OnButtonPressedQuit()
	{


		GetTree().Quit();

	}

}

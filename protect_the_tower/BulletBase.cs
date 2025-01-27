using Godot;
using System;

public partial class BulletBase : Node
{
	//We DONT EVEN USE THIS
	private double damage = 10;
	private String owner = "nil";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

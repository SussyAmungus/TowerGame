using Godot;
using System.Collections.Generic;
using System;

public partial class Turret : Node2D
{

	private Sprite2D TurretHead;
	private Sprite2D TurretBase;


	private bool canFire = true;

	public List<Bullet> myBullets = new List<Bullet>();


	private PackedScene bulletCopyer = ResourceLoader.Load<PackedScene>("res://Bullet.tscn");

	public override void _Ready()
	{

		TurretBase = this.GetNode<Sprite2D>("TurretBase");
		TurretHead = TurretBase.GetNode<Sprite2D>("TurretHead");
	}

	public override void _Input(InputEvent @event)
	{

		if (canFire == false) return;

		if (@event is InputEventMouseButton eventMouseButton)
		{

			if (eventMouseButton.IsPressed())
			{


				Bullet newBull = bulletCopyer.Instantiate() as Bullet;
				newBull.GiveRotation(TurretHead.Rotation + (float)(Mathf.Pi));
				newBull.Position = TurretHead.GlobalPosition;
				GetTree().Root.GetNode("MainScene").AddChild(newBull);
				newBull.setOwner("player");

				myBullets.Add(newBull);

			}

			if (eventMouseButton.IsPressed())
			{

				//Godot.Collections.Dictionary result = GetWorld2D().DirectSpaceState.IntersectRay(PhysicsRayQueryParameters2D.Create(GetGlobalMousePosition(), GetGlobalMousePosition() - new Vector2(0.0001f, 0.0001f)));


			}



		}


		//base._Input(@event);
	}



	public void turnColor(Color cc)
	{
		GetNode<Sprite2D>("TurretBase").Modulate = cc;

	}

	public void removeColor()
	{

		GetNode<Sprite2D>("TurretBase").Modulate = GetNode<Sprite2D>("TurretBase").SelfModulate;

	}


	public void stopFire()
	{

		canFire = false;

	}


	public void startFire()
	{
		canFire = true;


	}




	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		Vector2 mousePos = GetLocalMousePosition();

		Vector2 AngleVect = TurretHead.Position - mousePos;

		TurretHead.Rotation = AngleVect.Angle();

	}
}

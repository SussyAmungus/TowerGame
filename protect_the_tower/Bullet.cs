using Godot;
using System;

public partial class Bullet : Node2D
{
	// Called when the node enters the scene tree for the first time.


	private float rot = 0;

	private float speed = 250;

	private double damage = 10;

	//defualt set to plane
	private string ownership = "enemies";


	private Node2D TowerTarget;

	public override void _Ready()
	{


	}
	public void GiveRotation(float rote)
	{

		rot = rote;
		this.Rotation = rot + Mathf.Pi / 2;

	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		moveBullet(delta);
		//WIll have probelms later with list stuff // will fix later

		if (this.Position.X >= GetViewport().GetVisibleRect().Size.X || this.Position.X <= 0)
		{

			this.QueueFree();
		}
		if (this.Position.Y >= GetViewport().GetVisibleRect().Size.Y || this.Position.Y <= 0)
		{

			this.QueueFree();
		}

	}


	public void moveBullet(double delta)
	{

		Vector2 ee = Vector2.FromAngle(rot);

		this.Position = this.Position + Vector2.FromAngle(rot) * speed * (float)delta;

	}

	public string getOwner()
	{


		return ownership;
	}

	public void setDamage(double newDag)
	{

		damage = newDag;
	}

	public double getDamage()
	{

		return damage;
	}


	public void setTarget(Node2D temp)
	{

		TowerTarget = temp;

	}

	public Node2D getTarget()
	{

		return TowerTarget;
	}

	public void PrintTargetID()
	{

		if (TowerTarget is Collum)
		{

			GD.Print(((Collum)TowerTarget).ID);
		}

		if (TowerTarget is Beamer)
		{

			GD.Print(((Beamer)TowerTarget).ID);
		}



	}

	public void setOwner(String newOwner)
	{


		ownership = newOwner;
	}





}

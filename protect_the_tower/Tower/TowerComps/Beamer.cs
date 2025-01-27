using Godot;
using System;
using System.Collections.Generic;

public partial class Beamer : Node2D
{

	private double defultsupportCanGive;
	private double defultsupportNeeded;
	private double defulttotalHealth;


	//like its the defult lol
	public double supportCanGive = 25;

	//amount of support it can give to others
	public double supportGiven = 25;


	//needed support
	public double supportNeeded = 50;


	//total support it has
	public double supportHad = 0;

	//potential health
	public double totalHealth = 100;
	public double health = 100;


	public StructureNode nodeLeft;

	public StructureNode nodeRight;

	public String ID;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Sprite2D>("Sprite2D").GetNode<Area2D>("Area2D").Connect("area_entered", new Callable(this, "onHit"));
		nodeLeft = new StructureNode(this);

		nodeRight = new StructureNode(this);
		SaveDefults();

	}

	public void repairIt()
	{

		respawn();
	}

	public double repairCost()
	{



		return totalHealth * 3;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{


	}


	public void SaveDefults()
	{

		defultsupportCanGive = supportCanGive;
		defultsupportNeeded = supportGiven;
		defulttotalHealth = totalHealth;

	}

	public void resetIt()
	{

		supportCanGive = defultsupportCanGive;
		supportGiven = defultsupportNeeded;
		totalHealth = defulttotalHealth;

		//some code inside is redundent asf
		respawn();


	}

	public void setID(String i)
	{


		ID = i;
	}

	public String returnAttachedStr()
	{


		return "Nodes left: " + nodeLeft.getAttachedString() + " " + "Nodes right: " + nodeRight.getAttachedString();
	}

	public Rect2 getGlobalRect()
	{
		//rediculus solution
		return new Rect2(this.GlobalPosition, new Vector2(30, 10));

	}

	public void onHit(Area2D other)
	{



		if (other.GetParent().GetParent() is Bullet)
		{

			if (other.GetParent().GetParent<Bullet>().getOwner() == "plane" && other.GetParent().GetParent<Bullet>().getTarget().Equals(this))
			{

				gotDamaged(other.GetParent().GetParent<Bullet>().getDamage());
				other.GetParent().GetParent<Bullet>().QueueFree();

			}


		}

	}

	public void gotDamaged(double dam)
	{
		this.health = this.health - dam;
		supportGiven = (health / totalHealth) * supportCanGive;


		if (this.health <= 0)
		{

			Destroy();
		}
	}

	public void reCalc()
	{


		supportHad = nodeLeft.getSupport() + nodeRight.getSupport();

		if (supportHad < supportNeeded)
		{
			Destroy();


		}
		Color temp = this.GetNode<Sprite2D>("Sprite2D").SelfModulate;

		this.GetNode<Sprite2D>("Sprite2D").Modulate = temp.Lerp(Colors.Red, 1 - ((float)(this.health / this.totalHealth)));




	}


	public void Destroy()
	{
		health = 0;
		supportGiven = 0;
		this.GetNode<Sprite2D>("Sprite2D").Modulate = Colors.Red;

	}


	public void respawn()
	{
		health = totalHealth;
		supportGiven = supportCanGive;
		this.GetNode<Sprite2D>("Sprite2D").Modulate = this.GetNode<Sprite2D>("Sprite2D").SelfModulate;



	}

	public string detailsStr()
	{
		return "Health: " + this.health + "\nSupport can Give: " + this.supportCanGive + "\nSupport Got: " + this.supportHad + "\n Support Neeced: " + this.supportNeeded;
	}

}

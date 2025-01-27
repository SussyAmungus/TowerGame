using Godot;
using System;

public partial class Collum : Node2D
{

	private double defultsupportCanGive;
	private double defultsupportNeeded;
	private double defulttotalHealth;

	public double supportCanGive = 25;
	public double supportGiven = 0;

	public double supportNeeded = 50;

	public double supportHad = 0;


	public double totalHealth = 100;
	public double health = 100;

	public StructureNode nodeUp;

	public StructureNode nodeDown;

	public String ID = "Nish";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		supportGiven = supportCanGive;
		GetNode<Sprite2D>("Sprite2D").GetNode<Area2D>("Area2D").Connect("area_entered", new Callable(this, "onHit"));
		nodeUp = new StructureNode(this);

		nodeDown = new StructureNode(this);
		SaveDefults();

	}

	public void repairIt()
	{

		respawn();
		GD.Print("repaired");

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


		return "Nodes up: " + nodeUp.getAttachedString() + " " + "Nodes down: " + nodeDown.getAttachedString();
	}

	internal Rect2 getGlobalRect()
	{
		return new Rect2(this.GlobalPosition, new Vector2(10, 30));
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

		supportHad = nodeUp.getSupport() + nodeDown.getSupport();




		if (supportHad < supportNeeded)
		{
			Destroy();


		}

		Color temp = this.GetNode<Sprite2D>("Sprite2D").SelfModulate;

		this.GetNode<Sprite2D>("Sprite2D").Modulate = temp.Lerp(Colors.Red, 1 - ((float)(this.health / this.totalHealth)));


		GetTree().Root.GetNode<MainScene>("MainScene").GetNode<BuildMenu>("BuildMenu").showDetails();

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

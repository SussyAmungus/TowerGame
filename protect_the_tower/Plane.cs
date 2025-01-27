using Godot;
using System.Collections.Generic;
using System;

public partial class Plane : Node2D
{


	//Probably useless but idk
	private int state = 0; //1 is the normal state probably remove later

	private float TowerAreaLevel = 0; //-1 to 1 ONLY Percentage of where the plane will be.

	private int moveDirection = 1; //Only 1 or -1 // 0 will make it stop I think

	private double range = 300; //How far left and right the john can go


	public double health = 50;


	private double fireTime = 1.5;

	private double totalishTime = 0;

	//like going towards left or right
	//left right and still
	private string movement = "still";

	private double speed = 250;
	private Tower Towerr;

	public List<Bullet> PlaneBullets = new List<Bullet>();

	private PackedScene bulletCopyer = ResourceLoader.Load<PackedScene>("res://Bullet.tscn");


	public override void _Ready()
	{


		getTowerTarget();//MUST HAPPEN FIRST FOR SHIT TO BE SWEET
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").GetNode<Area2D>("Area2D").Connect("area_entered", new Callable(this, "onHit"));
		spawnGen();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (state == 1)
		{

			movePlane(delta);

		}



		if (state == 0)
		{

			getToPosition(delta);




		}


		totalishTime = totalishTime + delta;

		if (totalishTime > fireTime)
		{

			FireAtTarget();
			totalishTime = 0;
		}



	}



	public void getToPosition(double delta)
	{

		float desY = (-1) * TowerAreaLevel * ((float)Towerr.sizeY / 2) + Towerr.Position.Y;

		this.Rotation = 0;

		Vector2 desrired = new Vector2(0, 0);
		if (movement == "left")
		{

			desrired = new Vector2(Towerr.Position.X + (float)range, desY);
		}
		else if (movement == "right")
		{

			desrired = new Vector2(Towerr.Position.X - (float)range, desY);
		}


		float rad = (desrired - this.Position).Angle();

		this.Rotation = rad;


		this.Position = this.Position + new Vector2(Vector2.FromAngle(rad).X, Vector2.FromAngle(rad).Y) * 500 * (float)delta;


		//Lowky bad system ix later
		if (this.Position.DistanceTo(desrired) < 2)
		{
			state = 1;
		}

	}

	/// <summary>
	/// Not Really used
	/// </summary>
	public void Fire()
	{

		Bullet newBull = bulletCopyer.Instantiate() as Bullet;
		newBull.GiveRotation((this.Position - getTowerShotSpot()).Angle() + (float)(Mathf.Pi));
		newBull.Position = this.GlobalPosition;
		PlaneBullets.Add(newBull);
		GetTree().Root.GetNode("MainScene").AddChild(newBull);
	}


	public void FireAtTarget()
	{
		Bullet newBull = bulletCopyer.Instantiate() as Bullet;
		newBull.setOwner("plane");
		Node2D tar = getTarget();
		newBull.setTarget(tar);
		newBull.GiveRotation((this.Position - tar.GlobalPosition).Angle() + (float)(Mathf.Pi));
		newBull.Position = this.GlobalPosition;
		PlaneBullets.Add(newBull);
		GetTree().Root.GetNode("MainScene").AddChild(newBull);

	}


	public void getTowerTarget()
	{
		try
		{

			Towerr = GetTree().Root.GetNode("MainScene").GetNode<Tower>("Tower");

		}
		catch (Exception e)
		{


			GD.PushWarning("Bro is not in the world scene" + e);
		}


	}


	public void movePlane(double delta)
	{
		if (movement == "right")
		{

			this.Scale = new Vector2(-1, 1);
		}
		else if (movement == "left")
		{
			this.Scale = new Vector2(1, 1);

		}


		this.Rotation = 0;

		float accualY = TowerAreaLevel * Towerr.Scale.Y;
		//this is where the flip happens
		if (this.Position.X >= Towerr.Position.X + range)
		{
			moveDirection = -1;
			movement = "left";
		}
		else if (this.Position.X <= Towerr.Position.X - range)
		{
			moveDirection = 1;
			movement = "right";
		}

		float desY = (-1) * TowerAreaLevel * ((float)Towerr.sizeY / 2) + Towerr.Position.Y;

		this.Position = new Vector2((float)((double)this.Position.X + (delta * speed * moveDirection)), desY);
	}

	/// <summary>
	/// Deprecated
	/// </summary>
	/// <returns> Area to shoot</returns>
	public Vector2 getTowerShotSpot()
	{
		//0 to 1 btw

		int rand = GD.RandRange(-1, 1);
		float randomSection;
		if (rand >= 0)
		{

			randomSection = GD.Randf();
		}
		else
		{

			randomSection = GD.Randf() * -1;
		}


		return new Vector2(Towerr.Position.X, (-1) * randomSection * ((float)Towerr.sizeY / 2) + Towerr.Position.Y);

	}

	public Node2D getTarget()
	{

		return Towerr.getRandomStruct();
	}

	public void onHit(Area2D other)
	{
		if (other.GetParent().GetParent() is Bullet)
		{

			if (other.GetParent().GetParent<Bullet>().getOwner() == "player")
			{

				onDamaged(other.GetParent().GetParent<Bullet>().getDamage());

				other.GetParent().GetParent<Bullet>().QueueFree();
			}


		}

	}


	public void onDamaged(double dam)
	{
		this.health = this.health - dam;

		if (this.health <= 0)
		{
			onKilled();
			this.QueueFree();
		}


	}

	public void onKilled()
	{

		GetTree().Root.GetNode<MainScene>("MainScene").addMoney(100);
		GetTree().Root.GetNode<MainScene>("MainScene").onDestroyedPlane(this);
	}


	//Designed for the spawning of plane
	public void spawnGen()
	{
		movement = "right";
		TowerAreaLevel = GD.Randf() * 1 - 0.1f;
		if (GD.Randf() > 0.5)
		{
			movement = "left";
			TowerAreaLevel = GD.Randf() * -1 + 0.1f;

		}



		int X = -20;
		if (GD.Randf() > 0.5)
		{
			this.ApplyScale(new Vector2(-1, -1));
			X = (int)GetViewport().GetVisibleRect().Size.X + 20;

		}
		else
		{
			this.ApplyScale(new Vector2(-1, 1));

		}



		int Y = GD.RandRange(0, (int)GetViewport().GetVisibleRect().Size.Y / 2);



		this.Position = new Vector2(X, Y);




	}
}

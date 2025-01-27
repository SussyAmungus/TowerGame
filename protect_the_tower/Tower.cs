using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Tower : Node2D
{
	//never gets used.
	public double health { get; set; }


	//if it gets dleteed who cares.
	private Turret TurretRoot;

	private PackedScene beamCopyer = ResourceLoader.Load<PackedScene>("res://Tower/TowerComps/beamer.tscn");

	private PackedScene collumCopyer = ResourceLoader.Load<PackedScene>("res://Tower/TowerComps/collum.tscn");

	private PackedScene windowCopyer = ResourceLoader.Load<PackedScene>("res://Window.tscn");



	public List<Beamer> beamsList;

	public List<Collum> collumsList;

	public List<Window> windowsList;


	public List<Turret> turretList;

	//basdcialy padding for the beam 
	private float bufferBeam = 25;


	//other properties
	private float BeamHorz = 100;
	private float CollumVert = 200;

	//size
	//Still not okay btw
	public double sizeX = 0;
	public double sizeY = 0;


	//Techincally inside the Tower scene since root is its container
	private Vector2 startBuildPos;

	//Need a total x and y size eventualy, sooner than later


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		startBuildPos = new Vector2(-200, -200);

		beamsList = new List<Beamer>();
		collumsList = new List<Collum>();

		turretList = new List<Turret>();

		windowsList = new List<Window>();



		TurretRoot = this.GetNode<Turret>("TurretRoot");
		turretList.Add(TurretRoot);

		//Need to get better offsets and such
		buildTower(10, 5, 0.30f, 0.60f, 0f, 0f);
		Algorithm(10);

		//This is the debugger
		//showIDTree();

		ToggleOverLay();

	}

	public void stopTurretsFire()
	{

		for (int i = 0; i < turretList.Count; i++)
		{
			turretList[i].stopFire();
		}

	}

	public void startTurretsFire()
	{

		for (int i = 0; i < turretList.Count; i++)
		{
			turretList[i].startFire();
		}

	}


	public void resetAllUpgrades()
	{

		for (int i = 0; i < beamsList.Count; i++)
		{
			beamsList[i].resetIt();

		}

		for (int i = 0; i < collumsList.Count; i++)
		{
			collumsList[i].resetIt();

		}
		for (int i = 0; i < turretList.Count; i++)
		{

			if (i == 0) continue;
			turretList[i].QueueFree();

		}
	}

	public bool isCompletelyDestroyed()
	{
		for (int i = 0; i < beamsList.Count; i++)
		{

			if (beamsList[i].health >= 1)
			{

				return false;
			}


		}

		for (int i = 0; i < collumsList.Count; i++)
		{
			if (collumsList[i].health >= 1)
			{

				return false;
			}

		}

		return true;

	}


	public void PhysicsCheck()
	{

		for (int i = 0; i < beamsList.Count; i++)
		{
			beamsList[i].reCalc();

		}

		for (int i = 0; i < collumsList.Count; i++)
		{
			collumsList[i].reCalc();

		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		PhysicsCheck();

	}


	public Node2D getRandomStruct()
	{

		List<Node2D> allStruct = beamsList.Cast<Node2D>().ToList<Node2D>();

		allStruct.AddRange(collumsList);

		int index = (int)GD.RandRange(0, allStruct.Count() - 1);
		return allStruct[index];


	}
	//he


	public void buildTower(int levels, int collumsPerRow, float towersizeX, float towersizeY, float stX, float stY)
	{
		int IDNumColl = 1;
		int IDNumBeam = 1;

		float startY = stX;

		//up and down
		for (int j = 0; j < levels; j++)
		{
			float startX = stY;
			//left and right //wich is first in build
			for (int i = 0; i < collumsPerRow; i++)
			{


				Collum newColl = collumCopyer.Instantiate() as Collum;
				newColl.setID("Coll ID: " + IDNumColl);
				this.AddChild(newColl);
				newColl.Scale = new Vector2(towersizeX, towersizeY);
				newColl.Position = new Vector2(startX, startY);
				collumsList.Add(newColl);

				IDNumColl++;

				if (i != collumsPerRow - 1)
				{
					Beamer newBeam = beamCopyer.Instantiate() as Beamer;
					newBeam.setID("Beam ID: " + IDNumBeam);
					this.AddChild(newBeam);
					newBeam.Position = new Vector2(startX, startY);
					newBeam.Scale = new Vector2(towersizeX, towersizeY);



					beamsList.Add(newBeam);

					IDNumBeam++;



				}



				//Allitle bit better method but still prod code
				startX = startX + beamsList[0].GetNode<Sprite2D>("Sprite2D").Texture.GetSize().X * towersizeX;

				sizeX = sizeX + beamsList[0].GetNode<Sprite2D>("Sprite2D").Texture.GetSize().X * towersizeX;


				//constructs the window ovelay



			}
			//The rotation of the collum is made where X is the Y axis and vise versa
			startY = startY + collumsList[0].GetNode<Sprite2D>("Sprite2D").Texture.GetSize().Y * towersizeY;
			sizeY = sizeY + collumsList[0].GetNode<Sprite2D>("Sprite2D").Texture.GetSize().Y * towersizeY;
		}

		for (int i = 0; i < beamsList.Count; i++)
		{


			Window windowTemp = windowCopyer.Instantiate() as Window;
			windowsList.Add(windowTemp);
			this.AddChild(windowTemp);
			windowTemp.Position = beamsList[i].Position;
			windowTemp.Scale = new Vector2(0.4f, 0.5f);

		}



	}

	public void Algorithm(int levels)
	{


		int BeamsPerRows = beamsList.Count / levels;

		int CollumsPerRow = collumsList.Count / levels;

		//For now it will manually add the johns but a better solution involing the adding support is inbound via its own method call to add the support then it adds them

		//Toppest beams
		for (int i = 0; i < BeamsPerRows; i++)
		{
			beamsList[i].nodeLeft.addSupport(collumsList[i].nodeUp);

			beamsList[i].nodeRight.addSupport(collumsList[i + 1].nodeUp);
		}

		int additional = 0;

		//Might be an issue on where it starts 
		for (int i = BeamsPerRows; i < beamsList.Count; i++)
		{



			if (i % BeamsPerRows == 0)
			{
				additional++;

			}
			//Bottom of beams
			beamsList[i].nodeLeft.addSupport(collumsList[i + additional].nodeUp);
			beamsList[i].nodeRight.addSupport(collumsList[i + additional + 1].nodeUp);
			//Tops of beams
			beamsList[i].nodeLeft.addSupport(collumsList[i - CollumsPerRow + additional].nodeDown);
			beamsList[i].nodeRight.addSupport(collumsList[i - CollumsPerRow + additional + 1].nodeDown);

		}

		//All collums attached to eachother
		for (int i = 0; i < collumsList.Count - CollumsPerRow; i++)
		{
			collumsList[i].nodeDown.addSupport(collumsList[i + CollumsPerRow].nodeUp);

		}

	}

	public void showIDTree()
	{



		for (int i = 0; i < beamsList.Count; i++)
		{

			GD.Print(beamsList[i].ID + " " + beamsList[i].returnAttachedStr());

		}

		for (int i = 0; i < collumsList.Count; i++)
		{

			GD.Print(collumsList[i].ID + " " + collumsList[i].returnAttachedStr());

		}

	}

	public void ToggleOverLay()
	{


		for (int i = 0; i < beamsList.Count; i++)
		{

			beamsList[i].GetNode<Sprite2D>("Sprite2DOver").Visible = true;







		}

		for (int i = 0; i < collumsList.Count; i++)
		{

			collumsList[i].GetNode<Sprite2D>("Sprite2DOver").Visible = true;

		}

		for (int i = 0; i < windowsList.Count; i++)
		{

			windowsList[i].Visible = true;

		}



	}

	public void DeleteOverLay()
	{
		for (int i = 0; i < beamsList.Count; i++)
		{


			beamsList[i].GetNode<Sprite2D>("Sprite2DOver").Visible = false;


		}

		for (int i = 0; i < collumsList.Count; i++)
		{

			collumsList[i].GetNode<Sprite2D>("Sprite2DOver").Visible = false;

		}

		for (int i = 0; i < windowsList.Count; i++)
		{

			windowsList[i].Visible = false;

		}
	}

	public void buildTurret(Turret ee)
	{
		turretList.Add(ee);
	}

}

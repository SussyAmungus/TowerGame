using Godot;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using System.Text.Json;
using System.Threading;

public partial class MainScene : Node2D
{
	public List<Level> LevelList;

	//private Json roundLevels;

	Godot.Collections.Dictionary loadedLevelDict;

	private PackedScene PlanePack = ResourceLoader.Load<PackedScene>("res://Plane.tscn");

	private int CurRound;

	public Control MoneyGUI;

	public double Money;
	public List<Plane> currentPlanes;

	public bool paused = true;

	double time = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		MoneyGUI = GetNode<Control>("MoneyLabel");
		Money = 0;
		addMoney(10000);

		CurRound = 0;
		currentPlanes = new List<Plane>();
		LevelList = new List<Level>();


		//string path = Path.Join("res://Rounds/Levels.json", "Levels.json");
		string loadedData = File.ReadAllText("C:/Users/Leopold/Documents/protect_the_tower/Rounds/Levels.json");

		Json jsonLoader = new Json();

		Error error = jsonLoader.Parse(loadedData);

		if (error != Error.Ok)
		{

			GD.Print(error);
			return;
		}


		loadedLevelDict = (Godot.Collections.Dictionary)jsonLoader.Data;

		Godot.Collections.Array arr1 = loadedLevelDict["levels"].AsGodotArray();

		for (int index = 0; index < arr1.Count; index++)
		{

			Level temp = new Level((int)arr1[index].AsGodotDictionary()["levelName"], (int)arr1[index].AsGodotDictionary()["enemies"]);

			LevelList.Add(temp);



		}

		//Pre initialization done

		//spawn first round

		GetTree().Paused = true;

	}
	//idk


	public void addMoney(double moneyer)
	{

		Money = Money + moneyer;
		MoneyGUI.GetNode<RichTextLabel>("RichTextLabel").Text = "$" + Money;
		//Do some external function
	}

	public void spendMoney(double moneyer)
	{

		Money = Money - moneyer;
		MoneyGUI.GetNode<RichTextLabel>("RichTextLabel").Text = "$" + Money;
		GetNode<Message>("Message").putMessage("-" + moneyer, new Color(1f, 0f, 0f, 1f));
		//Do some external function
	}

	public void showMessage(String str, Color cc)
	{


		GetNode<Message>("Message").putMessage(str, cc);
	}

	public void startGame()
	{

		GetTree().Paused = false;

	}

	public void restartGame()
	{


		setDefults();
		GetTree().Paused = false;
		this.GetNode<Tower>("Tower").startTurretsFire();
	}


	public void setDefults()
	{

		spendMoney(Money);
		addMoney(100);
		this.GetNode<Tower>("Tower").resetAllUpgrades();
		removeAlPlanes();
		setRound(0);
	}

	public void removeAlPlanes()
	{

		for (int i = 0; i < currentPlanes.Count; i++)
		{

			currentPlanes[i].QueueFree();

		}

	}

	public void setRound(int number)
	{

		CurRound = number;

	}



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{



		upDate();
		time = time + delta;
		//This casues issues ngl need a better method //conditions for a new round
		if (currentPlanes.Count() == 0 && CurRound < 2 && time > 1.5)
		{


			CurRound++;



		}

		if (this.GetNode<Tower>("Tower").isCompletelyDestroyed())
		{
			showGameOverScreen();

		}


	}

	public void showGameOverScreen()
	{
		this.GetNode<Tower>("Tower").stopTurretsFire();
		this.GetNode<GameOver>("GameOver").Visible = true;

	}



	public void upDate()
	{

		//for now let it spawn after time equals that


		if (LevelList[CurRound].PlaneSpawnReached() == false)
		{
			//dangerous game ngl
			if (time >= 1)
			{
				time = 0;
				spawnPlane();
				LevelList[CurRound].UpdatePlaneSpawned();
			}




		}
	}



	public void spawnRound(Level le)
	{

		for (int i = 0; i < le.enemies; i++)
		{

			spawnPlane();


		}

	}







	public void spawnPlane()
	{
		Plane p = PlanePack.Instantiate() as Plane;



		//GetTree().Root.c("add_child,p");

		this.CallDeferred("add_child", p);

		currentPlanes.Add(p);

		//p.doMainRunSequence();

	}

	//Probaly gonna make an emitter for this john tbh
	public void onDestroyedPlane(Plane planeDestroyed)
	{

		currentPlanes.Remove(planeDestroyed);
		GD.Print(currentPlanes.Count());

	}



}

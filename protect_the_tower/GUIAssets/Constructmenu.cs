using Godot;
using System;
using System.IO;

public partial class Constructmenu : Control
{


	private PackedScene CopyBuildOption = ResourceLoader.Load<PackedScene>("res://GUIAssets/buildComp.tscn");
	private Godot.Collections.Dictionary loadedBuildDict;


	private bool canPlace = false;

	private Node2D currentlySelected;

	private PackedScene CopyTurret = ResourceLoader.Load<PackedScene>("res://Turret.tscn");
	private PackedScene DropPack = ResourceLoader.Load<PackedScene>("res://DropBeam.tscn");

	private DropBeam dp;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Visible = false;
		setUpBuilds();
		//for now its all generated on fly
		propListOfAll();


		dp = DropPack.Instantiate() as DropBeam;
		GetTree().Root.CallDeferred("add_child", dp);
	}

	public void setUpBuilds()
	{

		string loadedData = File.ReadAllText("C:/Users/Leopold/Documents/protect_the_tower/Builds/Build.json");

		Json jsonLoader = new Json();
		Error error = jsonLoader.Parse(loadedData);
		if (error != Error.Ok)
		{
			GD.Print(error);
			return;
		}
		loadedBuildDict = (Godot.Collections.Dictionary)jsonLoader.Data;


	}


	public void toggleShow()
	{

		if (this.Visible == false)
		{

			this.Visible = true;

		}
		else
		{

			this.Visible = false;
		}
	}

	public void propListOfAll()
	{


		Godot.Collections.Array Type = loadedBuildDict["builds"].AsGodotArray();

		for (int i = 0; i < Type.Count; i++)
		{

			BuildComp newBuy = CopyBuildOption.Instantiate() as BuildComp;

			newBuy.Readyer(Type[i].AsGodotDictionary(), Type[i].AsGodotDictionary()["Type"].ToString());


			this.GetNode<PanelContainer>("PanelContainer").GetNode<VBoxContainer>("VBoxContainer").AddChild(newBuy);
		}

	}






	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		followCreation(currentlySelected);




	}



	public override void _Input(InputEvent @event)
	{

		if (@event is InputEventMouseButton eventMouseButton)
		{

			if (eventMouseButton.IsReleased() && eventMouseButton.ButtonIndex == MouseButton.Left)
			{



				placeThing(currentlySelected);




			}
		}
	}



	public void followCreation(Node2D temp)
	{

		if (temp == null)
		{
			dp.Visible = false;

			return;
		}


		dp.Visible = true;

		dp.Position = GetGlobalMousePosition();

		if (dp.GetNode<RayCast2D>("RayCast2D").IsColliding())
		{

			if (temp is Turret)
			{

				((Turret)temp).turnColor(Colors.Green);
				canPlace = true;
			}


			temp.Position = dp.GetNode<RayCast2D>("RayCast2D").GetCollisionPoint();
			temp.Visible = true;
		}
		else
		{



			if (temp is Turret)
			{

				((Turret)temp).turnColor(Colors.Red);
				canPlace = false;
			}


			temp.Visible = false;

		}




	}


	public void bindSelect(Node2D ee)
	{

		currentlySelected = ee;


	}

	public Node2D CreateTurret()
	{

		Turret turret = CopyTurret.Instantiate() as Turret;

		GetTree().Root.GetNode<MainScene>("MainScene").AddChild(turret);

		return turret;

	}
	//some error 1/24/2025 when placing turret wrong
	public void placeThing(Node2D ee)
	{

		if (canPlace == false)
		{

			currentlySelected = null;

			return;
		}



		if (currentlySelected == null) return;

		ee.Visible = true;
		GD.Print("dropped");
		currentlySelected = null;

		if (ee is Turret)
		{

			((Turret)ee).removeColor();

			GetTree().Root.GetNode<MainScene>("MainScene").GetNode<Tower>("Tower").buildTurret((Turret)ee);

			canPlace = false;
		}


	}

}

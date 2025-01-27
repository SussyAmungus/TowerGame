using Godot;
using Godot.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public partial class BuildMenu : Control
{

	public Node2D selected = null;

	private PackedScene CopyBuyOption = ResourceLoader.Load<PackedScene>("res://GUIAssets/BuyOption.tscn");
	private Dictionary loadedUpgradeDict;


	private bool isDetailsPoped = false;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{




		this.Visible = false;
		setUpGrades();
	}




	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{




	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey eventKey)
		{
			if (eventKey.Pressed && eventKey.Keycode == Key.F)
			{
				toggleShow();
				//Bruh also doing the fucken Construction menu

				GetTree().Root.GetNode<MainScene>("MainScene").GetNode<Constructmenu>("ConstructMenu").toggleShow();

			}
			else if (eventKey.Pressed && eventKey.Keycode == Key.R)
			{

				if (selected != null)
				{


					repairer();
				}

			}
		}

		if (@event is InputEventMouseButton eventMouseButton)
		{

			if (eventMouseButton.IsPressed())
			{

				if (ObjectUnderMouse(GetGlobalMousePosition()) is Beamer)
				{

					deleteList();
					selected = ObjectUnderMouse(GetGlobalMousePosition());
					GD.Print("GOT BEAM");

					//needed
					propList("beamUpgrades");
					showTopbar("Beam", ((Beamer)selected).repairCost().ToString(), ((Beamer)selected).detailsStr());

				}
				else if (ObjectUnderMouse(GetGlobalMousePosition()) is Collum)
				{

					deleteList();
					selected = ObjectUnderMouse(GetGlobalMousePosition());
					GD.Print("GOT Collum");

					//needed
					propList("collumUpgrades");
					showTopbar("Collum", ((Collum)selected).repairCost().ToString(), ((Collum)selected).detailsStr());
				}
				else
				{

					deleteList();
					closeTopbar();
					removeDetails();
					selected = null;
				}




			}
		}
	}

	public void setUpGrades()
	{

		string loadedData = File.ReadAllText("C:/Users/Leopold/Documents/protect_the_tower/.godot/Upgrades/Upgrade.json");

		Json jsonLoader = new Json();
		Error error = jsonLoader.Parse(loadedData);
		if (error != Error.Ok)
		{
			GD.Print(error);
			return;
		}
		loadedUpgradeDict = (Godot.Collections.Dictionary)jsonLoader.Data;


	}

	public void showTopbar(String name, String cost, String details)
	{

		this.GetNode<PanelContainer>("PanelContainer2").GetNode<VBoxContainer>("VBoxContainer").GetNode<HBoxContainer>("HBoxContainer").GetNode<Panel>("PanelName").GetNode<RichTextLabel>("RichTextLabel").Text = name;
		this.GetNode<PanelContainer>("PanelContainer2").GetNode<VBoxContainer>("VBoxContainer").GetNode<Panel>("Panel3").GetNode<Button>("RepairButton").Text = "Repair: " + cost;

	}

	public void closeTopbar()
	{

		this.GetNode<PanelContainer>("PanelContainer2").GetNode<VBoxContainer>("VBoxContainer").GetNode<HBoxContainer>("HBoxContainer").GetNode<Panel>("PanelName").GetNode<RichTextLabel>("RichTextLabel").Text = "";
		this.GetNode<PanelContainer>("PanelContainer2").GetNode<VBoxContainer>("VBoxContainer").GetNode<Panel>("Panel3").GetNode<Button>("RepairButton").Text = "Repair: ";

	}




	public Node2D getSelected()
	{

		return selected;

	}

	public void propList(String catagory)
	{

		if (loadedUpgradeDict.ContainsKey(catagory))
		{

			Godot.Collections.Array Type = loadedUpgradeDict[catagory].AsGodotArray();

			for (int i = 0; i < Type.Count; i++)
			{

				BuyOption newBuy = CopyBuyOption.Instantiate() as BuyOption;


				newBuy.Readyer(Type[i].AsGodotDictionary(), catagory);

				this.GetNode<PanelContainer>("PanelContainer").GetNode<VBoxContainer>("VBoxContainer").AddChild(newBuy);
			}

		}
		else
		{

			GD.PushWarning("Not a real catagory");
		}
	}


	public void deleteList()
	{

		for (int i = 0; i < this.GetNode<PanelContainer>("PanelContainer").GetNode<VBoxContainer>("VBoxContainer").GetChildren().Count; i++)
		{

			this.GetNode<PanelContainer>("PanelContainer").GetNode<VBoxContainer>("VBoxContainer").GetChildren()[i].QueueFree();


		}

	}



	public override void _Draw()
	{

		var rect = new Rect2(GetGlobalMousePosition(), 0.001f, 0.001f);

		DrawRect(rect, new Color("ALICE_BLUE"), true, -1, false);


		base._Draw();
	}


	public Node2D ObjectUnderMouse(Vector2 mouse)
	{

		var rect = new Rect2(mouse, 1, 1);

		for (int i = 0; i < GetTree().Root.GetNode<MainScene>("MainScene").GetNode<Tower>("Tower").beamsList.Count; i++)
		{
			Rect2 rec = new Rect2(GetTree().Root.GetNode<MainScene>("MainScene").GetNode<Tower>("Tower").beamsList[i].GlobalPosition, GetTree().Root.GetNode<MainScene>("MainScene").GetNode<Tower>("Tower").beamsList[i].GetNode<Sprite2D>("Sprite2D").Texture.GetSize() * GetTree().Root.GetNode<MainScene>("MainScene").GetNode<Tower>("Tower").beamsList[i].Scale);
			if (rect.Intersects(rec, false))
			{

				return GetTree().Root.GetNode<MainScene>("MainScene").GetNode<Tower>("Tower").beamsList[i];
			}

		}


		for (int i = 0; i < GetTree().Root.GetNode<MainScene>("MainScene").GetNode<Tower>("Tower").collumsList.Count; i++)
		{
			Rect2 rec = new Rect2(GetTree().Root.GetNode<MainScene>("MainScene").GetNode<Tower>("Tower").collumsList[i].GlobalPosition, GetTree().Root.GetNode<MainScene>("MainScene").GetNode<Tower>("Tower").collumsList[i].GetNode<Sprite2D>("Sprite2D").Texture.GetSize() * GetTree().Root.GetNode<MainScene>("MainScene").GetNode<Tower>("Tower").collumsList[i].Scale);
			if (rect.Intersects(rec, false))
			{
				return GetTree().Root.GetNode<MainScene>("MainScene").GetNode<Tower>("Tower").collumsList[i];
			}

		}


		return null;

	}




	public void toggleShow()
	{

		if (this.Visible == true)
		{

			GetTree().Root.GetNode<MainScene>("MainScene").GetNode<Tower>("Tower").ToggleOverLay();
			this.Visible = false;
		}
		else if (this.Visible == false)
		{


			GetTree().Root.GetNode<MainScene>("MainScene").GetNode<Tower>("Tower").DeleteOverLay();
			this.Visible = true;
		}

	}

	public void repairer()
	{

		if (selected is Collum)
		{

			if (((Collum)selected).repairCost() <= GetTree().Root.GetNode<MainScene>("MainScene").Money)
			{

				((Collum)selected).repairIt();
				GetTree().Root.GetNode<MainScene>("MainScene").spendMoney(((Collum)selected).repairCost());
			}
			else
			{

				GetTree().Root.GetNode<MainScene>("MainScene").showMessage("Your broke", new Color(1, 0, 0));

			}

		}

		if (selected is Beamer)
		{

			if (((Beamer)selected).repairCost() <= GetTree().Root.GetNode<MainScene>("MainScene").Money)
			{

				((Beamer)selected).repairIt();
				GetTree().Root.GetNode<MainScene>("MainScene").spendMoney(((Beamer)selected).repairCost());
			}
			else
			{

				GetTree().Root.GetNode<MainScene>("MainScene").showMessage("Your broke", new Color(1, 0, 0));

			}
		}

	}


	public void OnButtonPressedRepair()
	{

		repairer();

	}


	public void OnButtonPressedDetails()
	{


		if (isDetailsPoped == false)
		{
			showDetails();
			isDetailsPoped = true;
		}
		else
		{

			isDetailsPoped = false;
			removeDetails();
			GD.Print("Removed details");
		}



	}

	//Also loweky updates
	public void showDetails()
	{

		if (selected == null)
		{
			this.GetNode<PanelContainer>("PanelContainer3").Visible = false;
			return;
		}

		this.GetNode<PanelContainer>("PanelContainer3").Visible = true;

		if (selected is Collum)
		{

			this.GetNode<PanelContainer>("PanelContainer3").GetNode<RichTextLabel>("RichTextLabel").Text = ((Collum)selected).detailsStr();


		}
		if (selected is Beamer)
		{

			this.GetNode<PanelContainer>("PanelContainer3").GetNode<RichTextLabel>("RichTextLabel").Text = ((Beamer)selected).detailsStr();


		}

	}


	public void removeDetails()
	{

		this.GetNode<PanelContainer>("PanelContainer3").Visible = false;


	}
}

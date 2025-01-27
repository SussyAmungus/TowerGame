using Godot;
using System;

public partial class BuildComp : Control
{

	public int cost = 0;

	public Godot.Collections.Dictionary Dict;

	public String catagoryer = "Nothing";






	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void Readyer(Godot.Collections.Dictionary newDict, String catagory)
	{


		Dict = newDict.Duplicate();



		setName(Dict["name"].ToString());
		setMoney(Dict["cost"].ToString());
		cost = Dict["cost"].AsInt16();
		//setToolTip(Dict["toolTip"].ToString());


		//Also in the type catagory
		catagoryer = catagory;



	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{


	}


	public void setName(String nn)
	{

		this.GetNode<Panel>("Panel").GetNode<RichTextLabel>("RichTextLabel").Text = nn;
	}

	public void setMoney(String nn)
	{

		this.GetNode<Panel>("Panel").GetNode<Button>("Button").Text = "$" + nn;


	}

	public void setToolTip(String nn)
	{

		this.GetNode<Panel>("Panel").GetNode<RichTextLabel>("RichTextLabel2").Text = "--" + nn;

	}

	public void OnButtonDown()
	{
		if (cost > GetTree().Root.GetNode<MainScene>("MainScene").Money)
		{

			GetTree().Root.GetNode<MainScene>("MainScene").GetNode<Message>("Message").putMessage("Not Enough Money", new Color(1f, 0f, 0f, 1));
			return;
		}
		else
		{
			GetTree().Root.GetNode<MainScene>("MainScene").spendMoney(cost);


			if (catagoryer == "Turret")
			{

				GD.Print("Creating a turret");
				this.GetParent().GetParent().GetParent<Constructmenu>().bindSelect(this.GetParent().GetParent().GetParent<Constructmenu>().CreateTurret());

			}
		}





	}








}

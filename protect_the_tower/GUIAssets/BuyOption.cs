using Godot;
using System;
using System.Collections.Generic;

public partial class BuyOption : Control
{

	public int cost = 0;

	public Godot.Collections.Dictionary Dict;

	public String catagoryer = "Nothing";


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{





	}
	//has to be called
	public void Readyer(Godot.Collections.Dictionary newDict, String catagory)
	{


		Dict = newDict.Duplicate();



		setName(Dict["name"].ToString());
		setMoney(Dict["cost"].ToString());
		setToolTip(Dict["toolTip"].ToString());
		cost = Dict["cost"].AsInt16();

		catagoryer = catagory;

		//Or other system with seeming if the property is there
		if (catagory == "collumUpgrades")
		{

			setAdditional(Dict["health"].ToString() + " : HP");
		}

		if (catagory == "beamUpgrades")
		{

			setAdditional(Dict["health"].ToString() + " : HP");
		}



	}

	public void setAdditional(String nn)
	{

		this.GetNode<Panel>("Panel").GetNode<RichTextLabel>("RichTextLabel3").Text = nn;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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

			Node2D temp = GetTree().Root.GetNode<MainScene>("MainScene").GetNode<BuildMenu>("BuildMenu").getSelected();



			if (temp is Collum)
			{

				Collum col = (Collum)temp;

				col.health = (double)Dict["health"];

				GD.Print(col.health);

			}

			if (temp is Beamer)
			{

				Beamer col = (Beamer)temp;

				col.health = (double)Dict["health"];

				GD.Print(col.health);

			}



		}




	}

	public Godot.Collections.Dictionary getDict()
	{


		return Dict;
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

	//The buy is generated on the build menu // this is only action

}

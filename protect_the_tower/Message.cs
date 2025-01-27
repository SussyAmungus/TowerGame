using Godot;
using System;

public partial class Message : Control
{

	private double beforeFade = 3;

	private double totalFadeTime = 1;

	private bool messageShown = false;

	private bool fading = false;

	private double time = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (messageShown == true)
		{

			time = time + delta;

			if (time >= beforeFade)
			{

				time = 0;
				fading = true;
				messageShown = false;
			}
		}

		if (fading == true)
		{
			time = time + delta;


			//this is retarded
			Color samecolr = this.GetNode<RichTextLabel>("RichTextLabel").Modulate;

			samecolr.A = (float)Mathf.Lerp(1f, 0f, time / totalFadeTime);

			GetNode<RichTextLabel>("RichTextLabel").Modulate = samecolr;




			if (time >= totalFadeTime)
			{

				time = 0;
				fading = false;
				this.GetNode<RichTextLabel>("RichTextLabel").Text = "";
				this.GetNode<RichTextLabel>("RichTextLabel").Modulate = this.GetNode<RichTextLabel>("RichTextLabel").SelfModulate;
				this.GetNode<RichTextLabel>("RichTextLabel").Visible = false;
				GD.Print("DONE");
			}

		}



	}



	public void putMessage(String text)
	{
		this.GetNode<RichTextLabel>("RichTextLabel").Text = text;
		messageShown = true;
		this.GetNode<RichTextLabel>("RichTextLabel").Visible = true;
	}

	public void putMessage(String text, Color cc)
	{
		this.GetNode<RichTextLabel>("RichTextLabel").Text = text;
		this.GetNode<RichTextLabel>("RichTextLabel").Modulate = cc;
		messageShown = true;
		this.GetNode<RichTextLabel>("RichTextLabel").Visible = true;

	}
}

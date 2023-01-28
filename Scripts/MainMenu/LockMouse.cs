using Godot;
using MainMenu;
using System;

public partial class TestCamera : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{

        if (!GetTree().Paused)
        {
            Input.MouseMode = Input.MouseModeEnum.Confined;
        }
        else
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
        GD.Print(Input.MouseMode);
    }
}
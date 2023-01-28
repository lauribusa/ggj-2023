using Godot;
using System;

namespace Game;
public partial class TentacleBehavior : Area2D
{
	// Called when the node enters the scene tree for the first time.
	Vector2 mousePos;
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

    public override void _PhysicsProcess(double delta)
    {
        var to = GetViewport().GetMousePosition();
        GlobalPosition = GlobalPosition.Lerp(to, 15 * (float)GetPhysicsProcessDeltaTime());
    }

    public override void _Input(InputEvent @event)
    {

        /*if (@event is InputEventMouseMotion)
        {
            var to = GetViewport().GetMousePosition();
            GlobalPosition = GlobalPosition.Lerp(to, 1 * (float)GetPhysicsProcessDeltaTime());
            //mousePos = viewport.GetMousePosition();
        }*/
    }

    public override void _InputEvent(Viewport viewport, InputEvent @event, long shapeIdx)
    {
        if(@event is InputEventMouseMotion)
		{
			//GlobalPosition = viewport.GetMousePosition();
			//mousePos = viewport.GetMousePosition();
		}
    }
}

using Godot;
using System;
namespace Game;
public partial class ControlNode : Area3D
{
	// Called when the node enters the scene tree for the first time.
	private bool isDragging;
	private Vector3 originalPosition;
	private float lerpSpeed = .1f;
	private float rayLength = 4000;
	[Export]
	public Camera3DVariable CameraVariable;
	Vector2 mousePos = new Vector2();
	public override void _Ready()
	{
		originalPosition = GlobalPosition;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionReleased("MouseDown"))
        {
            isDragging = false;
            GlobalPosition = originalPosition;
            GD.Print("Mouse Is Released");
        }
		if(inputEvent is InputEventMouseMotion inputEventMouseMotion)
		{
			mousePos = inputEventMouseMotion.GlobalPosition;
		}
    }

    public override void _PhysicsProcess(double delta)
    {
        if(isDragging) 
		{
			var mousePosVP = GetViewport().GetFinalTransform().BasisXformInv(mousePos);
            var from = CameraVariable.value.ProjectRayOrigin(mousePosVP);
			var to = from + CameraVariable.value.ProjectRayNormal(mousePosVP) * rayLength;
			var intersect = PhysicsRayQueryParameters3D.Create(from, to);
			var collision = GetWorld3d().DirectSpaceState.IntersectRay(intersect);
			
			//GlobalPosition.Lerp((Vector3)collision["Position"], lerpSpeed * (float)delta);
			
			GlobalPosition = new Vector3(to.x, to.y, 0);
			//GlobalTranslate((Vector3)GlobalPosition.Lerp(new Vector3(mouseToWorldPoint.x, mouseToWorldPoint.y, originalPosition.z), (float)(lerpSpeed * delta)));
			//GlobalPosition = (Vector3)GlobalPosition.Lerp(new Vector3(mouseToWorldPoint.x, mouseToWorldPoint.y, originalPosition.z), (float)(lerpSpeed * delta));
            GD.Print($"New Global Position: " + GlobalPosition + $" {collision.ToString()}");
        }
    }

    public void OnInputEvent(Node camera, InputEvent inputEvent, Vector3 position, Vector3 normal, int shapeIdx)
	{
		if (inputEvent.IsActionPressed("MouseDown"))
		{
			isDragging = true;
			GD.Print("Mouse is down");
		}
    }
}

using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Game;
[Tool]
public partial class GameNode : Area2D
{
    // Called when the node enters the scene tree for the first time.
    [Export]
    public Array<NodePath> closeNeighborsNodes;
    [Export]
    public Array<NodePath> farNeighborsNodes;
    [Export]
    public Faction currentFaction;
    [Export]
    public int currentPowerValue;
    [Export]
    public int chargeRate;
    [Export]
    public Label powerValueLabel;
    public bool isHovered;
    [Export]
    public bool isMainNode;
    private double elapsedTime;

    public override void _Ready()
    {
        if (Engine.IsEditorHint()) return;
        GameManager gameManager = GetNode("/root/GameManager") as GameManager;
        gameManager.AddToNodeList(GetPath());
        UpdateValueText(currentPowerValue);
    }

    public override void _Draw()
    {
        if (Engine.IsEditorHint())
        {
            for (int i = 0; i < farNeighborsNodes.Count; i++)
            {
                var node = GetNode<GameNode>(farNeighborsNodes[i]);
                DrawConnection(node);
            }
            for (int i = 0; i < closeNeighborsNodes.Count; i++)
            {
                var node = GetNode<GameNode>(closeNeighborsNodes[i]);
                DrawConnection(node);
            }
        }
    }
    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint()) {
            QueueRedraw();
        
        } else
        {
            elapsedTime += delta;
            if(elapsedTime >= 1 && currentPowerValue < GameManager.Instance.maxValueOnNodes)
            {
                elapsedTime = 0;
                GeneratePowerValue();
            }
        }
    }
    private void UpdateValueText(int value)
    {
        powerValueLabel.Text = value.ToString();
    }
    private void DrawConnection(GameNode toNode)
    {
        if (toNode is null) return;
        DrawDashedLine(GlobalPosition, toNode.GlobalPosition, Color.Color8(255, 255, 255));
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Input(InputEvent @event)
    {
        if (Engine.IsEditorHint()) return;
        if (@event.IsActionReleased("MouseDown"))
        {
            if (!isHovered) return;
            if (GameManager.Instance.HasOriginSelected())
            {
                if (GameManager.Instance.originNode == this)
                {
                    GameManager.Instance.RemoveOriginNode(this);
                    //GameManager.Instance.CleanSelectedNodes();
                    return;
                }
                if (GameManager.Instance.HasDestinationSelected())
                {
                    if (GameManager.Instance.destinationNode == this)
                    {
                        GameManager.Instance.RemoveDestinationNode(this);
                        //GameManager.Instance.CleanSelectedNodes();
                        return;
                    }
                    return;
                }
                GameManager.Instance.SelectDestinationNode(this);
                return;
            }
            GameManager.Instance.SelectOriginNode(this);
        }
    }

    public void OnMouseEnter()
    {
        if (Engine.IsEditorHint()) return;
        isHovered = true;
    }

    public void GeneratePowerValue()
    {
        currentPowerValue += chargeRate;
        UpdateValueText(currentPowerValue);
    }

    public void OnMouseLeft()
    {
        if (Engine.IsEditorHint()) return;
        isHovered = false;
    }
}

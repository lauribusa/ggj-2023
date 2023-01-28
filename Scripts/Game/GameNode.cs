using Godot;
using Godot.Collections;
using System.Collections.Generic;


namespace Game;
[Tool]
public partial class GameNode : Area2D
{
    // Called when the node enters the scene tree for the first time.
    public List<NodePath> neighbors = new List<NodePath>();
    [Export]
    public Array<NodePath> closeNeighborsNodes;
    [Export]
    public Array<NodePath> farNeighborsNodes;

    private Faction _currentFaction;

    [Export]
    public Cell Cell { get; set; }

    [Export]
    public Faction CurrentFaction
    {
        get => _currentFaction;
        set
        {
            _currentFaction = value;
            SetCellFaction();
        }
    }

    [Export]
    public int currentPowerValue;
    [Export]
    public int chargeRate;
    [Export]
    public Label powerValueLabel;
    public bool isHovered;
    [Export]
    public bool isMainNode;
    [Export]
    public TextureRect sprite;
    private double elapsedTime;
    public bool canChargeUp;

    public override void _Ready()
    {
        Cell.GameNode = this;
        SetCellFaction();
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
            if (CurrentFaction == Faction.Neutral) return;
            if (!canChargeUp) return;
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
        GD.Print(value);
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
                if (GameManager.Instance.playerOriginNode == this)
                {
                    GameManager.Instance.RemoveOriginNode(this);
                    return;
                }
                if (GameManager.Instance.HasDestinationSelected())
                {
                    if (GameManager.Instance.playerDestinationNode == this)
                    {
                        GameManager.Instance.RemoveDestinationNode(this);
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

    public void IncreasePower()
    {
        currentPowerValue++;
        UpdateValueText(currentPowerValue);
    }

    public void IncreasePower(int value)
    {
        currentPowerValue += value;
        UpdateValueText(currentPowerValue);
    }

    public void DecreasePower() 
    {
        currentPowerValue--;
        UpdateValueText(currentPowerValue);
    }

    public void DecreasePower(int value)
    {
        currentPowerValue -= value;
        UpdateValueText(currentPowerValue);
    }

    public void SetPower(int value)
    {
        currentPowerValue = value;
        UpdateValueText(currentPowerValue);
    }

    public void OnMouseLeft()
    {
        if (Engine.IsEditorHint()) return;
        isHovered = false;
    }

    private void SetCellFaction()
    {
        if (Cell == null) return;
        switch (_currentFaction)
        {
            case Faction.Neutral:
                Cell.SetNeutral();
                break;

            case Faction.Parasite:
                Cell.SetCorrupt();
                break;

            case Faction.ImmuneSystem:
                Cell.SetImmune();
                break;
        }
    }
}
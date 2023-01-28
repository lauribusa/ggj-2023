using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace Game;
public enum Faction
{
	Parasite,
	ImmuneSystem,
	Neutral
}
public partial class GameManager : Node2D
{
	
	// Called when the node enters the scene tree for the first time.
	public GameNode originNode;
	public GameNode destinationNode;
	private static GameManager _instance;

    [Signal] public delegate void NodeSelectedEventHandler(GameNode nodeFrom, GameNode nodeTo);
	[Signal] public delegate void NodeUnselectedEventHandler(GameNode node, bool isOrigin);
	[Signal] public delegate void GameEndEventHandler(bool playerWins);

    public static GameManager Instance { get => _instance; }

	public Array<GameNode> gameNodes;
	public GameNode playerBase;
	public GameNode enemyBase;
	[Export]
	public int maxValueOnNodes;
	public override void _Ready()
	{
		_instance = this;
        NodeSelected += OnNodeSelected;
		NodeUnselected += OnNodeUnselected;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    private void OnNodeSelected(GameNode nodeFrom, GameNode nodeTo)
    {
		if (nodeFrom.currentPowerValue >= nodeTo.currentPowerValue)
		{
			nodeTo.currentFaction = nodeFrom.currentFaction;
			nodeTo.currentPowerValue = Mathf.FloorToInt(nodeFrom.currentPowerValue * 0.5f);
			nodeFrom.currentPowerValue -= Mathf.FloorToInt(nodeFrom.currentPowerValue * 0.5f);
		}
		GD.Print(nodeFrom.Name, nodeFrom.currentFaction);
		GD.Print(nodeTo.Name, nodeFrom.currentFaction);
    }
    public override void _Process(double delta)
	{
		QueueRedraw();
		if (playerBase is null || enemyBase is null) return;
		if(playerBase.currentFaction != Faction.Parasite)
		{
			EmitSignal("GameEnd", false);
		}
		if(enemyBase.currentFaction != Faction.ImmuneSystem) 
		{
			EmitSignal("GameEnd", true);
		}
	}

    public override void _Draw()
    {
		DrawConnection();
    }

	private void OnGameEnd(bool playerWins)
	{

	}

    public void SelectOriginNode(GameNode node)
	{
		originNode = node;
		node.SelfModulate = Color.Color8(255, 0, 0);
		GD.Print("Added origin: "+node.Name);
	}

	public void SelectDestinationNode(GameNode node)
	{
		destinationNode = node;
		node.SelfModulate = Color.Color8(0, 255, 0);
		GD.Print("Added destination: "+node.Name);
        _Draw();
		EmitSignal("NodeSelected", originNode, node);
    }

	private void DrawConnection()
	{
		if (destinationNode is null || originNode is null) return;
        DrawDashedLine(originNode.Position, destinationNode.Position, Color.Color8(255, 0, 0));
    }

	private void OnNodeUnselected(GameNode node, bool isOrigin)
	{
		node.SelfModulate = Color.Color8(255, 255, 255);
		if(isOrigin)
		{
			if (node.currentFaction == Faction.Parasite) originNode = node;
		}
	}

    public void AddToNodeList(NodePath node)
	{
		GameNode gameNode = GetNode(node) as GameNode;
		gameNodes.Add(gameNode);
		GD.Print(gameNode.Name);
	}

	public bool HasOriginSelected()
	{
		return originNode != null;
	}

	public bool HasDestinationSelected()
	{
		return destinationNode != null;
	}

	public bool HasNoDestination()
	{
		return destinationNode == null;
	}

	public void CleanSelectedNodes()
	{
		originNode = null;
		destinationNode = null;
	}

	public void RemoveOriginNode(GameNode node)
	{
		GD.Print("Removed origin.");
		EmitSignal("NodeUnselected",node, true);
		originNode = null;
	}

	public void RemoveDestinationNode(GameNode node)
	{
		GD.Print("Removed destination.");
		EmitSignal("NodeUnselected", node, false);
		destinationNode = null;
	}
}

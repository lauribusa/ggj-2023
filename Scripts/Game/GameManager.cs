using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

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
	public GameNode playerOriginNode;
	public GameNode playerDestinationNode;
	public GameNode enemyOriginNode;
	public GameNode enemyDestinationNode;
	private static GameManager _instance;

    [Signal] public delegate void NodeSelectedEventHandler(GameNode nodeFrom, GameNode nodeTo);
	[Signal] public delegate void NodeUnselectedEventHandler(GameNode node, bool isOrigin);
	[Signal] public delegate void GameEndEventHandler(bool playerWins);

    public static GameManager Instance { get => _instance; }

	public Array<NodePath> gameNodes = new Array<NodePath>();
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
		if (nodeFrom.CurrentFaction != Faction.Parasite) return;
		
		if (nodeFrom.currentPowerValue >= nodeTo.currentPowerValue)
		{
			nodeTo.CurrentFaction = nodeFrom.CurrentFaction;
			nodeTo.currentPowerValue += Mathf.FloorToInt(nodeFrom.currentPowerValue * 0.5f);
			nodeFrom.currentPowerValue -= Mathf.FloorToInt(nodeFrom.currentPowerValue * 0.5f);
		}
		GD.Print(nodeFrom.Name, nodeFrom.CurrentFaction);
		GD.Print(nodeTo.Name, nodeFrom.CurrentFaction);
    }
	public bool LookForNeighbor(Array<GameNode> nodes, GameNode nodeToLookUp)
	{
		return nodes.ToList().Exists(node => node == nodeToLookUp);
	}
    public override void _Process(double delta)
	{
		QueueRedraw();
		if (playerBase is null || enemyBase is null) return;
		if(playerBase.CurrentFaction != Faction.Parasite)
		{
			EmitSignal("GameEnd", false);
		}
		if(enemyBase.CurrentFaction != Faction.ImmuneSystem) 
		{
			EmitSignal("GameEnd", true);
		}
		if(HasOriginSelected() && HasDestinationSelected())
		{
			playerOriginNode.DecreasePower();
			if(playerDestinationNode.CurrentFaction == Faction.Parasite)
			{
				if (playerDestinationNode.currentPowerValue >= maxValueOnNodes) playerDestinationNode.SetPower(maxValueOnNodes);
				playerDestinationNode.IncreasePower();
			} else
			{
				playerDestinationNode.DecreasePower();
				if(playerDestinationNode.currentPowerValue <= 0)
				{
					// switch faction
				}
			}

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
		if (node.CurrentFaction != Faction.Parasite) return;
		playerOriginNode = node;
		node.sprite.SelfModulate = Color.Color8(255, 0, 0);
		GD.Print("Added origin: "+node.Name);
	}

	public void SelectDestinationNode(GameNode node)
	{
		playerDestinationNode = node;
		node.sprite.SelfModulate = Color.Color8(0, 255, 0);
		GD.Print("Added destination: "+node.Name);
		EmitSignal("NodeSelected", playerOriginNode, node);
    }

	private void DrawConnection()
	{
		if (playerDestinationNode is null || playerOriginNode is null) return;
        DrawDashedLine(playerOriginNode.Position, playerDestinationNode.Position, Color.Color8(255, 0, 0));
    }

	private void OnNodeUnselected(GameNode node, bool isOrigin)
	{
		node.sprite.SelfModulate = Color.Color8(255, 255, 255);
		if(isOrigin)
		{
			if (playerDestinationNode.CurrentFaction == Faction.Parasite)
			{
				var _newNode = playerDestinationNode;
				CleanSelectedNodes();
				SelectOriginNode(_newNode);
			}
		}
	}

    public void AddToNodeList(NodePath node)
	{
		GameNode gameNode = GetNode(node) as GameNode;
		gameNodes.Add(node);
		GD.Print(gameNode.Name);
	}

	public bool HasOriginSelected()
	{
        return playerOriginNode != null;
	}

	public bool HasDestinationSelected()
	{
		return playerDestinationNode != null;
	}

	public void CleanSelectedNodes()
	{
		if (playerOriginNode is not null) playerOriginNode.sprite.SelfModulate = Color.Color8(255, 255, 255);
		playerOriginNode = null;
		if (playerDestinationNode is not null) playerDestinationNode.sprite.SelfModulate = Color.Color8(255, 255, 255);
		playerDestinationNode = null;
	}

	public void RemoveOriginNode(GameNode node)
	{
        playerOriginNode.sprite.SelfModulate = Color.Color8(255, 255, 255);
		GD.Print("Removed origin.");
		playerOriginNode = null;
        EmitSignal("NodeUnselected",node, true);
	}

	public void RemoveDestinationNode(GameNode node)
	{
        playerDestinationNode.sprite.SelfModulate = Color.Color8(255, 255, 255);
		GD.Print("Removed destination.");
        playerDestinationNode = null;
		EmitSignal("NodeUnselected", node, false);
	}
}

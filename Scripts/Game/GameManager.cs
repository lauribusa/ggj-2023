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
	public GameNode playerOriginNode;
	public GameNode playerDestinationNode;
	public GameNode enemyOriginNode;
	public GameNode enemyDestinationNode;
	private static GameManager _instance;

    [Signal] public delegate void NodeSelectedEventHandler(GameNode nodeFrom, GameNode nodeTo);
	[Signal] public delegate void NodeUnselectedEventHandler(GameNode node, bool isOrigin);
	[Signal] public delegate void NodeCapturedEventHandler(GameNode node, Faction newFaction);
	[Signal] public delegate void GameEndEventHandler(bool playerWins);

    public static GameManager Instance { get => _instance; }

	public Array<GameNode> gameNodes = new Array<GameNode>();
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
			nodeTo.currentPowerValue += Mathf.FloorToInt(nodeFrom.currentPowerValue * 0.5f);
			nodeFrom.currentPowerValue -= Mathf.FloorToInt(nodeFrom.currentPowerValue * 0.5f);
		}
		GD.Print(nodeFrom.Name, nodeFrom.currentFaction);
		GD.Print(nodeTo.Name, nodeFrom.currentFaction);
		for (int i = 0; i < gameNodes.Count; i++)
		{
			if (gameNodes[i].isMainNode && gameNodes[i].currentFaction == Faction.Parasite)
			{
				playerBase = gameNodes[i];
			}
            if (gameNodes[i].isMainNode && gameNodes[i].currentFaction == Faction.ImmuneSystem)
            {
                playerBase = gameNodes[i];
            }
        }
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
		if(HasOriginSelected() && HasDestinationSelected())
		{
			playerOriginNode.DecreasePower();
			if(playerDestinationNode.currentFaction == Faction.Parasite)
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
		playerOriginNode = node;
		node.sprite.SelfModulate = Color.Color8(255, 0, 0);
		GD.Print("Added origin: "+node.Name);
		EmitSignal("NodeSelected", playerO)
	}

	public void SelectDestinationNode(GameNode node)
	{
		playerDestinationNode = node;
		node.sprite.SelfModulate = Color.Color8(0, 255, 0);
		GD.Print("Added destination: "+node.Name);
        _Draw();
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
			if (playerDestinationNode.currentFaction == Faction.Parasite)
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
		gameNodes.Add(gameNode);
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

	public bool HasNoDestination()
	{
		return playerDestinationNode == null;
	}

	public void CleanSelectedNodes()
	{
		if (playerOriginNode is not null) playerOriginNode.sprite.SelfModulate = Color.Color8(255, 255, 255);
		playerOriginNode = null;
		playerDestinationNode = null;
	}

	public void RemoveOriginNode(GameNode node)
	{
        playerOriginNode.sprite.SelfModulate = Color.Color8(255, 255, 255);
		GD.Print("Removed origin.");
        EmitSignal("NodeUnselected",node, true);
		playerOriginNode = null;
	}

	public void RemoveDestinationNode(GameNode node)
	{
        playerDestinationNode.sprite.SelfModulate = Color.Color8(255, 255, 255);
		GD.Print("Removed destination.");
		EmitSignal("NodeUnselected", node, false);
        playerDestinationNode = null;
	}
}

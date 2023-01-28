using Godot;
using Godot.Collections;

namespace Game;

public partial class Cell : Node3D
{
    #region Exposed

    [Export]
    public GameNode GameNode {get; set;}

    [Export]
    private Node3D _neutralCell;

    [Export]
    private Node3D _defenseCell;

    [Export]
    private Node3D _corruptCell;

    [Export]
    private PackedScene _tentacleLongPrefab;

    #endregion


    #region Godot API

    public override void _Ready()
    {     
        Array<NodePath> neighbourPath = GameNode.farNeighborsNodes;
        for (int i = 0; i < neighbourPath.Count; i++)
        {
            var gameNode = GameNode.GetNode<GameNode>(neighbourPath[i]);
            var tentacle = _tentacleLongPrefab.Instantiate<Tentacle>();
            tentacle.Target = gameNode;

            AddChild(tentacle);
            tentacle.Position = Position;
            tentacle.LookAt(gameNode.Cell.Position);
        }
    }

    #endregion


    #region Main

    public void SetNeutral()
    {
        _neutralCell.Visible = true;
        _defenseCell.Visible = false;
        _corruptCell.Visible = false;
    }

    public void SetCorrupt()
    {
        _neutralCell.Visible = false;
        _defenseCell.Visible = false;
        _corruptCell.Visible = true;
    }

    public void SetImmune()
    {
        _neutralCell.Visible = false;
        _defenseCell.Visible = true;
        _corruptCell.Visible = false;
    }

    #endregion
}
using Godot;

namespace Game;

public partial class TentacleSpawner : Node
{
    #region Exposed

    [Export]
    private PackedScene _tentacleLong;

    #endregion


    #region Godot API

    public override void _Ready()
    {
        _cell = GetOwner<Cell>();
        GameManager.Instance.NodeSelected += OnGameManagerNodeSelected;
    }

    #endregion


    #region Main

    private void OnGameManagerNodeSelected(GameNode nodeFrom, GameNode nodeTo)
    {
        if (nodeFrom != _cell.GameNode) return;
    }

    #endregion


    #region Private And Protected

    private Cell _cell;

    #endregion
}
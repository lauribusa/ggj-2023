using Godot;

namespace Game;

public partial class Cell : Node3D
{
    #region Exposed

    public GameNode GameNode {get; set;}

    [Export]
    private Node3D _neutralCell;

    [Export]
    private Node3D _defenseCell;

    [Export]
    private Node3D _corruptCell;

    #endregion


    #region Godot API

    public override void _Ready()
    {
        GameManager.Instance.NodeSelected += OnGameManagerNodeSelected;
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

    private void OnGameManagerNodeSelected(GameNode nodeFrom, GameNode nodeTo)
    {
        
    }

    #endregion
}
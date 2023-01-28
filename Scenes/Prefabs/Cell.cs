using Godot;

namespace Game;

public partial class Cell : Node3D
{
    #region Exposed

    [Export]
    private Node3D _neutralCell;

    [Export]
    private Node3D _defenseCell;

    [Export]
    private Node3D _corruptCell;

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
using Godot;

namespace Game;

public partial class Tentacle : Node3D
{
    #region Signals

    [Signal]
    public delegate void DeployedEventHandler(bool isDeploy);

    #endregion


    #region Exposed

    public GameNode Target { get; set; }

    #endregion


    #region Godot API

    public override void _Ready()
    {
        _cell = GetParent<Cell>();
        GameManager.Instance.NodeSelected += OnGameManagerNodeSelected;
        _animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _animPlayer.AnimationFinished += OnAnimationFinished;
    }

    private void OnAnimationFinished(StringName animName) => EmitSignal(SignalName.Deployed, _isDeployed);

    #endregion


    #region Main

    public void Deploy()
    {
        _animPlayer.Play(DEPLOY);
        _isDeployed = true;
    }

    public void Retract()
    {
        _animPlayer.PlayBackwards(DEPLOY);
        _isDeployed = false;
    }

    private void OnGameManagerNodeSelected(GameNode nodeFrom, GameNode nodeTo)
    {
        if (nodeFrom != _cell.GameNode) return;
        if (_cell.GameNode.CurrentFaction != Faction.Parasite) return;
        if (Target != nodeTo) return;

        Deploy();
    }

    #endregion


    #region Private And Protected

    private static readonly StringName DEPLOY = "deploy";

    private AnimationPlayer _animPlayer;
    private Cell _cell;
    private bool _isDeployed;

    #endregion
}
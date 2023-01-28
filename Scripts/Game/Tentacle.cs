using Godot;

namespace Game;

public partial class Tentacle : Node3D
{
    #region Signals

    [Signal]
    public delegate void DeployedEventHandler(bool isDeploy);

    #endregion


    #region Godot API

    public override void _Ready()
    {
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

    #endregion


    #region Private And Protected

    private static readonly StringName DEPLOY = "deploy";

    private AnimationPlayer _animPlayer;
    private bool _isDeployed;

    #endregion
}
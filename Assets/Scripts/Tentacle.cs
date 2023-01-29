using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Tentacle : MonoBehaviour
{
    #region Exposed

    public UnityEvent Deployed;
    public UnityEvent Retracted;
    public UnityEvent Reached;

    public GameNodeUI Target { get; set; }
    public GameNodeUI GameNode { get; set; }
    public Transform Transform => _transform ? _transform : _transform = GetComponent<Transform>();

    #endregion


    #region Unity API

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        GameManager.Instance.linkDestroyedEvent.AddListener(OnLinkDestroyed);
        GameManager.Instance.NodeClickedEvent.AddListener(OnNodeClicked);
    }

    #endregion


    #region Main

    public void Deploy()
    {
        _animator.SetBool(CAN_DEPLOY, true);
        _hasReach = true;
        Deployed?.Invoke();
    }

    public void Retract()
    {
        _animator.SetBool(CAN_DEPLOY, false);
        _hasReach = false;
        Retracted?.Invoke();
    }

    private void OnTargetReached()
    {
        Debug.Log("target reached");
        GameManager.Instance.linkCreatedEvent?.Invoke(GameNode, Target);
        Reached?.Invoke();
    }

    private void OnNodeClicked(GameNodeUI node)
    {
        if (GameManager.Instance.playerSelectedOrigin != GameNode) return;
        if (node != Target) return;

        Deploy();
    }

    private void OnLinkDestroyed(GameNodeUI from, GameNodeUI to)
    {
        if (from != GameNode && to != Target) return;
        if(to.CurrentFaction == Faction.Parasite) return;

        Retract();
    }

    #endregion


    #region Private And Protected

    private const string CAN_DEPLOY = "CanDeploy";

    private Animator _animator;
    private Transform _transform;
    private bool _hasReach;

    #endregion
}
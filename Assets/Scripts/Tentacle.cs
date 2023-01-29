using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Tentacle : MonoBehaviour
{
    #region Events

    public UnityEvent<bool> Deployed;

    #endregion


    #region Exposed

    public GameNodeUI Target { get; set; }

    public Transform Transform => _transform ? _transform : _transform = GetComponent<Transform>();

    public GameNodeUI GameNode { get; set; }
    #endregion


    #region Unity API

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        GameManager.Instance.linkCreatedEvent.AddListener(OnLinkCreated);
    }

    #endregion


    #region Main

    public void Deploy()
    {
        _animator.SetBool(CAN_DEPLOY, true);
        Deployed?.Invoke(true);
    }

    public void Retract()
    {
        _animator.SetBool(CAN_DEPLOY, false);
        Deployed?.Invoke(false);
    }

    private void OnLinkCreated(GameNodeUI from, GameNodeUI to)
    {
        if (to != Target) return;
        if (from != GameNode) return;
        if (GameNode.CurrentFaction != Faction.Parasite) return;

        Deploy();
    }

    #endregion


    #region Private And Protected

    private const string CAN_DEPLOY = "CanDeploy";
    private Animator _animator;
    private Transform _transform;

    #endregion
}
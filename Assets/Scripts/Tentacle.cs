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
        Deployed?.Invoke();
        _isDeploying = true;
    }

    public void Retract()
    {
        _animator.SetBool(CAN_DEPLOY, false);
        Retracted?.Invoke();
        _isDeploying = false;
    }

    private void OnTargetReached()
    {

        if (!_isDeploying) 
        {
            Debug.Log("Is already deploying");
            return;
        
        }
        if (GameNode.CurrentFaction != Faction.Parasite) return;
        Debug.Log($"{name}: target reached");
        GameManager.Instance.linkCreatedEvent?.Invoke(GameNode, Target);
        Reached?.Invoke();
    }

    private void OnNodeClicked(GameNodeUI targetNode)
    {
        Debug.Log($"Node clicked (tentacle) {targetNode.CurrentFaction} {targetNode.gameObject.name}");
        if(targetNode == GameNode)
        {
            Debug.Log("Cannot click self.");
            return;
        }
        if (GameManager.Instance.playerSelectedOrigin != GameNode)
        {
            Debug.Log($"Origin is not GameNode");
            return;
        }
        if (targetNode != Target) 
        {
            Debug.Log($"node is not a target");
            return;
        }
        var linkExists = GameManager.Instance.CheckIfLinkAlreadyExists(GameManager.Instance.playerSelectedOrigin, targetNode);
        if (linkExists)
        {
            Debug.Log($"Link already exists or is not valid.");
            return;
        }
        if (GameManager.Instance.playerSelectedOrigin != null)
        {
            GameManager.Instance.playerSelectedOrigin = null;
        }
        Deploy();
    }

    private void OnLinkDestroyed(GameNodeUI from, GameNodeUI to)
    {
        if (from != GameNode && to != Target) return;
        if (to.CurrentFaction == Faction.Parasite) return;

        Retract();
    }

    #endregion


    #region Private And Protected

    private const string CAN_DEPLOY = "CanDeploy";

    private Animator _animator;
    private Transform _transform;
    private bool _isDeploying;

    #endregion
}
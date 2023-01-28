using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Tentacle : MonoBehaviour
{
    #region Events

    public UnityEvent<bool> Deployed;

    #endregion


    #region Unity API

    private void Awake() => _animator = GetComponent<Animator>();

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

    #endregion


    #region Private And Protected

    private const string CAN_DEPLOY = "CanDeploy";
    private Animator _animator;

    #endregion
}
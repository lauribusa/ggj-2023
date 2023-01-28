using UnityEngine;

public class Billboard : MonoBehaviour
{
    #region Unity API

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _cameraTransform = Camera.main.GetComponent<Transform>();
    }

    private void Update() => AlignToCamera();

    #endregion


    #region Main

    private void AlignToCamera()
    {
        var direction = _transform.position - _cameraTransform.position;
        direction.Normalize();

        var up = Vector3.Cross(direction, _cameraTransform.right);
        _transform.rotation = Quaternion.LookRotation(direction, up);
    }

    #endregion


    #region Private and Protected

    private Transform _transform;
    private static Transform _cameraTransform;

    #endregion
}
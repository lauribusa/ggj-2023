using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Canvas))]
public class CanvasCameraInjector : MonoBehaviour
{
    #region Unity API

    private void Awake()
    {
        var canvas = GetComponent<Canvas>();
        if (canvas.renderMode != RenderMode.WorldSpace) return;

        canvas.worldCamera = Camera.main;
    }

    #endregion
}
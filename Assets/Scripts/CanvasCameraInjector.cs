using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(Canvas))]
public class CanvasCameraInjector : MonoBehaviour
{
	#region Unity API
	
    private void Awake() => GetComponent<Canvas>().worldCamera = Camera.main;

	#endregion
}
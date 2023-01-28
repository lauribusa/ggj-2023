using UnityEngine;

public class CellNeighborsChecker : MonoBehaviour
{
	#region Exposed

	[SerializeField]
	private Transform[] _neighbors;
	
	#endregion


	#region Unity API
	
    private void Awake()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (_neighbors.Length == 0) return;
    }

    #endregion


    #region Utils



    #endregion


    #region Private and Protected



    #endregion
}
using UnityEngine;

public class CellNeighborsChecker : MonoBehaviour
{
    #region Exposed

    [field:SerializeField]
    public Transform[] Neighbors { get; private set; }
	
	#endregion


	#region Unity API

    private void OnDrawGizmos()
    {
        for (int i = 0; i < Neighbors.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, Neighbors[i].position);
        }
    }

    #endregion
}
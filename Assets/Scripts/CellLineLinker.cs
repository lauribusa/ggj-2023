using UnityEngine;

public class CellLineLinker : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private GameNodeUI _gameNode;

    [SerializeField]
    private LineRenderer _veinPathPrefab;

    #endregion


    #region Unity API

    private void Awake()
    {
        DrawLineToNeighbors();
    }

	#endregion


	#region Utils

    private void DrawLineToNeighbors()
    {
        foreach (GameNodeUI neighbor in _gameNode.farNeighbors)
        {
            var lineRenderer = Instantiate(_veinPathPrefab, transform.position, Quaternion.identity);

            Vector3 cellPosition = transform.position;
            cellPosition.y += Y_OFFSET;
            lineRenderer.SetPosition(0, cellPosition);

            Vector3 neighborPosition = neighbor.cell.Transform.position;
            neighborPosition.y += Y_OFFSET;
            lineRenderer.SetPosition(1, neighborPosition);
        }

        foreach (GameNodeUI neighbor in _gameNode.closeNeighbors)
        {
            var lineRenderer = Instantiate(_veinPathPrefab, transform.position, Quaternion.identity);

            Vector3 cellPosition = transform.position;
            cellPosition.y += Y_OFFSET;
            lineRenderer.SetPosition(0, cellPosition);

            Vector3 neighborPosition = neighbor.cell.Transform.position;
            neighborPosition.y += Y_OFFSET;
            lineRenderer.SetPosition(1, neighborPosition);
        }
    }

    #endregion


    #region Private and Protected

    private const float Y_OFFSET = 0.5f;

    #endregion
}
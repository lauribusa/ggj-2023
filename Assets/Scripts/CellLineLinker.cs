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

            Vector3 cellPosition = GetRandom(transform.position);
            cellPosition.y += Y_OFFSET;
            lineRenderer.SetPosition(0, cellPosition);

            Vector3 neighborPosition = neighbor.cell.Transform.position;
            neighborPosition.y += Y_OFFSET;
            lineRenderer.SetPosition(1, neighborPosition);
        }

        foreach (GameNodeUI neighbor in _gameNode.closeNeighbors)
        {
            var lineRenderer = Instantiate(_veinPathPrefab, transform.position, Quaternion.identity);

            Vector3 cellPosition = GetRandom(transform.position);
            cellPosition.y += Y_OFFSET;
            lineRenderer.SetPosition(0, cellPosition);

            Vector3 neighborPosition = neighbor.cell.Transform.position;
            neighborPosition.y += Y_OFFSET;
            lineRenderer.SetPosition(1, neighborPosition);
        }
    }

    private Vector3 GetRandom(Vector3 position)
    {
        position.x += GetRandomFloat();
        position.y += GetRandomFloat();
        position.z += GetRandomFloat();
        return position;
    }

    private float GetRandomFloat() => Random.RandomRange(-RANDOM_FLOAT, RANDOM_FLOAT);

    #endregion


    #region Private and Protected

    private const float RANDOM_FLOAT = 0.5f;
    private const float Y_OFFSET = 0.3f;

    #endregion
}
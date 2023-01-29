using UnityEngine;

public class TentacleGenerator : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private GameNodeUI _gameNode;

    [SerializeField]
    private Tentacle _tentacleMediumPrefab;

    [SerializeField]
    private Tentacle _tentacleLongPrefab;

    public Transform Transform => _transform ? _transform : _transform = GetComponent<Transform>();

    #endregion


    #region Properties

    public GameNodeUI GameNode => _gameNode;

    #endregion


    #region Unity API

    private void Awake()
    {
        GenerateMediumTentacles();
        GenerateLongTentacles();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Transform gameNodeTransform = _gameNode.transform;

        for (int i = 0; i < _gameNode.closeNeighbors.Count; i++)
        {
            Gizmos.DrawLine(gameNodeTransform.position, _gameNode.closeNeighbors[i].transform.position);
        }

        Gizmos.color = Color.yellow;
        for (int i = 0; i < _gameNode.farNeighbors.Count; i++)
        {
            Gizmos.DrawLine(gameNodeTransform.position, _gameNode.farNeighbors[i].transform.position);
        }
    }

    #endregion


    #region Main

    private void GenerateMediumTentacles()
    {
        var neighbors = _gameNode.closeNeighbors;
        foreach (var neighbor in neighbors)
        {
            var tentacle = Instantiate(_tentacleMediumPrefab, Transform.position, Quaternion.identity);
            tentacle.Transform.LookAt(neighbor.cell.transform.position);
            tentacle.GameNode = GameNode;
            tentacle.Target = neighbor;
        }
    }

    private void GenerateLongTentacles()
    {
        var neighbors = _gameNode.farNeighbors;
        foreach (var neighbor in neighbors)
        {
            var tentacle = Instantiate(_tentacleLongPrefab, Transform.position, Quaternion.identity);
            tentacle.Transform.LookAt(neighbor.cell.transform.position);
            tentacle.GameNode = GameNode;
            tentacle.Target = neighbor;
        }
    }

    #endregion


    #region Private And Protected

    private Transform _transform;

    #endregion
}
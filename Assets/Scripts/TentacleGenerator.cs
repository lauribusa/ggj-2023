using System.Collections.Generic;
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

    #endregion


    #region Properties

    public GameNodeUI GameNode => _gameNode;

    public Transform Transform => _transform ? _transform : _transform = GetComponent<Transform>();

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
        const float radius = 0.2f;

        for (int i = 0; i < _gameNode.closeNeighbors.Count; i++)
        {
            Vector3 neighborPosition = _gameNode.closeNeighbors[i].transform.position;
            Gizmos.DrawLine(gameNodeTransform.position, neighborPosition);
            Gizmos.DrawSphere(Vector3.Lerp(gameNodeTransform.position, neighborPosition, 0.1f), radius);
        }

        Gizmos.color = Color.yellow;
        for (int i = 0; i < _gameNode.farNeighbors.Count; i++)
        {
            Vector3 neighborPosition = _gameNode.farNeighbors[i].transform.position;
            Gizmos.DrawLine(gameNodeTransform.position, neighborPosition);
            Gizmos.DrawSphere(Vector3.Lerp(gameNodeTransform.position, neighborPosition, 0.1f), radius);
        }
    }

    #endregion


    #region Main

    private void GenerateMediumTentacles() => GenerateTentacles(_gameNode.closeNeighbors, _tentacleMediumPrefab);

    private void GenerateLongTentacles() => GenerateTentacles(_gameNode.farNeighbors, _tentacleLongPrefab);

    private void GenerateTentacles(List<GameNodeUI> neighbors, Tentacle tentaclePrefab)
    {
        foreach (var neighbor in neighbors)
        {
            var tentacle = Instantiate(tentaclePrefab, Transform.position, Quaternion.identity);
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
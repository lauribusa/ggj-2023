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


    #region Unity API

    private void Awake()
    {
        GenerateMediumTentacles();
        GenerateLongTentacles();
    }

    #endregion


    #region Main

    private void GenerateMediumTentacles()
    {
        var neighbors = _gameNode.closeNeighbors;
        foreach (var neighbor in neighbors)
        {
            var tentacle = Instantiate(_tentacleMediumPrefab, Transform);
            tentacle.Transform.LookAt(neighbor.transform.position);
            tentacle.Deploy();
        }
    }

    private void GenerateLongTentacles()
    {
        var neighbors = _gameNode.farNeighbors;
        foreach (var neighbor in neighbors)
        {
            var tentacle = Instantiate(_tentacleLongPrefab, Transform);
            tentacle.Transform.LookAt(neighbor.transform.position);
            tentacle.Deploy();
        }
    }

    #endregion


    #region Private And Protected

    private Transform _transform;

    #endregion
}
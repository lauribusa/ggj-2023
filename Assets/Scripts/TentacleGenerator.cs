using UnityEngine;

public class TentacleGenerator : MonoBehaviour
{
    #region Exposed

    [field:SerializeField]
    public Transform[] Neighbors { get; private set; }

    [SerializeField]
    private Tentacle _tentacleMediumPrefab;

    [SerializeField]
    private Tentacle _tentacleLongPrefab;

    public Transform Transform => _transform ? _transform : _transform = GetComponent<Transform>();

    #endregion


    #region Unity API

    private void Awake() => GenerateTentacles();

    private void OnDrawGizmos()
    {
        for (int i = 0; i < Neighbors.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, Neighbors[i].position);
        }
    }

    #endregion


    #region Main

    private void GenerateTentacles()
    {
        var neighbors = GetComponent<TentacleGenerator>().Neighbors;
        foreach (var neighbor in neighbors)
        {
            var tentacle = Instantiate(_tentacleLongPrefab, Transform);
            tentacle.Transform.LookAt(neighbor.position);
        }
    }

    #endregion


    #region Private And Protected

    private Transform _transform;

    #endregion
}
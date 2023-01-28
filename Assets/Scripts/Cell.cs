using UnityEngine;

public class Cell : MonoBehaviour
{
	#region Exposed

	[SerializeField]
	private GameObject _neutralCell;

    [SerializeField]
    private GameObject _immuneCell;

    [SerializeField]
    private GameObject _corruptCell;

    [SerializeField]
    private Tentacle _tentacleMediumPrefab;

    [SerializeField]
    private Tentacle _tentacleLongPrefab;

    #endregion


    #region Unity API

    private void Awake()
    {
        GenerateTentacles();
    }

    private void Update()
    {
        
    }
	
	#endregion
	
	
	#region Utils
	
	public void SetNeutral()
	{
        _neutralCell.SetActive(true);
        _immuneCell.SetActive(false);
        _corruptCell.SetActive(false);
    }

    public void SetImmune()
    {
        _neutralCell.SetActive(false);
        _immuneCell.SetActive(true);
        _corruptCell.SetActive(false);
    }

    public void SetCorrupt()
    {
        _neutralCell.SetActive(false);
        _immuneCell.SetActive(false);
        _corruptCell.SetActive(true);
    }

    private void GenerateTentacles()
    {
        var neighbors = GetComponent<CellNeighborsChecker>().Neighbors;
        foreach (var neighbor in neighbors)
        {
            var tentacle = Instantiate(_tentacleLongPrefab, transform);
            tentacle.Transform.LookAt(neighbor.position);
        }
    }

    #endregion
}
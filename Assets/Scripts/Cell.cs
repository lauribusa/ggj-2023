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
    private GameObject _tentacleMediumPrefab;

    [SerializeField]
    private GameObject _tentacleLongPrefab;

    #endregion


    #region Unity API

    private void Awake()
    {
        
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

    #endregion


    #region Private and Protected



    #endregion
}
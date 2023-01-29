using UnityEngine;

public class Cell : MonoBehaviour
{
    #region Exposed

    [SerializeField]
    private float _maxScale = 3f;

    [SerializeField]
    private GameNodeUI _gameNode;

	[SerializeField]
	private GameObject _neutralCell;

    [SerializeField]
    private GameObject _immuneCell;

    [SerializeField]
    private GameObject _corruptCell;

    #endregion


    #region Properties

    public Transform Transform => _transform ? _transform : _transform = GetComponent<Transform>();

    #endregion


    #region Unity API

    private void Update()
    {
        Grow();
    }

    #endregion


    #region Main

    private void Grow()
    {
        float maxNodeValue = GameManager.Instance.globalMaxCharge;
        float normalizedValue = _gameNode.NodeValue / maxNodeValue;
        normalizedValue *= _maxScale;

        float scale = Mathf.Clamp(normalizedValue, 1f, _maxScale);
        Transform.localScale = new Vector3(scale, scale, scale);
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


    #region Private And Protected

    private Transform _transform;

    #endregion
}
using UnityEngine;
using UnityEngine.UIElements;

public class Cell : MonoBehaviour
{
    #region Exposed

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
        float maxNodeValue = 100f;
        float maxScale = 3f;

        float value = _gameNode.NodeValue / maxNodeValue;
        value *= maxScale;

        float scale = Mathf.Clamp(value, 1f, maxScale);

        var newScale = new Vector3(scale, scale, scale);
        Transform.localScale = newScale;
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
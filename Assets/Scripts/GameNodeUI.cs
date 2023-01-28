using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameNodeUI : MonoBehaviour
{
    #region Exposed
    public Cell cell;
    public TextMeshProUGUI valueText;
    public Button interactable;
    public List<GameNodeUI> closeNeighbors;
    public List<GameNodeUI> farNeighbors;
    
    public Faction CurrentFaction
    {
        get { return _currentFaction; }
        set { _currentFaction = value; }
    }
    [HideInInspector] public List<GameNodeUI> neighbors = new List<GameNodeUI>();
    
    public int NodeValue
    {
        get
        {
            return nodeValue;
        }
        set
        {
            nodeValue = value;
            NodeValueDisplayUpdate(nodeValue);
        }
    }
    [Header("Node values")]
    public int chargeRate;
    public bool isFactionMainNode;
    [HideInInspector]
    public bool isLinked;
    #endregion


    #region Private And Protected
    [SerializeField]
    private Faction _currentFaction;
    private float _timeElapsed;

    [SerializeField]
    private int nodeValue;

    

	#endregion

	
	#region Unity API
	
    private void Awake()
    {
        neighbors = closeNeighbors.Concat(farNeighbors).ToList();
        GameManager.Instance.AddToNodeList(this);
        NodeValueDisplayUpdate(nodeValue);
    }

    private void OnDrawGizmosSelected()
    {
        neighbors.ForEach(neighbor =>
        {
            Gizmos.DrawLine(transform.position, neighbor.transform.position);
        });
    }

    public void Update()
    {
        if (isLinked) return;
        _timeElapsed += Time.deltaTime;
        PassiveCharge();
    }
    
    #endregion
    

    #region Main
    public void IncreasePower()
    {
        nodeValue += chargeRate;
    }
    public void DecreasePower() 
    { 
    
    }
    public void PassiveCharge()
    {
        if (CurrentFaction == Faction.Neutral) return;
        if(NodeValue >= GameManager.Instance.globalMaxCharge)
        {
            NodeValue = GameManager.Instance.globalMaxCharge;
            return;
        }
        if (_timeElapsed > GameManager.Instance.passiveChargeInterval)
        {
            _timeElapsed = 0;
            NodeValue = NodeValue + GameManager.Instance.globalChargeRate;
        }
    }

    public void NodeValueDisplayUpdate(int value)
    {
        valueText.text = value.ToString();
    }

    public void OnClickNode()
    {
        GameManager.Instance.NodeSelectedEvent?.Invoke(this);
    }
    #endregion


    #region Singleton

    #endregion
}


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
    public TextMeshProUGUI modifierText;
    public Button interactable;
    public Image cursor;
    public List<GameNodeUI> closeNeighbors;
    public List<GameNodeUI> farNeighbors;
    
    public Faction CurrentFaction
    {
        get { return _currentFaction; }
        set 
        {
            _currentFaction = value;
            ChangeFactionColor();
        }
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
    private int _chargeRate;
    public int chargeRate
    {
        get { return _chargeRate; }
        set
        {
            _chargeRate = value;
            modifierText.text = $"{_chargeRate} + {bonusCharge}";
        }
    }
    public bool isFactionMainNode;
    [HideInInspector]
    private int _bonusCharge;
    public int bonusCharge
    {
        get { return _bonusCharge; }
        set
        {
            _bonusCharge = value;
            modifierText.text = $"{_chargeRate} + {bonusCharge}";
        }
    }
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
        ChangeFactionColor();
        if(!isFactionMainNode) chargeRate = GameManager.Instance.globalChargeRate;
        if (CurrentFaction == Faction.Neutral) chargeRate = 0;
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

    public void ChargeUp()
    {
        nodeValue += chargeRate;
    }

    public void SetSelected()
    {
        cursor.gameObject.SetActive(true);
    }

    public void UnsetSelected()
    {
        cursor.gameObject.SetActive(false);
    }

    public void ChangeFactionColor()
    {
        switch (CurrentFaction)
        {
            case Faction.Neutral:
                interactable.image.color = Color.black;
                valueText.color = Color.black;
                if(cell != null)
                {
                    cell.SetNeutral();
                }
                break;
            case Faction.ImmuneSystem:
                interactable.image.color = Color.blue;
                valueText.color = Color.cyan;
                if (cell != null)
                {
                    cell.SetImmune();
                }
                break;
            case Faction.Parasite:
                interactable.image.color = Color.red;
                valueText.color = Color.magenta;
                if (cell != null)
                {
                    cell.SetCorrupt();
                }
                break;
            default:
                break;
        }
       
    }
    public void PassiveCharge()
    {
        //Debug.Log($"Node Info Charge: faction: {CurrentFaction} value: {NodeValue} charge: {chargeRate} + {bonusCharge}");
        //if (CurrentFaction == Faction.Neutral) return;
        if(NodeValue >= GameManager.Instance.globalMaxCharge)
        {
            NodeValue = GameManager.Instance.globalMaxCharge;
            return;
        }
        if (_timeElapsed > GameManager.Instance.passiveChargeInterval)
        {
            _timeElapsed = 0;
            
            NodeValue = NodeValue + chargeRate + bonusCharge;
        }
    }

    public void NodeValueDisplayUpdate(int value)
    {
        valueText.text = value.ToString();
    }

    public void OnClickNode()
    {
        GameManager.Instance.NodeClickedEvent?.Invoke(this);
    }
    #endregion


    #region Singleton

    #endregion
}


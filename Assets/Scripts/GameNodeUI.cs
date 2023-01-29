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
            UpdateBaseChargeRate();
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
    public int baseCharge
    {
        get { return _chargeRate; }
        set
        {
            _chargeRate = value;
            modifierText.text = $"{_chargeRate+chargeModifier}";
        }
    }
    public bool isFactionMainNode;
    [HideInInspector]
    private int _chargeModifier;
    public int chargeModifier
    {
        get { return _chargeModifier; }
        set
        {
            _chargeModifier = value;
            modifierText.text = $"{baseCharge+_chargeModifier}";
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
        GameManager.Instance.AddToNodeList(this);
    }


    private void Start()
    {
        neighbors = closeNeighbors.Concat(farNeighbors).ToList();
        NodeValueDisplayUpdate(nodeValue);
        ChangeFactionColor();
        UpdateBaseChargeRate();
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
        _timeElapsed += Time.deltaTime;
        PassiveCharge();
    }
    
    #endregion
    

    #region Main

    public void ChargeUp()
    {
        int bonus = 0;
        if(NodeValue < GameManager.Instance.highChargeThreshold2)
        {
            bonus = 1;
        }
        if(NodeValue < GameManager.Instance.lowChargeThreshold1)
        {
            bonus = 2;
        }
        if(CurrentFaction == Faction.Neutral || isFactionMainNode || isLinked)
        {
            bonus = 0;
        }
        NodeValue += baseCharge + chargeModifier + bonus;
    }

    public void SetSelected()
    {
        cursor.gameObject.SetActive(true);
    }

    public void UnsetSelected()
    {
        cursor.gameObject.SetActive(false);
    }

    public void UpdateBaseChargeRate()
    {
        switch (CurrentFaction)
        {
            case Faction.Neutral:
                baseCharge = 0;
                break;
            case Faction.ImmuneSystem:
                baseCharge = GameManager.Instance.globalChargeRate;
                break;
            case Faction.Parasite:
                baseCharge = GameManager.Instance.globalChargeRate;
                break;
            default:
                break;
        }
    }

    public void ChangeFactionColor()
    {
        switch (CurrentFaction)
        {
            case Faction.Neutral:
                interactable.image.color = Color.black;
                valueText.color = Color.black;
                modifierText.color = Color.black;
                if(cell != null)
                {
                    cell.SetNeutral();
                }
                break;
            case Faction.ImmuneSystem:
                interactable.image.color = Color.blue;
                valueText.color = Color.cyan;
                modifierText.color = Color.cyan;
                if (cell != null)
                {
                    cell.SetImmune();
                }
                break;
            case Faction.Parasite:
                interactable.image.color = Color.red;
                valueText.color = Color.magenta;
                modifierText.color = Color.magenta;
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
        if (_timeElapsed < GameManager.Instance.passiveChargeInterval) return;
        _timeElapsed = 0;
        ChargeUp();
        if (NodeValue >= GameManager.Instance.globalMaxCharge)
        {
            NodeValue = GameManager.Instance.globalMaxCharge;
        }
        if (NodeValue < 0)
        {
            NodeValue = 0;
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


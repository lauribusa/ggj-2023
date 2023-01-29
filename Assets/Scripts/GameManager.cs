using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum Faction
{
    Neutral,
    ImmuneSystem,
    Parasite
}
public class NodeEvent : UnityEvent<GameNodeUI> { }
public class LinkEvent : UnityEvent<GameNodeUI, GameNodeUI> { }
public class GameManager : MonoBehaviour
{
    #region Exposed
    public UnityEvent<GameNodeUI> NodeSelectedEvent;
    public UnityEvent<GameNodeUI> NodeUnselectedEvent;
    public UnityEvent<GameNodeUI> NodeClickedEvent;

    public UnityEvent<GameNodeUI, GameNodeUI> linkCreatedEvent;
    public UnityEvent<GameNodeUI, GameNodeUI> linkDestroyedEvent;

    public UnityEvent<GameNodeUI, Faction> factionChangedEvent;

    [Tooltip("Passes true in param if the player wins")]
    public UnityEvent<bool> gameEndEvent;
    private GameNodeUI _playerSelectedOrigin;
    public GameNodeUI playerSelectedOrigin
    {
        get
        {
            return _playerSelectedOrigin;
        }
        set
        {
            if(_playerSelectedOrigin != null)
            {
                _playerSelectedOrigin.UnsetSelected();
            }
            if (value != null)
            {
                value.SetSelected();
            }
            _playerSelectedOrigin = value;
        }
    }
    public GameNodeUI playerSelectedDestination;

    public List<NodeLink> existingLinks = new List<NodeLink>();

    public List<GameNodeUI> existingNodes = new List<GameNodeUI>();

    public int globalDrainRate;
    public int globalChargeRate;
    public int globalMaxCharge = 60;
    public int lowChargeThreshold1 = 30;
    public int highChargeThreshold2 = 30;

    [Tooltip("Passive charge interval in seconds")]
    public float passiveChargeInterval;
    [Tooltip("Speed of draining/Empowering nodes in seconds")]
    public float processingRate;
    #endregion

    #region Private
    private float _elapsedTime;
    private List<Coroutine> activeLinks = new List<Coroutine>();
    #endregion

    #region Unity API
    private void Awake()
    {
        if (FindObjectsOfType(typeof(GameManager)).Length > 1)
        {
            Debug.LogWarning("Already found instance of GameManager in scene; destroying.");
            DestroyImmediate(gameObject);
        }
        factionChangedEvent?.AddListener(OnNodeFactionChange);
        gameEndEvent?.AddListener(OnGameEnded);
        linkCreatedEvent?.AddListener(OnLinkCreated);
        linkDestroyedEvent?.AddListener(OnLinkDestroyed);
        NodeClickedEvent?.AddListener(OnNodeClicked);
        NodeSelectedEvent?.AddListener(OnNodeSelected);
        NodeUnselectedEvent?.AddListener(OnNodeUnselected);

    }
    private void Start()
    {
        var AInodes = existingNodes.Where(x => x.CurrentFaction == Faction.ImmuneSystem).ToList();
        ImmuneSystem.Instance.SetStartNodes(AInodes);
    }

    private void Update()
    {
        if (!existingLinks.Any())
        {
            if(_elapsedTime != 0) _elapsedTime = 0;
            return;
        }
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime < processingRate) return;
        _elapsedTime = 0;
        for (int i = 0; i < existingLinks.Count; i++)
        {
            existingLinks[i].Process();
        }
    }

    #endregion


    #region Main

    public void AddToNodeList(GameNodeUI gameNodeUI)
    {
        existingNodes.Add(gameNodeUI);
    }

   
    public void CheckIfNodeIsAlreadySelected(GameNodeUI node)
    {
        if (playerSelectedOrigin == node)
        {
            Debug.Log("Already an origin.");
            return;
        }
        if (playerSelectedDestination == node)
        {
            Debug.Log("Already a destination.");
        }
    }

    public void OnGameEnded(bool hasPlayerWon)
    {
        Debug.Log($"Game ended. Player win? {hasPlayerWon}");
    }

    public void OnNodeClicked(GameNodeUI node)
    {
        Debug.Log($"Node clicked: {node}");

        if (playerSelectedOrigin is null)
        {
            if (node.CurrentFaction != Faction.Parasite) return;
            Debug.Log($"Has been added to origin");
            playerSelectedOrigin = node;
            return;
        }
        if (!playerSelectedOrigin.neighbors.Contains(node))
        {
            Debug.Log($"Is not a valid neighbor.");
            playerSelectedOrigin = null;
            return;
        }
        var linkExists = CheckIfLinkAlreadyExists(playerSelectedOrigin, node);
        if(linkExists) 
        {
            Debug.Log($"Link already exists");
        }
        if (playerSelectedOrigin is not null && node.CurrentFaction != Faction.Parasite)
        {
            playerSelectedOrigin = null;
        }

        //linkCreatedEvent?.Invoke(playerSelectedOrigin, node);

    }

    public bool CheckIfLinkAlreadyExists(GameNodeUI from, GameNodeUI to)
    {
        if (from == null || to == null) return true;
        var fromTo = existingLinks.Any(link => link.from == from && link.to == to);
        var toFrom = existingLinks.Any(link => link.to == from && link.from == to);
        return toFrom || fromTo;
    }

    public void OnNodeUnselected(GameNodeUI node)
    {
        Debug.Log($"Node unselected: {node}");
    }

    public void OnNodeSelected(GameNodeUI node)
    {
        Debug.Log($"Node selected: {node}");
    }

    public void OnNodeFactionChange(GameNodeUI node, Faction newFaction)
    {
        if(node.CurrentFaction == Faction.ImmuneSystem)
        {
            ImmuneSystem.Instance.RemoveNodeFromCapturedList(node);
        }
        node.CurrentFaction = newFaction;
        if(newFaction == Faction.ImmuneSystem) ImmuneSystem.Instance.AddNodeToCapturedList(node);
        
        if (node.isFactionMainNode)
        {
            node.isFactionMainNode = false;
        }
        if (!existingNodes.Any(x => x.CurrentFaction == Faction.ImmuneSystem))
        {
            gameEndEvent?.Invoke(true);
        }
        if(!existingNodes.Any(x => x.CurrentFaction == Faction.Parasite))
        {
            gameEndEvent?.Invoke(false);
        }

    }

    public void FlushSelectedNodes()
    {
        playerSelectedOrigin = null;
        playerSelectedDestination = null;
    }

    public void OnLinkDestroyed(GameNodeUI from, GameNodeUI to)
    {
        Debug.Log($"Link being destroyed: from: {from} ({from.CurrentFaction} : {from.baseCharge} + {from.chargeModifier}, to: {to} {to.CurrentFaction} {to.baseCharge} + {to.chargeModifier}");
/*
        if (from.CurrentFaction == to.CurrentFaction)
        {
            from.chargeRate++;
            to.chargeRate--;
        }
        else
        {
            from.chargeRate++;
            to.chargeRate++;
            to.chargeRate++;
        }*/
        from.chargeModifier = 0;
        to.chargeModifier = 0;
        Debug.Log($"Link destroyed: from: {from} ({from.CurrentFaction} : {from.baseCharge} + {from.chargeModifier}, to: {to} {to.CurrentFaction} {to.baseCharge} + {to.chargeModifier}");
    }
    public void OnLinkCreated(GameNodeUI from, GameNodeUI to)
    {
        Debug.Log($"Link created: from: {from}, to: {to}");
        
        var nodeLink = new NodeLink(from, to);
        existingLinks.Add(nodeLink);
    }

    #endregion


    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance != null)
                    DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    #endregion
}


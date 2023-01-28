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
public class NodeEvent: UnityEvent<GameNodeUI> { }
public class LinkEvent: UnityEvent<GameNodeUI, GameNodeUI> { }
public class GameManager : MonoBehaviour
{
    #region Exposed
    public UnityEvent<GameNodeUI> NodeSelectedEvent;
    public UnityEvent<GameNodeUI> NodeUnselectedEvent;
    public UnityEvent<GameNodeUI> NodeClickedEvent;

    public UnityEvent<GameNodeUI, GameNodeUI> linkCreatedEvent;
    public UnityEvent<GameNodeUI, GameNodeUI> linkDestroyedEvent;

    public GameNodeUI playerSelectedOrigin;
    public GameNodeUI playerSelectedDestination;

    public List<NodeLink> existingLinks = new List<NodeLink>();

    public List<GameNodeUI> existingNodes = new List<GameNodeUI>();

    public int globalDrainRate;
    public int globalChargeRate;

    [Tooltip("Passive charge interval in seconds")]
    public float passiveChargeInterval;
    #endregion

    #region Unity API
    private void Awake()
    {
        if(FindObjectsOfType<GameManager>().Length > 1)
        {
            Debug.LogWarning("Found another GameManager, destroying.");
            Destroy(this);
        }
        _instance = this;
    }
    private void Start()
    {
        var AInodes = existingNodes.Where(x => x.CurrentFaction == Faction.ImmuneSystem).ToList();
        ImmuneSystem.Instance.SetStartNodes(AInodes);
    }

    private void Update()
    {
        if(existingLinks.Count > 0)
        {

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
        if(playerSelectedOrigin == node)
        {
            Debug.Log("Already an origin.");
            return;
        }
        if(playerSelectedDestination == node)
        {
            Debug.Log("Already a destination.");
        }
    }

    public void OnNodeClicked(GameNodeUI node)
    {
        if(playerSelectedOrigin is null)
        {
            playerSelectedOrigin = node;
        }
        if(playerSelectedDestination is null)
        {
            playerSelectedDestination = node;
        }
    }

    public void CheckIfLinkIsFormed()
    {
        if(playerSelectedOrigin.neighbors.Contains(playerSelectedDestination))
        {
            linkCreatedEvent?.Invoke(playerSelectedOrigin, playerSelectedDestination);
        }
    }

    public void FlushSelectedNodes()
    {
        playerSelectedOrigin = null;
        playerSelectedDestination = null;
    }

    #endregion


    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    #endregion
}


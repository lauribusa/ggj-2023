using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImmuneSystem : MonoBehaviour
{
    #region Exposed
    [Tooltip("Time in seconds before AI attacks")]
    public float timeBeforeAction;
    #endregion


    #region Private And Protected
    private float _timeElapsed;
    private List<GameNodeUI> _controlledNodes = new List<GameNodeUI>();
    #endregion


    #region Unity API

    private void Start()
    {

    }

    private void Update()
    {
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed > timeBeforeAction)
        {
            LookForWeakestNeighbor();
            _timeElapsed= 0;
        }
    }

    #endregion


    #region Main
    public void SetStartNodes(List<GameNodeUI> nodes)
    {
        _controlledNodes = nodes;
    }
    public void LookForWeakestNeighbor()
    {
        HashSet<GameNodeUI> allNeighbors = new HashSet<GameNodeUI>();
        _controlledNodes.ForEach(node =>
        {
            node.neighbors.ForEach(neighbor =>
            {
                if (neighbor.CurrentFaction != Faction.ImmuneSystem)
                {
                    allNeighbors.Add(neighbor);
                }
            });
        });
        var weakestNeighbor = allNeighbors.OrderBy(x => x.NodeValue).First();
        AttackNode(weakestNeighbor);
    }

    public void AttackNode(GameNodeUI node)
    {
        Debug.Log($"Immune System is attempting a move on node {node} ({node.CurrentFaction} : {node.NodeValue})");
    }

    public void AddNodeToCapturedList(GameNodeUI node)
    {
        _controlledNodes.Add(node);
    }

    public void RemoveNodeFromCapturedList(GameNodeUI node)
    {
        _controlledNodes.Remove(node);
    }
    #endregion


    #region Singleton
    private static ImmuneSystem _instance;
    public static ImmuneSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ImmuneSystem>();
                if (_instance != null)
                    DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    #endregion
}


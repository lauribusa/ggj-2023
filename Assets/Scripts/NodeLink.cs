using UnityEngine;
using UnityEngine.Events;

public class NodeLink
{
    public GameNodeUI from;
    private int fromInitialValue;
    public GameNodeUI to;
    private int toInitialValue;

    public NodeLink(GameNodeUI from, GameNodeUI to)
    {
        this.from = from;
        this.to = to;
        fromInitialValue = from.NodeValue;
        toInitialValue = to.NodeValue;
        from.isLinked = true;
        to.isLinked = true;
    }

    public void DestroyLink()
    {
        from.isLinked = false;
        to.isLinked = false;
        GameManager.Instance.linkDestroyedEvent?.Invoke(from, to);
        GameManager.Instance.existingLinks.Remove(this);
    }
    public void Process()
    {
        if(from.CurrentFaction == to.CurrentFaction) 
        {
            GivePower(from, to);
            return;
        }
        DrainPower(from, to);
    }

    public void DrainPower(GameNodeUI from, GameNodeUI to)
    {
        from.NodeValue -= GameManager.Instance.globalDrainRate;
        if(from.NodeValue <= 0 )
        {
            FromNodeIsDepleted();
        }
        to.NodeValue -= GameManager.Instance.globalDrainRate;
        if(from.NodeValue <= 0)
        {
            ToNodeIsDepleted();
        }
    }


    public void GivePower(GameNodeUI from, GameNodeUI to)
    {
        if(from.NodeValue > (fromInitialValue / 2) || to.NodeValue >= GameManager.Instance.globalMaxCharge) 
        {
            GameManager.Instance.linkDestroyedEvent?.Invoke(from, to);
            return;
        }
        from.NodeValue -= GameManager.Instance.globalDrainRate;
        to.NodeValue += GameManager.Instance.globalDrainRate;
    }

    public void FromNodeIsDepleted()
    {
        switch (from.CurrentFaction)
        {
            case Faction.Neutral:
                break;
            case Faction.ImmuneSystem:
                GameManager.Instance.factionChangedEvent?.Invoke(from, Faction.Neutral);
                break;
            case Faction.Parasite:
                GameManager.Instance.factionChangedEvent?.Invoke(from, Faction.Neutral);
                break;
            default:
                break;
        }
    }

    public void ToNodeIsDepleted()
    {
        switch (to.CurrentFaction)
        {
            case Faction.Neutral:
                GameManager.Instance.factionChangedEvent?.Invoke(to, from.CurrentFaction);
                break;
            case Faction.ImmuneSystem:
                GameManager.Instance.factionChangedEvent?.Invoke(to, Faction.Neutral);
                break;
            case Faction.Parasite:
                GameManager.Instance.factionChangedEvent?.Invoke(to, Faction.Neutral);
                break;
            default:
                break;
        }
        
    }
}


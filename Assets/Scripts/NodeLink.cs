using UnityEngine;
using UnityEngine.Events;

public class NodeLink
{
    public GameNodeUI from;
    private int fromInitialValue;
    public GameNodeUI to;
    private int toInitialValue;

    //TODO
    // Faire en sorte que les nodes ne font pas juste d�penser leur points mais avoir un d�bit constant qui se d�pense dans les liens.
    // node principale as + de d�bit?

    //TODO 2
    // condition de victoire: it�rer les nodes et v�rifier si une node appartiens encore a une faction

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
        if(to.NodeValue <= 0)
        {
            ToNodeIsDepleted();
        }
    }


    public void GivePower(GameNodeUI from, GameNodeUI to)
    {
        if(from.NodeValue < (fromInitialValue / 2) || to.NodeValue >= GameManager.Instance.globalMaxCharge) 
        {
            DestroyLink();
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
        DestroyLink();
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
        DestroyLink();
    }
}


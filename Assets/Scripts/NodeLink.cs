using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class NodeLink
{
    public GameNodeUI from;
    private int fromInitialValue;
    public Faction fromInitialFaction;
    public GameNodeUI to;
    public Faction toInitialFaction;

    //TODO
    // Faire en sorte que les nodes ne font pas juste dépenser leur points mais avoir un débit constant qui se dépense dans les liens.
    // node principale as + de débit?

    //TODO 2
    // condition de victoire: itérer les nodes et vérifier si une node appartiens encore a une faction

    public NodeLink(GameNodeUI from, GameNodeUI to)
    {
        this.from = from;
        this.to = to;
        fromInitialValue = from.NodeValue;
        fromInitialFaction = from.CurrentFaction;
        toInitialFaction = to.CurrentFaction;
        if(from.CurrentFaction == to.CurrentFaction) 
        { 
            DecreaseFromModifier();
            IncreaseToModifier();
        } else
        {
            DecreaseFromModifier();
            DecreaseToModifier();
        }
        from.isLinked = true;
        to.isLinked = true;
    }

    public void DestroyLink()
    {
        from.isLinked = false;
        to.isLinked = false;
        if (fromInitialFaction == toInitialFaction)
        {
            IncreaseFromModifier();
            DecreaseToModifier();
        }
        else
        {
            IncreaseFromModifier();
            IncreaseToModifier();
        }
        GameManager.Instance.linkDestroyedEvent?.Invoke(from, to);
        GameManager.Instance.existingLinks.Remove(this);
    }
    public void Process()
    {
        if (from.CurrentFaction == to.CurrentFaction)
        {
            GivePower(from, to);
            return;
        }
        DrainPower(from, to);
    }

    private void IncreaseFromModifier()
    {
        from.chargeModifier++;
    }

    private void DecreaseFromModifier()
    {
        from.chargeModifier--;
    }

    private void IncreaseToModifier()
    {
        to.chargeModifier++;
    }

    private void DecreaseToModifier()
    {
        to.chargeModifier--;
    }

    public void DrainPower(GameNodeUI from, GameNodeUI to)
    {

        if (from.NodeValue <= 0)
        {
            FromNodeIsDepleted();
        }
        
        if (to.NodeValue <= 0)
        {
            ToNodeIsDepleted();
        }
    }


    public void GivePower(GameNodeUI from, GameNodeUI to)
    {
        if (from.NodeValue < (fromInitialValue / 2) || to.NodeValue >= GameManager.Instance.globalMaxCharge)
        {
            DestroyLink();
            return;
        }
    }

    public void FromNodeIsDepleted()
    {
        DestroyLink();
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
        DestroyLink();
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


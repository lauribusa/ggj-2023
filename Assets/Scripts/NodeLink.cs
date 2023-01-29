using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class NodeLink
{
    public GameNodeUI from;
    private int fromInitialValue;
    public GameNodeUI to;
    private int toInitialValue;

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
        toInitialValue = to.NodeValue;
        if(from.CurrentFaction == to.CurrentFaction) 
        { 
            from.chargeRate--;
            to.chargeRate++;
        } else
        {
            from.chargeRate--;
            to.chargeRate--;
            to.chargeRate--;
        }
        from.isLinked = true;
        to.isLinked = true;
    }

    public void DestroyLink()
    {
        from.isLinked = false;
        to.isLinked = false;
        from.bonusCharge = 0;
        to.bonusCharge = 0;
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

    public void DrainPower(GameNodeUI from, GameNodeUI to)
    {
        //from.ChargeUp();

        if (to.NodeValue < GameManager.Instance.lowChargeThreshold1)
        {
            from.bonusCharge = -2;
            to.bonusCharge = 2;
        }
        if (to.NodeValue < GameManager.Instance.highChargeThreshold2)
        {
            from.bonusCharge = -1;
            to.bonusCharge = 1;
        }
        from.bonusCharge = 0;
        to.bonusCharge = 0;

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
        //from.ChargeUp();
        //to.ChargeUp();
        if (from.NodeValue < (fromInitialValue / 2) || to.NodeValue >= GameManager.Instance.globalMaxCharge)
        {
            DestroyLink();
            return;
        }
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


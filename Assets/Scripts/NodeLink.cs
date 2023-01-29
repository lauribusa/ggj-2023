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
            from.chargeModifier--;
            to.chargeModifier++;
        } else
        {
            from.chargeModifier--;
            to.chargeModifier--;
        }
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
        if (from.CurrentFaction == to.CurrentFaction)
        {
            GivePower(from, to);
            return;
        }
        DrainPower(from, to);
    }

    public void DrainPower(GameNodeUI from, GameNodeUI to)
    {
        // trouver une façon de mettre les bonus sans que ca sois casse couille;

        /* if (to.NodeValue < GameManager.Instance.lowChargeThreshold1)
         {
             from.chargeModifier = -2;
             to.chargeModifier = -2;
         } else
         if (to.NodeValue < GameManager.Instance.highChargeThreshold2 && to.NodeValue > GameManager.Instance.lowChargeThreshold1)
         {
             from.chargeModifier = -2;
             to.chargeModifier = -2;
         } else
         {
             from.chargeModifier = -1;
             to.chargeModifier = -1;
         }*/

        if (from.NodeValue <= 0)
        {
            FromNodeIsDepleted();
            from.chargeModifier++;
            to.chargeModifier--;
        }
        
        if (to.NodeValue <= 0)
        {
            ToNodeIsDepleted();
            from.chargeModifier++;
            to.chargeModifier--;
        }
    }


    public void GivePower(GameNodeUI from, GameNodeUI to)
    {
        // trouver une façon de mettre les bonus sans que ca sois casse couille;
        /*if (to.NodeValue < GameManager.Instance.highChargeThreshold2)
        {
            from.chargeModifier += -2;
            to.chargeModifier += 2;
        }
        else
        {
            from.chargeModifier = -1;
            to.chargeModifier = 1;
        }*/
        if (from.NodeValue < (fromInitialValue / 2) || to.NodeValue >= GameManager.Instance.globalMaxCharge)
        {
            from.chargeModifier++;
            to.chargeModifier++;
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

